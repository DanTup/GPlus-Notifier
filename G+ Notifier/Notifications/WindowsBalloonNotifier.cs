using System.Windows.Forms;

namespace DanTup.GPlusNotifier
{
	class WindowsBalloonNotifier : INotifier
	{
		public NotifyIcon NotificationIcon { get; set; }

		public WindowsBalloonNotifier(NotifyIcon notificationIcon)
		{
			this.NotificationIcon = notificationIcon;
		}

		private void SendNotification(int timeoutSeconds, string title, string message)
		{
			this.NotificationIcon.ShowBalloonTip(timeoutSeconds * 1000, title, message, ToolTipIcon.None);
		}

		public void SendErrorNotification(int timeoutSeconds, string title, string message)
		{
			this.NotificationIcon.ShowBalloonTip(timeoutSeconds * 1000, title, message, ToolTipIcon.Error);
		}

		public void SendNewVersionNotification(int timeoutSeconds, string title, string message)
		{
			this.SendNotification(timeoutSeconds, title, message);
		}

		public void SendStartupSummary(int timeoutSeconds, string title, string message)
		{
			this.SendNotification(timeoutSeconds, title, message);
		}

		public void SendNewMessagesNotification(int timeoutSeconds, string title, string message)
		{
			this.SendNotification(timeoutSeconds, title, message);
		}
	}
}