using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using AwesomiumSharp;
using DanTup.GPlusNotifier.Properties;

namespace DanTup.GPlusNotifier
{
	public partial class MainForm : Form
	{
		GooglePlusClient googlePlusClient;

		/// <summary>
		/// Used for installing updates.
		/// </summary>
		Updater updater = new Updater();

		LoginForm loginForm;
		NotificationsForm notificationsForm;

		/// <summary>
		/// Used to force a notification if 0 messages (eg. at startup, or forced check).
		/// </summary>
		bool forceNotification = true;

		// Used to reduce the change of showing the same balloon notification over and over.
		int? lastNotificationCount;

		// Whether the user appears to be logged in
		bool isLoggedIn = false;

		// If the user cancels a login, we don't want to keep popping the form up until they ask (context menu)
		bool userHasCancelledPreviousLogin = false;

		// Default notifiers (this will be tied to config in some way)
		ConcurrentBag<INotifier> notifiers = new ConcurrentBag<INotifier>();

		// Where to store the Awesomium data
		string userDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "G+ Notifier");

		// Current app version
		Version currentVersion = Assembly.GetExecutingAssembly().GetName().Version;

		bool isFirstRun;

		public MainForm()
		{
			InitializeComponent();
		}

		public MainForm(bool hasUpdated)
			: this()
		{
			this.isFirstRun = hasUpdated;
		}

		private void MainForm_Load(object sender, EventArgs e)
		{
			// Hide the form at startup, we don't want it to be seen (since we live in the notification area).
			this.Hide();

			// Set the default icon
			notificationIcon.Icon = Icons.GetLogo();

			// Set up notifiers
			notifiers.Add(new WindowsBalloonNotifier(notificationIcon));

			// Try the Snarl notifier - this will test on a background thread (to avoid locking the UI thread) and add
			// itself to the collection if it's found and registered.
			SnarlNotifier.TryRegister(notifiers);

			// Pause slightly in an attempt to allow Snarl detection to finish
			Thread.Sleep(100);

			// Context menu tweaks
			versionToolStripMenuItem.Text = "G+ Notifier " + currentVersion.ToString(2);
			automaticallyInstallUpdatesToolStripMenuItem.Checked = Settings.Default.AutomaticallyInstallUpdates;

			// Initialiser Awesomium settings
			WebCoreConfig config = new WebCoreConfig
			{
				SaveCacheAndCookies = true, // Make sure we save cookies to avoid logging in every time
				UserDataPath = userDataPath,
				LogPath = userDataPath
			};
			WebCore.Initialize(config);

			// Create the client used for communicating with Google+.
			googlePlusClient = new GooglePlusClient();
			googlePlusClient.UpdateNotificationCount += OnUpdateNotificationCount;

			// Check whether we're logged in and immediately show the login form, if required.
			isLoggedIn = googlePlusClient.IsLoggedIn();
			if (!isLoggedIn)
				DisplayLoginForm();

			// If this was the first fun, tell the user we updated
			if (isFirstRun)
				SendNewVersionNotification(null, "G+ Notifier Updated!", "G+ Notifier successfully updated to version " + currentVersion.ToString());

			// Force a check for updates
			CheckForUpdates();
		}

		private void DisplayLoginForm()
		{
			if (!isLoggedIn && loginForm == null && !userHasCancelledPreviousLogin)
			{
				loginForm = new LoginForm();
				loginForm.FormClosed += LoginFormClosed;
				loginForm.PageChanged += new EventHandler(loginForm_PageChanged);
				loginForm.Show();
			}
		}

		private void OnUpdateNotificationCount(object sender, JSCallbackEventArgs e)
		{
			if (e.Arguments.Length == 1)
			{
				int notificationCount = e.Arguments[0].ToInteger();

				if (notificationCount == -1)
				{
					isLoggedIn = false;
					DisplayLoginForm();
				}
				else if (notificationCount == -2)
				{
					// This occurs whenever we receive an invalid response from the
					// server (either empty or malformed response).
					isLoggedIn = false;
					SendErrorNotification(5, "Error", "There was an error retrieving notifications from Google+.");
				}
				else
				{
					isLoggedIn = true;

					if (isLoggedIn && loginForm != null)
					{
						loginForm.Hide();
						loginForm.Close();
						loginForm = null;
					}

					// Update the icon with the number of messages.
					UpdateIcon(notificationCount);

					Console.WriteLine("Notification Count is now: " + notificationCount);

					// Show a balloon notification if there are some messages and it isn't the same as the previous one.
					if ((notificationCount > 0) && (lastNotificationCount != notificationCount || forceNotification))
					{
						SendNewMessagesNotification(5, "New Notifications", "You have " + notificationCount + " unread notification" + (notificationCount == 1 ? "" : "s") + " in Google+!");
					}
					else if (notificationCount == 0 && forceNotification)
					{
						SendNewMessagesNotification(5, "No Notifications", "You have no unread notifications in Google+.");
					}

					// Don't show the message again if zero
					forceNotification = false;

					lastNotificationCount = notificationCount;
				}

				loginToolStripMenuItem.Text = isLoggedIn ? "&Logout" : "&Login";
			}
		}

		void loginForm_PageChanged(object sender, EventArgs e)
		{
			// We force another update whenever the login form's page changes
			googlePlusClient.BindNotificationScripts();
		}

		void LoginFormClosed(object sender, FormClosedEventArgs e)
		{
			userHasCancelledPreviousLogin = true;
			loginForm.FormClosed -= LoginFormClosed;
			loginForm.PageChanged -= loginForm_PageChanged;
			loginForm = null;
		}

		private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
		{
			if (notificationsForm != null && !notificationsForm.IsDisposed)
			{
				notificationsForm.Close();
				notificationsForm = null;
			}

			googlePlusClient.Dispose();
			WebCore.Shutdown();
		}

