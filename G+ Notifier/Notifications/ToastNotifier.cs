using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace DanTup.GPlusNotifier
{
	class ToastNotifier : INotifier
	{
		List<NotificationForm> notifications = new List<NotificationForm>();

		/// <summary>
		/// Shows a notification.
		/// </summary>
		private void SendNotification(int timeoutSeconds, string title, string message)
		{
			// Create a new notification
			var notification = new NotificationForm(timeoutSeconds, title, message);

			// Get the best location to display it on the screen.
			Point loc = GetAvailableLocation(notification);

			// Set the location and show the message.
			notification.SetDesktopLocation(loc.X, loc.Y);
			notification.Show();

			// Add this notification to the list of currently active notifications.
			notifications.Add(notification);

			// Remove it from the list when it closes.
			notification.FormClosed += (sender, e) => notifications.Remove(notification);
		}

		private Point GetAvailableLocation(NotificationForm notification)
		{
			// Get the position to show this notification.
			int padding = 10;
			Point loc = new Point();
			if (notifications.Count == 0)
			{
				// Bottom-right
				loc.X = Screen.PrimaryScreen.WorkingArea.Width - (notification.Width + padding);
				loc.Y = Screen.PrimaryScreen.WorkingArea.Height - (notification.Height + padding);
			}
			else
			{
				// Get the last notification shown, and add it to that.
				var last = notifications.Last();

				// Try putting this one above the previous one.
				loc.X = last.Location.X;
				loc.Y = last.Location.Y - notification.Height - padding;

				// If we go off the top of the screen, then move over to next column.
				if (loc.Y <= padding)
				{
					loc.X = last.Location.X - (notification.Width + padding);
					loc.Y = Screen.PrimaryScreen.WorkingArea.Height - (notification.Height + padding);
				}
			}
			return loc;
		}

		/// <summary>
		/// Sends notification of an error that has occured.
		/// </summary>
		public void SendErrorNotification(int timeoutSeconds, string title, string message)
		{
			this.SendNotification(timeoutSeconds, title, message);
		}

		/// <summary>
		/// Sends notification that there is a new version of G+ Notifier.
		/// </summary>
		public void SendNewVersionNotification(int? timeoutSeconds, string title, string message)
		{
			this.SendNotification(timeoutSeconds ?? 30, title, message);
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