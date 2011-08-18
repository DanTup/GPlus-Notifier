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

			// HACK: Resize to window to by a reasonable height.
			int padding = 50;
			var newSize = new Size(500, (int)((Screen.PrimaryScreen.WorkingArea.Height - (padding * 2)) * 0.6));
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
