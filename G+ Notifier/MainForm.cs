﻿using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Reflection;
using System.Threading;
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

		string userDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "G+ Notifier");

		public MainForm()
		{
			InitializeComponent();
		}

		private void MainForm_Load(object sender, EventArgs e)
		{
			// Hide the form at startup, we don't want it to be seen (since we live in the notification area).
			this.Hide();

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
			this.WebView.LoadURL("http://www.google.com/webhp?tab=ww");

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
			BindGplusNotificationCountUpdate();
		}

		// For extra measure, we'll attempt to bind the function at the end of each page load
		void WebView_LoadCompleted(object sender, EventArgs e)
		{
			BindGplusNotificationCountUpdate();
		}

		private void BindGplusNotificationCountUpdate()
		{
			// We hijack one of the Google Bar's functions and redirect it to our own custom callback
			// so that we know whenever the notification count changes
			if (!this.WebView.IsDisposed)
				this.WebView.ExecuteJavascript("window.gbar.logNotificationsCountUpdate = function(a) { GNotifier.updateCount(a) }");
		}

		// Handle Google Bar's "logNotificationsCountUpdate" function in our own custom callback
		private void OnUpdateNotificationCount(object sender, JSCallbackEventArgs e)
		{
			if (e.Arguments.Length == 1)
			{
				int notificationCount = e.Arguments[0].ToInteger();

				// Update the icon with the number of messages.
				UpdateIcon(notificationCount);

				Console.Out.WriteLine("Notification Count is now: " + notificationCount);

				// Show a balloon notification if there are some messages.
				// Only show if it's been at least 60 mins or the count has changed.
				if ((notificationCount > 0)
					&& ((DateTime.Now - lastShownBalloon).TotalMinutes > 60 || lastNotificationCount != notificationCount))
				{
					notificationIcon.ShowBalloonTip(5000, "New Notifications", "You have " + notificationCount + " unread notifications in Google+!", ToolTipIcon.None);
					lastShownBalloon = DateTime.Now;
					lastNotificationCount = notificationCount;
				}
				else if (notificationCount == 0 && lastShownBalloon == DateTime.MinValue)
				{
					notificationIcon.ShowBalloonTip(5000, "No Notifications", "You have no unread notifications in Google+.", ToolTipIcon.None);
					lastShownBalloon = DateTime.Now;
					lastNotificationCount = notificationCount;
				}
			}
		}

		private void CheckLogin()
		{
			if (!this.WebView.IsDomReady)
				return;

			// Check whether we're logged in, by checking for the presence of the "gb_119" element.
			isLoggedIn = this.WebView.ExecuteJavascriptWithResult("document.getElementById('gb_119') != null", timeoutMs: 5000).ToBoolean();

			// If it failed, check again before declaring we're not logged in.
			// TODO: This is an attempt to fix the period showing of the login form, which I suspect may be due to the
			// call timing out, maybe due to the page reloading.
			if (!isLoggedIn)
			{
				WebCore.Update();
				Thread.Sleep(500);
				WebCore.Update();
				isLoggedIn = this.WebView.ExecuteJavascriptWithResult("document.getElementById('gb_119') != null", timeoutMs: 5000).ToBoolean();
			}

			// If we're logged in, always cancel the login flag
			if (isLoggedIn)
				userHasCancelledPreviousLogin = false;

			// If we're not logged in, not showing the login form and user has not cancelled a previous login.
			if (!isLoggedIn && loginForm == null && !userHasCancelledPreviousLogin)
			{
				loginForm = new LoginForm(this.WebView);
				loginForm.FormClosed += LoginFormClosed;
				loginForm.Show();
			}

			// If we're logged in and the login form is visible, hide/close it.
			if (isLoggedIn && loginForm != null)
			{
				loginForm.Hide();
				loginForm.Close();
				loginForm = null;
			}
		}

		void LoginFormClosed(object sender, FormClosedEventArgs e)
		{
			userHasCancelledPreviousLogin = true;
			loginForm.FormClosed -= LoginFormClosed;
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

		/// <summary>
		/// Fired when we need to check for new notifications. NOTE: Although this triggers frequently, it does not
		/// actually hit Google's servers to check - that is done automatically by Google's javascript in the browser
		/// at the frequency they have determined is ok. This timer simply reads from the browser and shows it in the
		/// icon.
		/// </summary>
		private void checkNotificationsTimer_Tick(object sender, EventArgs e)
		{
			checkLoginTimer.Stop();

			CheckLogin();
			loginToolStripMenuItem.Text = isLoggedIn ? "&Logout" : "&Login";

			checkLoginTimer.Start();
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
					notificationIcon.ShowBalloonTip(50000, "G+ Notifier Update Available", message, ToolTipIcon.None);

					// Also update text on the context menu.
					gNotifierWebsiteToolStripMenuItem.Text = "Update available! " + gNotifierWebsiteToolStripMenuItem.Text;
				}
			}
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
				userHasCancelledPreviousLogin = false;

				CheckLogin();
			}
		}
		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Application.Exit();
		}

		#endregion

		private void reloadTimer_Tick(object sender, EventArgs e)
		{
			checkLoginTimer.Stop();

			if (isLoggedIn && loginForm == null)
				this.WebView.LoadURL("http://www.google.com/webhp?tab=ww");

			// Restart the login timer, in case it was the reloading of this page that was causing the intermittent
			// showing off the login form.
			checkLoginTimer.Start();
		}
	}
}
