using System.Windows.Forms;

namespace DanTup.GPlusNotifier
{
	/// <summary>
	/// Shows Windows balloon notifications from the system tray.
	/// </summary>
	class WindowsBalloonNotifier : INotifier
	{
		public NotifyIcon NotificationIcon { get; set; }

		public WindowsBalloonNotifier(NotifyIcon notificationIcon)
		{
			this.NotificationIcon = notificationIcon;
		}

		/// <summary>
		/// Shows a balloon notification.
		/// </summary>
		private void SendNotification(int timeoutSeconds, string title, string message)
		{
			this.NotificationIcon.ShowBalloonTip(timeoutSeconds * 1000, title, message, ToolTipIcon.None);
		}

		/// <summary>
		/// Sends notification of an error that has occured.
		/// </summary>
		public void SendErrorNotification(int timeoutSeconds, string title, string message)
		{
			this.NotificationIcon.ShowBalloonTip(timeoutSeconds * 1000, title, message, ToolTipIcon.Error);
		}

		/// <summary>
		/// Sends notification that there is a new version of G+ Notifier.
		/// </summary>
		public void SendNewVersionNotification(string title, string message)
		{
			this.SendNotification(30, title, message);
		}

		/// <summary>
		/// Sends notification of how many unread messages there are in Google+.
		/// </summary>
		public void SendMessageCountNotification(int timeoutSeconds, string title, string message)
		{
			this.SendNotification(timeoutSeconds, title, message);
		}
	}
}