using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using AwesomiumSharp;

namespace DanTup.GPlusNotifier
{
	public partial class NotificationsForm : AwesomiumForm
	{
		private Timer animateTimer;
		private int startPosX;
		private int startPosY;

		public NotificationsForm()
		{
			InitializeComponent();

			var newSize =
				// If either direction is < 100px
				Properties.Settings.Default.NotificationsWindowSize.Width < 100
				|| Properties.Settings.Default.NotificationsWindowSize.Height < 100
				// Then use some nice defaults
				? new Size(500, (int)(Screen.PrimaryScreen.WorkingArea.Height * 0.6))
				// Otherwise use the required size
				: Properties.Settings.Default.NotificationsWindowSize;
			this.Size = newSize;

			SetupBrowser();

			// Set up binding for new windows (webView.OpenExternalLink doesn't seem to work)
			webView.CreateObject("GNotifier");
			webView.SetObjectCallback("GNotifier", "launchUrl", OnOpenExternalLink);
			webView.DomReady += new EventHandler(WebView_DomReady);
			webView.LoadURL("https://m.google.com/app/plus/#~loop:view=notifications");
			webView.Focus();

			// Pop doesn't need to be shown in task bar
			ShowInTaskbar = false;
			TopMost = true;

			// Set up hiding on blur.
			this.Deactivate += NotificationsForm_Deactivate;
			this.Focus();
			this.Select();

			// Create and run timer for animation
			animateTimer = new Timer();
			animateTimer.Interval = 15;
			animateTimer.Tick += animateTimer_Tick;
		}

		void NotificationsForm_Deactivate(object sender, EventArgs e)
		{
			Properties.Settings.Default.NotificationsWindowSize = this.Size;
			Properties.Settings.Default.Save();
			this.Hide();
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			// Move window out of screen
			var taskbarLocation = TaskbarHelper.GetPosition();
			if (taskbarLocation == TaskbarPosition.Left)
				startPosX = 0;
			else
				startPosX = Screen.PrimaryScreen.WorkingArea.Width - Width;
			startPosY = Screen.PrimaryScreen.WorkingArea.Height;
			SetDesktopLocation(startPosX, startPosY);

			// Begin animation
			animateTimer.Start();
		}

		void animateTimer_Tick(object sender, EventArgs e)
		{
			// Lift window by 5 pixels
			startPosY -= 20;
			// If window is fully visible stop the timer
			if (startPosY < Screen.PrimaryScreen.WorkingArea.Height - this.Height)
			{
				// Set the fial position
				SetDesktopLocation(startPosX, Screen.PrimaryScreen.WorkingArea.Height - this.Height);
				animateTimer.Stop();
				this.BringToFront();
				this.Activate();
			}
			else
				SetDesktopLocation(startPosX, startPosY);
		}

		// Attempt to bind the notification function once the DOM has finished initializing
		void WebView_DomReady(object sender, EventArgs e)
		{
			BindGplusOpenWindow();
		}

		private void BindGplusOpenWindow()
		{
			// We hijack the _window function that launches new windows
			if (!webView.IsDisposed)
				webView.ExecuteJavascript("_window = function(a) { GNotifier.launchUrl(a) }");
		}

		void OnOpenExternalLink(object sender, JSCallbackEventArgs e)
		{
			if (e.Arguments.Length == 1)
				Process.Start(e.Arguments[0].ToString());

			this.Focus();
		}

		private void NotificationsForm_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Escape)
				this.Close();
		}
	}
}
