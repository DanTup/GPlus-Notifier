
namespace DanTup.GPlusNotifier
{
	interface INotifier
	{
		void SendErrorNotification(int timeoutSeconds, string title, string message);
		void SendNewVersionNotification(int timeoutSeconds, string title, string message);
		void SendStartupSummary(int timeoutSeconds, string title, string message);
		void SendNewMessagesNotification(int timeoutSeconds, string title, string message);
	}
}
