using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using DanTup.GPlusNotifier.Properties;

namespace DanTup.GPlusNotifier
{
	static class Program
	{
		// Where to store data
		public static string UserDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "G+ Notifier");

		[STAThread]
		static void Main(string[] args)
		{
			// Log any errors
			AppDomain.CurrentDomain.UnhandledException += LogException;

			bool hasUpdated = args.Contains("-u");

			// If we're a new version, we'll have lost all settings, so pull in the previous settings
			if (hasUpdated || !Settings.Default.HasUpgradedSettings)
			{
				Settings.Default.Upgrade();
				Settings.Default.HasUpgradedSettings = true;
				Settings.Default.Save();
			}

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new MainForm(hasUpdated));
		}

		/// <summary>
		/// Gets the folder the application is install in/running from.
		/// </summary>
		/// <returns></returns>
		public static string GetApplicationPath()
		{
			var codeBase = Assembly.GetExecutingAssembly().CodeBase;
			var uri = new UriBuilder(codeBase);
			var path = Uri.UnescapeDataString(uri.Path);
			var folder = Path.GetDirectoryName(path);
			return folder;
		}

		static void LogException(object sender, UnhandledExceptionEventArgs e)
		{
			var file = Path.Combine(UserDataPath, "ErrorLog.txt");
			var exception = e.ExceptionObject as Exception;
			var errorText = exception != null ? exception.ToString() : "Unknown error";

			File.AppendAllText(file, errorText);
		}
	}
}
