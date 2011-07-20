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

		// Used to ensure we don't show balloon too frequently, even though we're updating the icon.
		DateTime lastShownBalloon = DateTime.MinValue;

		// Used to reduce the change of showing the same balloon notification over and over.
		int? lastNotificationCount;

		// Used to suppress the login if the user clicks cancel, rather than spamming!
		bool suppressLogin = false;

		public MainForm()
		{
			InitializeComponent();
		}

		private void MainForm_Load(object sender, EventArgs e)
		{
			// TODO: This isn't used until we are showing previews on the form.
			//// HACK: Resize to dock over on the right (this is temporary - needs to take into account where task bar is)
			//// HACK: For some reason, if set set Size, then read it, it still has the old value, so keep a copy of the
			//// size we're resizing to.
			//int padding = 50;
			//var newSize = new Size(500, Screen.PrimaryScreen.WorkingArea.Height - padding * 2);
			//this.Size = newSize;
			//this.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width - newSize.Width - padding, padding);

			// Hide the form at startup, we don't want it to be seen (since we live in the notification area).
			this.Hide();

			// Set up the browser (make sure we save cookies to avoid logging in every time).
			WebCoreConfig config = new WebCoreConfig { SaveCacheAndCookies = true };
			WebCore.Initialize(config);
			this.WebView = WebCore.CreateWebView(1024, 768);

			// Load a page that contains the notification box, but isn't as heavy as the Plus site.
			this.WebView.LoadURL("http://www.google.com/webhp?tab=Xw&authuser=0");

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

			// Kick off a check for updates (since the timer fires *after* 24hr).
			CheckForUpdates();
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
			// Disable the timer so we don't run more than once (not sure how we do that since we're supposed to be on
			// the UI thread?!)
			checkNotificationsTimer.Stop();

			// Check whether we're logged in, by checking for the presence of the "gb_119" element.
			var isLoggedIn = this.WebView.ExecuteJavascriptWithResult("document.getElementById('gb_119') != null", timeoutMs: 1000).ToBoolean();
			if (!isLoggedIn && !suppressLogin)
			{
				var loginForm = new LoginForm(this.WebView);
				var result = loginForm.ShowDialog();

				// If the user clicked OK, assume logged in.
				if (result == DialogResult.OK)
					isLoggedIn = true;
				else
					suppressLogin = true;
			}

			// Get the count (null = couldn't read count).
			int? notificationCount = isLoggedIn ? ReadNotificationCountFromBrowser() : null;

			// Update the icon with the number of messages.
			UpdateIcon(notificationCount);

			// Show a balloon notification if there are some messages.
			// Only show if it's been at least 60 mins or the count has changed.
			if (notificationCount != null && notificationCount.Value > 0
				&& ((DateTime.Now - lastShownBalloon).TotalMinutes > 60 || lastNotificationCount != notificationCount))
			{
				notificationIcon.ShowBalloonTip(5000, "New Notifications", "You have " + notificationCount.Value.ToString() + " unread notifications in Google+!", ToolTipIcon.None);
				lastShownBalloon = DateTime.Now;
				lastNotificationCount = notificationCount;
			}

			// Enable the timer again.
			checkNotificationsTimer.Start();
		}

		private void checkForUpdates_Tick(object sender, EventArgs e)
		{
			CheckForUpdates();
		}

		private int? ReadNotificationCountFromBrowser()
		{
			// Try to read the value, but don't ever crash!
			try
			{
				var res = this.WebView.ExecuteJavascriptWithResult("parseInt(document.getElementById('gbi1').innerText)", timeoutMs: 1000);

				// Successfully got an integer?
				if (res.Type == JSValueType.Integer)
					return res.ToInteger();
			}
			catch { } // Ignore errors, probably user wasn't logged in.

			return null;
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
			Process.Start("https://plus.google.com/");
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

		private void feedbackSupportToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Process.Start("http://gplusnotifier.uservoice.com/");
		}

		private void donateToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Process.Start("http://gplusnotifier.com/Donate");
		}

		private void loginToolStripMenuItem_Click(object sender, EventArgs e)
		{
			checkNotificationsTimer.Stop();

			var loginForm = new LoginForm(this.WebView);
			loginForm.ShowDialog();
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
	}
}
