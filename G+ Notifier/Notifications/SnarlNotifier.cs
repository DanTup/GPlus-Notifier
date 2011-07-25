using System;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace DanTup.GPlusNotifier
{
	/// <summary>
	/// Providers notifications using Snarl.
	/// </summary>
	class SnarlNotifier : INotifier
	{
		/// <summary>
		/// Tries to register with Snarl, adding itself to the notifiers collection if successfully registered.
		/// </summary>
		/// <param name="notifiers"></param>
		public static void TryRegister(ConcurrentBag<INotifier> notifiers)
		{
			ThreadPool.QueueUserWorkItem(
				o =>
				{
					try
					{
						// TODO: This should change when we have options for this.

						// Try to create a notifier - this will fail if unable to register.
						var snarl = new SnarlNotifier();

						// Remove the Windows balloon notification since we have Snarl.
						INotifier removed;
						while (!notifiers.IsEmpty)
						{
							notifiers.TryTake(out removed);
						}

						// Add the Snarl notifie.
						notifiers.Add(snarl);
					}
					catch
					{
						// Do nothing - registration failed
						Console.WriteLine("Snarl not found");
					}
				});
		}

		/// <summary>
		/// The application name displayed in Snarl.
		/// </summary>
		const string ApplicationName = "G+ Notifier";

		/// <summary>
		/// The name displayed in Snarl for error notifications.
		/// </summary>
		const string MessageError = "Error";

		/// <summary>
		/// The name displayed in Snarl for notifications of new versions of G+ Notifier.
		/// </summary>
		const string MessageNewVersion = "New Version Available";

		/// <summary>
		/// The name displayed in Snarl for notifications of new messages in Google+.
		/// </summary>
		const string MessageCount = "New Messages";

		/// <summary>
		/// Creates an instance of the Snarl notifier and registers with Snarl.
		/// </summary>
		public SnarlNotifier()
		{
			Register(true);
		}

		/// <summary>
		/// Registers with a local instance of Snarl.
		/// </summary>
		/// <param name="register">true to register; false to unregister</param>
		private void Register(bool register)
		{
			string action = register ? "register" : "unregister";

			// Sent the Register command
			SendMessage(string.Format("type=SNP#?version=1.0#?action={0}#?app={1}\r\n", action, ApplicationName));

			// TODO: Refactor these out into classes and enumerate + come up with more reusable way of calling
			// Register the different message types so the user can apply different settings.
			if (register)
			{
				SendMessage(string.Format("type=SNP#?version=1.0#?action=add_class#?app={0}#?class={1}\r\n", ApplicationName, MessageError));
				SendMessage(string.Format("type=SNP#?version=1.0#?action=add_class#?app={0}#?class={1}\r\n", ApplicationName, MessageNewVersion));
				SendMessage(string.Format("type=SNP#?version=1.0#?action=add_class#?app={0}#?class={1}\r\n", ApplicationName, MessageCount));
			}
		}

		public void SendErrorNotification(int timeoutSeconds, string title, string message)
		{
			SendMessage(string.Format("type=SNP#?version=1.0#?action=notification#?app={0}#?class={1}#?title={2}#?text={3}#?timeout={4}\r\n", ApplicationName, MessageError, title, message, timeoutSeconds));
		}

		public void SendNewVersionNotification(string title, string message)
		{
			SendMessage(string.Format("type=SNP#?version=1.0#?action=notification#?app={0}#?class={1}#?title={2}#?text={3}\r\n", ApplicationName, MessageNewVersion, title, message));
		}

		public void SendMessageCountNotification(int timeoutSeconds, string title, string message)
		{
			SendMessage(string.Format("type=SNP#?version=1.0#?action=notification#?app={0}#?class={1}#?title={2}#?text={3}#?timeout={4}\r\n", ApplicationName, MessageCount, title, message, timeoutSeconds));
		}

		private static void SendMessage(string message)
		{
			IPAddress host = IPAddress.Loopback;
			IPEndPoint endpoint = new IPEndPoint(host, 9887);
			using (Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
			{
				// Set low timeouts
				sock.SendTimeout = 500;
				sock.ReceiveTimeout = 500;

				// Send message
				sock.Connect(endpoint);
				sock.Send(Encoding.ASCII.GetBytes(message));
				sock.Close();
			}
		}
	}
}