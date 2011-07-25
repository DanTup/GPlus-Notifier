
using System.Net;
using System.Net.Sockets;
using System.Text;
namespace DanTup.GPlusNotifier
{
	class SnarlNotifier : INotifier
	{
		const string ApplicationName = "G+ Notifier";
		const string MessageError = "Error";
		const string MessageNewVersion = "New Version Available";
		const string MessageStartupSummary = "Startup Message Summary";
		const string MessageNewMessages = "New Messages";

		public SnarlNotifier()
		{
			Register(true);
		}

		public void Register(bool register)
		{
			string action = register ? "register" : "unregister";

			// Register
			SendMessage(string.Format("type=SNP#?version=1.0#?action={0}#?app={1}\r\n", action, ApplicationName));

			// TODO: Refactor these out into classes and enumerate + come up with more reusable way of calling
			SendMessage(string.Format("type=SNP#?version=1.0#?action=add_class#?app={0}#?class={1}\r\n", ApplicationName, MessageError));
			SendMessage(string.Format("type=SNP#?version=1.0#?action=add_class#?app={0}#?class={1}\r\n", ApplicationName, MessageNewVersion));
			SendMessage(string.Format("type=SNP#?version=1.0#?action=add_class#?app={0}#?class={1}\r\n", ApplicationName, MessageStartupSummary));
			SendMessage(string.Format("type=SNP#?version=1.0#?action=add_class#?app={0}#?class={1}\r\n", ApplicationName, MessageNewMessages));
		}

		public void SendErrorNotification(int timeoutSeconds, string title, string message)
		{
			SendMessage(string.Format("type=SNP#?version=1.0#?action=notification#?app={0}#?class={1}#?title={2}#?text={3}#?timeout={4}\r\n", ApplicationName, MessageError, title, message, timeoutSeconds));
		}

		public void SendNewVersionNotification(int timeoutSeconds, string title, string message)
		{
			SendMessage(string.Format("type=SNP#?version=1.0#?action=notification#?app={0}#?class={1}#?title={2}#?text={3}#?timeout={4}\r\n", ApplicationName, MessageNewVersion, title, message, timeoutSeconds));
		}

		public void SendStartupSummary(int timeoutSeconds, string title, string message)
		{
			SendMessage(string.Format("type=SNP#?version=1.0#?action=notification#?app={0}#?class={1}#?title={2}#?text={3}#?timeout={4}\r\n", ApplicationName, MessageStartupSummary, title, message, timeoutSeconds));
		}

		public void SendNewMessagesNotification(int timeoutSeconds, string title, string message)
		{
			SendMessage(string.Format("type=SNP#?version=1.0#?action=notification#?app={0}#?class={1}#?title={2}#?text={3}#?timeout={4}\r\n", ApplicationName, MessageNewMessages, title, message, timeoutSeconds));
		}

		private static void SendMessage(string message)
		{
			IPAddress host = IPAddress.Loopback;
			IPEndPoint hostep = new IPEndPoint(host, 9887);
			using (Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
			{
				sock.Connect(hostep);
				sock.Send(Encoding.ASCII.GetBytes(message));
				sock.Close();
			}
		}
	}
}