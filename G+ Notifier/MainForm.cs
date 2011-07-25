﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
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

		// Icons used in the systray.
		Icon iconNone, iconSome, iconCustom;

		// Used for drawing the number on the icon.
		Brush brush = new SolidBrush(Color.WhiteSmoke);
		Font font = new Font("Segoe UI", 10F, FontStyle.Bold);
		PointF badgePosition = new PointF(2.0f, -1f);
		Size badgeOffset = new Size(1, 1);
		LoginForm loginForm;
		NotificationsForm notificationsForm;

		// Used to ensure we don't show balloon too frequently, even though we're updating the icon.
		DateTime lastShownBalloon = DateTime.MinValue;

		// Used to reduce the change of showing the same balloon notification over and over.
		int? lastNotificationCount;

		// Whether the user appears to be logged in
		bool isLoggedIn = false;

		// If the user cancels a login, we don't want to keep popping the form up until they ask (context menu)
		bool userHasCancelledPreviousLogin = false;

		// Default notifiers (this will be tied to config in some way)
		List<INotifier> notifiers = new List<INotifier>();

		string userDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "G+ Notifier");

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
			// Snarl support basically works - needs some refactoring/commenting/etc. and testing
			// Also needs an option to turn on/off (maybe we can detect it automatically?)
			//notifiers.Add(new SnarlNotifier());

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
			else
			{
				SendStartupSummary(3, "Logging In", "Attempting to login to Google+, please wait...");
			}

			// Set up the icons we'll need for the notification area.
			var ass = Assembly.GetExecutingAssembly();
			using (Stream stream = ass.GetManifestResourceStream("DanTup.GPlusNotifier.Icons.None.ico"))
			{
				iconNone = new Icon(stream);
			}
			using (Stream stream = ass.GetManifestResourceStream("DanTup.GPlusNotifier.Icons.Some.ico"))
			{
				iconSome = new Icon(stream);
			}

			// Force a check for updates
			CheckForUpdates();
		}

		void WebView_JSConsoleMessageAdded(object sender, JSConsoleMessageEventArgs e)
		{
			Console.Out.WriteLine(e.Message + ", " + e.Source);
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

					Console.Out.WriteLine("Notification Count is now: " + notificationCount);

					// Show a balloon notification if there are some messages.
					// Only show if it's been at least 60 mins or the count has changed.
					if ((notificationCount > 0)
						&& ((DateTime.Now - lastShownBalloon).TotalMinutes > 60 || lastNotificationCount != notificationCount))
					{
						SendNewMessagesNotification(5, "New Notifications", "You have " + notificationCount + " unread notifications in Google+!");
						lastShownBalloon = DateTime.Now;
					}
					else if (notificationCount == 0 && lastShownBalloon == DateTime.MinValue)
					{
						SendNewMessagesNotification(5, "No Notifications", "You have no unread notifications in Google+.");
						lastShownBalloon = DateTime.Now;
					}

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
			notificationIcon.Icon = iconNone;
			if (iconCustom != null)
			{
				iconCustom.Dispose();
			}

			// If we know the count, overlay the number on to the icon.
			if (notificationCount.HasValue)
			{
				// Create a clone of the icon and add text
				var baseIcon = notificationCount == 0 ? iconNone : iconSome;
				using (var bmp = baseIcon.ToBitmap())
				using (var img = Graphics.FromImage(bmp))
				{
					var badge = notificationCount > 9 ? "9+" : notificationCount.ToString();
					var pos = notificationCount > 9 ? badgePosition - new SizeF(4, 0) : badgePosition;
					img.DrawString(badge, font, brush, pos);
					iconCustom = Icon.FromHandle(bmp.GetHicon());
				}

				// Set the new icon
				notificationIcon.Icon = iconCustom;
			}
		}

		private void ShowNotificationsForm()
		{
			if (notificationsForm == null || notificationsForm.IsDisposed)
			{
				notificationsForm = new NotificationsForm();
				notificationsForm.Show();
			}
			else if (!notificationsForm.Visible)
				notificationsForm.Show();
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
				Version currentVersion = Assembly.GetExecutingAssembly().GetName().Version;

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
				notifier.SendNewVersionNotification(timeoutSeconds, title, message);
		}

		private void SendStartupSummary(int timeoutSeconds, string title, string message)
		{
			foreach (var notifier in notifiers)
				notifier.SendStartupSummary(timeoutSeconds, title, message);
		}

		private void SendNewMessagesNotification(int timeoutSeconds, string title, string message)
		{
			foreach (var notifier in notifiers)
				notifier.SendNewMessagesNotification(timeoutSeconds, title, message);
		}

		#region Context Menu & Icon event handlers

		private void notificationIcon_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			ShowNotificationsForm();
		}

		private void notificationIcon_BalloonTipClicked(object sender, EventArgs e)
		{
			ShowNotificationsForm();
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

		private void loginToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (loginForm == null)
			{
				// We will log the user out by clearing cookies
				WebCore.ClearCookies();
				isLoggedIn = false;
				// We set window.count to an arbitrary value to invalidate it and force an update
				this.WebView.ExecuteJavascript("window.count = -5");
				// Force an update by calling 'tick()' immediately
				BindNotificationScripts();
				lastShownBalloon = DateTime.MinValue;
				userHasCancelledPreviousLogin = false;
			}
		}
		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Application.Exit();
		}

		#endregion
	}
}
