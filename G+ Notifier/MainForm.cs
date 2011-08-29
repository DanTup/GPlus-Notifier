using System;
using System.Collections.Concurrent;
using System.Diagnostics;
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
		UpdaterHelper updater = new UpdaterHelper();

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

		// Current app version
		Version currentVersion = Assembly.GetExecutingAssembly().GetName().Version;

		bool isFirstRun;

		public MainForm()
		{
			InitializeComponent();

			SetupTranslations();
		}

		public MainForm(bool hasUpdated)
			: this()
		{
			this.isFirstRun = hasUpdated;
		}

		/// <summary>
		/// Sets up any translated text used on the form.
		/// </summary>
		private void SetupTranslations()
		{
			// Doesn't seem to be a nice way of hooking this up in the designer that allows us to have one nice resx file
			// to make it asy to translate :-(

			notificationIcon.Text = Translations.ApplicationTitle;
			gNotifierWebsiteToolStripMenuItem.Text = Translations.WebsiteLink;
			feedbackSupportToolStripMenuItem.Text = Translations.FeedbackLink;
			twitterToolStripMenuItem.Text = Translations.TwitterLink;
			donateToolStripMenuItem.Text = Translations.DonateLink;
			installUpdateToolStripMenuItem.Text = Translations.InstallUpdateLink;
			settingsToolStripMenuItem.Text = Translations.SettingsLink;
			loginToolStripMenuItem.Text = Translations.LoginLink;
			aboutToolStripMenuItem.Text = Translations.AboutLink;
			exitToolStripMenuItem.Text = Translations.ExitLink;
		}

		private void MainForm_Load(object sender, EventArgs e)
		{
			// Hide the form at startup, we don't want it to be seen (since we live in the notification area).
			this.Hide();

			// If we've never shown the user the settings dialog, show it, so they can choose to turn on auto-updates, etc.
			if (!Settings.Default.HasSeenSettings)
			{
				Settings.Default.HasSeenSettings = true;
				Settings.Default.Save();

				ShowSettingsForm();
			}
			else
			{
				// Set the dynamic context menu text (note: this is already done above, in ShowSettingsForm).
				SetContextMenuText();
			}

			// Set the default icon
			notificationIcon.Icon = Icons.GetLogo();

			// Set up notifiers
			notifiers.Add(new ToastNotifier());

			// Try the Snarl notifier - this will test on a background thread (to avoid locking the UI thread) and add
			// itself to the collection if it's found and registered.
			SnarlNotifier.TryRegister(notifiers);

			// Pause slightly in an attempt to allow Snarl detection to finish
			Thread.Sleep(100);

			// Initialiser Awesomium settings
			WebCoreConfig config = new WebCoreConfig
			{
				SaveCacheAndCookies = true, // Make sure we save cookies to avoid logging in every time
				UserDataPath = Program.UserDataPath,
				LogPath = Program.UserDataPath
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
				SendNewVersionNotification(null, Translations.ApplicationUpdatedTitle, string.Format(Translations.ApplicationUpdatedTitle, currentVersion.ToString()));

			// Force a check for updates
			CheckForUpdates();
		}

		private void SetContextMenuText()
		{
			// Context menu tweaks
			versionToolStripMenuItem.Text = string.Format(Translations.VersionText, currentVersion.ToString(2));
		}

		private void ShowSettingsForm()
		{
			using (var settingsForm = new SettingsForm())
			{
				settingsForm.ShowDialog();
			}

			SetContextMenuText();
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
					SendErrorNotification(5, Translations.ApplicationErrorTitle, Translations.ApplicationErrorMessage);
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
						if (notificationCount == 1)
							SendNewMessagesNotification(5, Translations.NewNotificationsTitle, Translations.NewNotificationMessage);
						else if (notificationCount >= 1 && notificationCount <= 4)
							SendNewMessagesNotification(5, Translations.NewNotificationsTitle, string.Format(Translations.NewNotificationsMessage1234, notificationCount));
						else
							SendNewMessagesNotification(5, Translations.NewNotificationsTitle, string.Format(Translations.NewNotificationsMessage5plus, notificationCount));
					}
					else if (notificationCount == 0 && forceNotification)
					{
						SendNewMessagesNotification(5, Translations.NoNewNotificationsTitle, Translations.NoNewNotificationsMessage);
					}

					// Don't show the message again if zero
					forceNotification = false;

					lastNotificationCount = notificationCount;
				}

				loginToolStripMenuItem.Text = isLoggedIn ? Translations.LogoutLink : Translations.LoginLink;
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
			if (!Settings.Default.UseNotificationWindow)
			{
				Process.Start("http://plus.google.com/");
				return;
			}

			if (notificationsForm == null || notificationsForm.IsDisposed)
			{
				notificationsForm = new NotificationsForm();
				notificationsForm.Show();
				notificationsForm.Activate();
			}
			else if (!notificationsForm.Visible)
			{
				notificationsForm.Show();
				notificationsForm.Activate();
			}
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
							message = Translations.ErrorUnableToWrite;
						else if (!updater.CanUnzipFiles())
							message = Translations.ErrorUnsupportedOS;
						else
						{
							updater.DownloadAndInstallUpdateAsync();
							message = string.Format(Translations.DownloadingNewVersionMessage, latestVersion);
							timeoutSeconds = 5;
						}
					}
					else
					{
						message = string.Format(Translations.NewVersionAvailableMessage,
							currentVersion,
							latestVersion
							);
					}
					SendNewVersionNotification(timeoutSeconds, Translations.NewVersionAvailableTitle, message);

					// Also show the context menu option
					if (updater.CanWriteToApplicationFolder() && updater.CanUnzipFiles())
						installUpdateToolStripMenuItem.Visible = true;

					gNotifierWebsiteToolStripMenuItem.Text = Translations.WebsiteUpdateLink;
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

		private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			ShowSettingsForm();
		}

		private void installUpdateToolStripMenuItem_Click(object sender, EventArgs e)
		{
			updater.DownloadAndInstallUpdateAsync();
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
