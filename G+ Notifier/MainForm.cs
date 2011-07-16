using System;
using System.Windows.Forms;
using System.Drawing;

namespace DanTup.GPlusNotifier
{
	public partial class MainForm : Form
	{
		public MainForm()
		{
			InitializeComponent();
		}

		private void MainForm_Load(object sender, EventArgs e)
		{
			// HACK: Resize to dock over on the right (this is temporary - needs to take into account where task bar is)

			// HACK: For some reason, if set set Size, then read it, it still has the old value, so keep a copy of the
			// size we're resizing to.
			int padding = 50;
			var newSize = new Size(500, Screen.PrimaryScreen.WorkingArea.Height - padding * 2);
			this.Size = newSize;
			this.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width - newSize.Width - padding, padding);


			// We only want to show in the notification area at startup, so hide the form.
			this.Hide();
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			// HACK: The systray icons stays visible if we just quit the app, so remove it first
			//niSystray.Visible = false;
			Application.Exit();
		}

		private void notificationIcon_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			// TODO: Start navigating to the notificatins window
			webBrowser.Navigate("http://plus.google.com/");

			// Show the notification window
			this.Show();
			this.WindowState = FormWindowState.Normal;
			this.BringToFront();
		}

		private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			// HACK: Hijack closing, and just minimise instead. This is because this form is actually what's showing the
			// notification icon and if we close the form, the icon will disappear. Is there a better way of having a
			// NotifyIcon without a form?
			if (e.CloseReason == CloseReason.UserClosing)
			{
				e.Cancel = true;
				this.Hide();
				this.WindowState = FormWindowState.Minimized;
			}
		}
	}
}
