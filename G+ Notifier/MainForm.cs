using System;
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

		bool isLoggedIn = false;

		public MainForm()
		{
			InitializeComponent();
		}

		private void MainForm_Load(object sender, EventArgs e)
		{
			// Hide the form at startup, we don't want it to be seen (since we live in the notification area).
			this.Hide();

			// Set up the browser (make sure we save cookies to avoid logging in every time).
			WebCoreConfig config = new WebCoreConfig { SaveCacheAndCookies = true };
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
		}

		void WebView_JSConsoleMessageAdded(object sender, JSConsoleMessageEventArgs e)
		{
			Console.Out.WriteLine(e.Message + ", " + e.Source);
		}

		// Atempt to bind the notification function once the DOM has finished initializing
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
		private void OnUpdateNotificationCount(object sender, AwesomiumSharp.JSCallbackEventArgs e)
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

		private void EnsureLoggedIn()
		{
			if (this.WebView.IsDomReady)
			{
				// Check whether we're logged in, by checking for the presence of the "gb_119" element.
				isLoggedIn = this.WebView.ExecuteJavascriptWithResult("document.getElementById('gb_119') != null", timeoutMs: 1000).ToBoolean();
				if (!isLoggedIn)
				{
					if (loginForm == null)
					{
						loginForm = new LoginForm(this.WebView);
						loginForm.Show();
					}
				}
				else
				{
					if (loginForm != null)
					{
						loginForm.Hide();
						loginForm.Close();
						loginForm = null;
					}
				}
			}
		}

		private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			// TODO: This isn't used until we are showing previews on the form.
			//// HACK: Hijack closing, and just minimise instead. This is because this form is actually what's showing the
			//// notification icon and if we close the form, the icon will disappear. Is there a better way of having a
			//// NotifyIcon without a form?
			//if (e.CloseReason == CloseReason.UserClosing)
			//{
			//    e.Cancel = true;
			//    this.Hide();
			//    this.WindowState = FormWindowState.Minimized;
			//}
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

			EnsureLoggedIn();

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

		private void LaunchPlus()
		{
			if (notificationsForm == null || notificationsForm.IsDisposed)
			{
				notificationsForm = new NotificationsForm();
				notificationsForm.Show();
			}
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
			// TODO: This isn't used until we are showing previews on the form.
			//// Show the notification window
			//this.Show();
			//this.WindowState = FormWindowState.Normal;
			//this.BringToFront();

			LaunchPlus();
		}

		private void notificationIcon_BalloonTipClicked(object sender, EventArgs e)
		{
			LaunchPlus();
		}

		private void gNotifierWebsiteToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Process.Start("http://gplusnotifier.com/");
		}

		private void gNotifierOnGoogleToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Process.Start("https://plus.google.com/101567187775228995510");
		}

		private void dannyTuppenyOnGoogleToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Process.Start("https://plus.google.com/116849139972638476037");
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
				loginForm = new LoginForm(this.WebView);
				loginForm.Show();
			}
		}

		private void clearCookiesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			WebCore.ClearCookies();
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Application.Exit();
		}

		#endregion

		private void reloadTimer_Tick(object sender, EventArgs e)
		{
			if (isLoggedIn && loginForm == null)
				this.WebView.Reload();
		}

		private void feedbackSupportMenuItem_Click(object sender, EventArgs e)
		{
			Process.Start("http://gplusnotifier.uservoice.com/");
		}
	}
}
