using System;
using System.Windows.Forms;

namespace DanTup.GPlusNotifier
{
	public partial class NotificationForm : Form
	{
		public NotificationForm(int timeoutSeconds, string title, string message)
		{
			InitializeComponent();

			timeoutTimer.Interval = timeoutSeconds * 1000;
			lblTitle.Text = title;
			lblMessage.Text = message;
		}

		private void NotificationForm_Shown(object sender, EventArgs e)
		{
			fadeInTimer.Start();
			timeoutTimer.Start();
		}

		private void fadeTimer_Tick(object sender, EventArgs e)
		{
			if (this.Opacity < 1)
				this.Opacity += 0.05;
			else
			{
				this.Opacity = 1;
				fadeInTimer.Stop();
			}
		}

		private void timeoutTimer_Tick(object sender, EventArgs e)
		{
			fadeInTimer.Stop();
			timeoutTimer.Stop();
			fadeOutTimer.Start();
		}

		private void fadeOutTimer_Tick(object sender, EventArgs e)
		{
			if (this.Opacity > 0)
				this.Opacity -= 0.05;
			else
			{
				this.Opacity = 0;
				fadeOutTimer.Stop();
				this.Close();
			}
		}

		private void NotificationForm_Click(object sender, EventArgs e)
		{

		}
	}
}
