namespace DanTup.GPlusNotifier
{
	/// <summary>
	/// Defines an interface for classes that are able to handle notifications for G+ Notifier.
	/// </summary>
	interface INotifier
	{
		/// <summary>
		/// Sends notification of an error that has occured.
		/// </summary>
		void SendErrorNotification(int timeoutSeconds, string title, string message);

		/// <summary>
		/// Sends notification that there is a new version of G+ Notifier.
		/// </summary>
		void SendNewVersionNotification(string title, string message);

		/// <summary>
		/// Sends notification of how many unread messages there are in Google+.
		/// </summary>
		void SendMessageCountNotification(int timeoutSeconds, string title, string message);
	}
}