		private void checkForUpdates_Tick(object sender, EventArgs e)
		{
			CheckForUpdates();
		}

		/// <summary>
		/// Updates the icon in the notification area based on the notification count
		/// </summary>
		/// <param name="notificationCount"></param>
		private void UpdateIcon(int? notificationCount)
		{
			// Remove any previous custom icon
			notificationIcon.Icon = Icons.GetLogo();

			// If we know the count, overlay the number on to the icon.
			if (notificationCount.HasValue)
			{
				// Set the new icon
				notificationIcon.Icon = Icons.GetIcon(notificationCount);
			}
		}

		private void ShowNotificationsForm(bool hideIfAlreadyVisible)
		{
			if (notificationsForm == null || notificationsForm.IsDisposed)
			{
				notificationsForm = new NotificationsForm();
				notificationsForm.Show();
				notificationsForm.Activate();
			}
			else if (!notificationsForm.Visible)
				notificationsForm.Show();
			else if (hideIfAlreadyVisible)
				notificationsForm.Hide();
		}

		private void CheckForUpdates()
		{
			checkForUpdatesWorker.RunWorkerAsync();
		}

		private void checkForUpdatesWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
		{
			var client = new WebClient();
			try
			{
				e.Result = client.DownloadString("http://gplusnotifier.com/Version");
			}
			catch { }
		}

		private void checkForUpdatesWorker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
		{
			if (e.Result != null)
			{
				Version latestVersion = Version.Parse((string)e.Result);

				if (latestVersion > currentVersion)
				{
					string message;
					int? timeoutSeconds = null;

					if (Settings.Default.AutomaticallyInstallUpdates)
					{
						if (!updater.CanWriteToApplicationFolder())
							message = "Unable to write to the folder G+ Notifier is installed in. Automatic updates will be unavailable.";
						else if (!updater.CanUnzipFiles())
							message = "Automatic updates are not supported on this version of Windows :-(";
						else
						{
							updater.DownloadAndInstallUpdateAsync();
							message = string.Format("Downloading and installing version {0} of G+ Notifier...", latestVersion);
							timeoutSeconds = 5;
						}
					}
					else
					{
						message = string.Format("There is a new version of G+ Notifier available. You have version {0}, but version {1} is available.",
							currentVersion,
							latestVersion
							);
					}
					SendNewVersionNotification(timeoutSeconds, "G+ Notifier Update Available", message);

					// Also update text on the context menu.
					gNotifierWebsiteToolStripMenuItem.Text = "Update available! " + gNotifierWebsiteToolStripMenuItem.Text;
				}
			}
		}

		private void SendErrorNotification(int timeoutSeconds, string title, string message)
		{
			foreach (var notifier in notifiers)
				notifier.SendErrorNotification(timeoutSeconds, title, message);
		}

		private void SendNewVersionNotification(int? timeoutSeconds, string title, string message)
		{
			foreach (var notifier in notifiers)
				notifier.SendNewVersionNotification(timeoutSeconds, title, message);
		}

		private void SendNewMessagesNotification(int timeoutSeconds, string title, string message)
		{
			foreach (var notifier in notifiers)
				notifier.SendMessageCountNotification(timeoutSeconds, title, message);
		}

		#region Context Menu & Icon event handlers

		private void notificationIcon_MouseClick(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				ShowNotificationsForm(true);
			}
			else if (e.Button == MouseButtons.Middle)
				ForceCheck();
		}

		private void notificationIcon_BalloonTipClicked(object sender, EventArgs e)
		{
			ShowNotificationsForm(false);
		}

		private void gNotifierWebsiteToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Process.Start("http://gplusnotifier.com/");
		}

		private void feedbackSupportMenuItem_Click(object sender, EventArgs e)
		{
			Process.Start("http://gplusnotifier.uservoice.com/");
		}

		private void donateToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Process.Start("http://gplusnotifier.com/Donate");
		}

		private void launchGoogleToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Process.Start("http://plus.google.com/");
		}

		private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
		{
			new AboutForm().ShowDialog();
		}

		private void twitterToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Process.Start("http://twitter.com/GPlusNotifier");
		}

		private void loginToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (loginForm == null)
			{
				// We will log the user out by clearing cookies
				WebCore.ClearCookies();
				isLoggedIn = false;

				// Clear the icon to the logo.
				notificationIcon.Icon = Icons.GetLogo();

				// Force a check
				ForceCheck();
			}
		}

		private void automaticallyInstallUpdatesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			// If we're trying to turn automatic updates on, check some stuff
			if (!Settings.Default.AutomaticallyInstallUpdates)
			{
				// Can write into the app folder
				if (!updater.CanWriteToApplicationFolder())
				{
					MessageBox.Show("Unable to write to the folder G+ Notifier is installed in. Automatic updates will be unavailable.", "Automatic Updates");
					return;
				}

				// Can use the Shell to unzip (although XP supports it, this class barfs)
				if (!updater.CanUnzipFiles())
				{
					MessageBox.Show("Automatic updates are not supported on this version of Windows :-(", "Automatic Updates");
					return;
				}

				// Force a check when we turn it on.
				CheckForUpdates();
			}

			Settings.Default.AutomaticallyInstallUpdates = !Settings.Default.AutomaticallyInstallUpdates;
			Settings.Default.Save();
			automaticallyInstallUpdatesToolStripMenuItem.Checked = Settings.Default.AutomaticallyInstallUpdates;
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Application.Exit();
		}

		#endregion

		private void ForceCheck()
		{
			googlePlusClient.ForceCheck();

			forceNotification = true;
			userHasCancelledPreviousLogin = false;
		}
	}
}
