using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Windows.Forms;
using AwesomiumSharp;

namespace DanTup.GPlusNotifier
{
	public partial class MainForm : Form
	{
		/// <summary>
		/// The Awesomium WebView rendering our web pages.
		/// </summary>
		private WebView WebView { get; set; }

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

		public MainForm()
		{
			InitializeComponent();
		}

		private void MainForm_Load(object sender, EventArgs e)
		{
			// Hide the form at startup, we don't want it to be seen (since we live in the notification area).
			this.Hide();

			// Set up notifiers
			notifiers.Add(new WindowsBalloonNotifier(notificationIcon));

			// Try the Snarl notifier - this will test on a background thread (to avoid locking the UI thread) and add
			// itself to the collection if it's found and registered.
			SnarlNotifier.TryRegister(notifiers);

			// Set version umber in the context menu
			versionToolStripMenuItem.Text = "G+ Notifier " + currentVersion.ToString(2);

			// Set up the browser
			WebCoreConfig config = new WebCoreConfig
			{
				SaveCacheAndCookies = true, // Make sure we save cookies to avoid logging in every time
				UserDataPath = userDataPath,
				LogPath = userDataPath
			};
			WebCore.Initialize(config);
			this.WebView = WebCore.CreateWebView(128, 128);

			// Load a page that contains the notification box, but isn't as heavy as the Plus site.
			this.WebView.CreateObject("GNotifier");
			this.WebView.SetObjectCallback("GNotifier", "updateCount", OnUpdateNotificationCount);
			this.WebView.DomReady += new EventHandler(WebView_DomReady);
			this.WebView.LoadCompleted += new EventHandler(WebView_LoadCompleted);
			this.WebView.JSConsoleMessageAdded += new JSConsoleMessageAddedEventHandler(WebView_JSConsoleMessageAdded);
			WebCore.Update();

			/**
			 * We use a fake URL because:
			 * A) We want the page to load quickly
			 * B) We are only loading this URL to spoof cross-domain security restrictions
			 * 
			 * We will be making AJAX calls from within the 404 page served by Google
			 * to make it seem as if the API calls are coming from an actual GPlus page.
			 * Hacky? Yes. Does it work? Definitely. :-)
			 **/
			this.WebView.LoadURL("http://plus.google.com/foobar/fakeurl");

			// We display the login form immediately if we don't detect the HSID cookie
			string googleCookies = WebCore.GetCookies("http://www.google.com", false);
			if (!googleCookies.Contains("HSID="))
			{
				isLoggedIn = false;
				DisplayLoginForm();
			}

			// Set the default icon
			notificationIcon.Icon = Icons.GetLogo();

			// Force a check for updates
			CheckForUpdates();
		}

		void WebView_JSConsoleMessageAdded(object sender, JSConsoleMessageEventArgs e)
		{
			Console.WriteLine(e.Message + ", " + e.Source);
		}

		// Attempt to bind the notification function once the DOM has finished initializing
		void WebView_DomReady(object sender, EventArgs e)
		{
			BindNotificationScripts();
		}

		// For extra measure, we'll attempt to bind the function at the end of each page load
		void WebView_LoadCompleted(object sender, EventArgs e)
		{
			BindNotificationScripts();
		}

		private void BindNotificationScripts()
		{
			// We make AJAX requests directly to Google Plus' internal API from an
			// empty page served on the http://plus.google.com domain.
			// We poll the API every 30 seconds for the notification count.
			// If we receive a bad status code, we assume we are not logged in.
			if (!this.WebView.IsDisposed)
				this.WebView.ExecuteJavascript(@"
					if(typeof window.xhr == 'undefined') { 
						window.notify = function(x) { 
							if(window.count != x) { window.count = x; GNotifier.updateCount(x); } 
						}; 
						window.xhr = new XMLHttpRequest();
						window.tick = function() {
							xhr.open('get','https://plus.google.com/u/0/_/n/guc'); 
							xhr.onreadystatechange = function() { 
								if(xhr.readyState == 4) { 
									if(xhr.status != 200) { 
										notify(-1); 
									} 
									else { 
										var result = JSON.parse(xhr.responseText.substr(4)); 
										if(typeof result != 'object') 
											notify(-2); 
										else 
											notify(result[1]); 
									}
								} 
							}; 
							xhr.send(null);
						};
						window.setInterval('tick()', 30000);
					}; 
					tick();"); // We fire off one tick immediately
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
			BindNotificationScripts();
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

			// Cleanly shut down the browser (presumably this is when it persists cookies).
			this.WebView.Close();
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
					string message = string.Format("There is a new version of G+ Notifier available. You have version {0}, but version {1} is available.",
						currentVersion,
						latestVersion
						);
					SendNewVersionNotification(5, "G+ Notifier Update Available", message);

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

		private void SendNewVersionNotification(int timeoutSeconds, string title, string message)
		{
			foreach (var notifier in notifiers)
				notifier.SendNewVersionNotification(title, message);
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

				// Force a check
				ForceCheck();
			}
		}
		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Application.Exit();
		}

		#endregion

		private void ForceCheck()
		{
			// We set window.count to an arbitrary value to invalidate it and force an update
			this.WebView.ExecuteJavascript("window.count = -5");

			// Force an update by calling 'tick()' immediately
			BindNotificationScripts();
			forceNotification = true;
			userHasCancelledPreviousLogin = false;
		}
	}
}
