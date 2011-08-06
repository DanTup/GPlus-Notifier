using System;
using System.Collections.Concurrent;
using System.IO;
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
					// Try up to 5 times, with a short delay, because as Startup, it's possible we loaded before Snarl.
					for (int attempt = 0; attempt < 5; attempt++)
					{
						try
						{
							// Try to create a notifier - this will fail if unable to register.
							var snarl = new SnarlNotifier();

							// Remove the Windows balloon notification since we have Snarl.
							INotifier removed;
							while (!notifiers.IsEmpty)
							{
								notifiers.TryTake(out removed);
							}

							// Add the Snarl notifier.
							notifiers.Add(snarl);

							// Break out of the loop, we're done.
							break;
						}
						catch
						{
							// Do nothing - registration failed
							Console.WriteLine("Snarl not found");
						}

						Thread.Sleep(1000); // Wait 1 second before trying again.
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
		/// The icon path used in Snarl notifications.
		/// </summary>
		string IconPath = GetIconPath();

		/// <summary>
		/// Gets the icon path for Snarl notifications.
		/// </summary>
		private static string GetIconPath()
		{
			var installFolder = Program.GetApplicationPath();
			return Path.Combine(installFolder, @"Icons\Logo.png");
		}

		/// <summary>
		/// Creates an instance of the Snarl notifier and registers with Snarl.
		/// </summary>
		public SnarlNotifier()
		{
			Register(true);
		}

		/// <summary>
		/// Escape function for Snarl SNP 2.
		/// </summary>
		private string SnarlEscape(string action)
		{
			if (action == null)
				return null;

			return action.Replace("=", "==").Replace("&", "&&");
		}

		/// <summary>
		/// Registers with a local instance of Snarl.
		/// </summary>
		/// <param name="register">true to register; false to unregister</param>
		private void Register(bool register)
		{
			string action = register ? "reg" : "unreg";

			// Sent the Register command
			SendMessage(string.Format("snp://{0}?app-sig=DanTup.GPlusNotifier&title={1}&icon={2}\r", SnarlEscape(action), SnarlEscape(ApplicationName), SnarlEscape(IconPath)));

			// TODO: Refactor these out into classes and enumerate + come up with more reusable way of calling
			// Register the different message types so the user can apply different settings.
			if (register)
			{
				SendMessage(string.Format("snp://addclass?app-sig=DanTup.GPlusNotifier&id=MessageError&name={0}\r", SnarlEscape(MessageError)));
				SendMessage(string.Format("snp://addclass?app-sig=DanTup.GPlusNotifier&id=MessageNewVersion&name={0}\r", SnarlEscape(MessageNewVersion)));
				SendMessage(string.Format("snp://addclass?app-sig=DanTup.GPlusNotifier&id=MessageCount&name={0}\r", SnarlEscape(MessageCount)));
			}
		}

		public void SendErrorNotification(int timeoutSeconds, string title, string message)
		{
			SendNotification("MessageError", timeoutSeconds, title, message);
		}

		public void SendNewVersionNotification(int? timeoutSeconds, string title, string message)
		{
			SendNotification("MessageNewVersion", timeoutSeconds, title, message);
		}

		public void SendMessageCountNotification(int timeoutSeconds, string title, string message)
		{
			SendNotification("MessageCount", timeoutSeconds, title, message);
		}

		public void SendNotification(string messageId, int? timeoutSeconds, string title, string message)
		{
			SendMessage(string.Format("snp://notify?app-sig=DanTup.GPlusNotifier&id={0}&uid={0}&title={1}&text={2}&timeout={3}\r", SnarlEscape(messageId), SnarlEscape(title), SnarlEscape(message), timeoutSeconds));
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