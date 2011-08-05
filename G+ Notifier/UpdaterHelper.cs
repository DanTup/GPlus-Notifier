using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using Shell32;

namespace DanTup.GPlusNotifier
{
	class UpdaterHelper
	{
		/// <summary>
		/// Check whether or not we'll be able to install updates automatically (eg. can write to the folder we're in).
		/// </summary>
		public bool CanWriteToApplicationFolder()
		{
			// Any failure can be considered down to permissions.
			try
			{
				// We'll try to write a test file to the install folder.
				var installFolder = Program.GetApplicationPath();
				var testFile = Path.Combine(installFolder, "TestWriteAccess.txt");

				// Test message to write/read.
				string testMessage = "Test Write";

				// Write the file, read it back, then delete it.
				File.WriteAllText(testFile, testMessage);
				var contents = File.ReadAllText(testFile);
				File.Delete(testFile);

				// Success if the file was written/read successfully.
				return contents == testMessage;
			}
			catch
			{
				// TODO: Log this to aid debugging.
				return false;
			}
		}

		public bool CanUnzipFiles()
		{
			try
			{
				var shellTest = new Shell();

				return true;
			}
			catch
			{
				return false;
			}
		}

		public void DownloadAndInstallUpdateAsync()
		{
			ThreadPool.QueueUserWorkItem(o => DownloadAndInstallUpdate());
		}

		private void DownloadAndInstallUpdate()
		{
			try
			{
				var applicationPath = Program.GetApplicationPath();
				var client = new WebClient();

				// Figure out what's the latest version.
				var versionString = client.DownloadString("http://gplusnotifier.com/Version");
				Version latestVersion = Version.Parse(versionString);

				// Download the zip file, if it's not already here.
				var updateFile = Path.Combine(applicationPath, string.Format("DanTup.GPlusNotifier-{0}.zip", latestVersion));
				if (!File.Exists(updateFile))
					client.DownloadFile(string.Format("https://bitbucket.org/DanTup/g-notifier/downloads/DanTup.GPlusNotifier-{0}.zip", latestVersion), updateFile);

				// Extract into an update folder.
				var updateFolder = Path.Combine(applicationPath, "Update");
				if (Directory.Exists(updateFolder))
					Directory.Delete(updateFolder, true);
				Directory.CreateDirectory(updateFolder);
				ExtractZipFile(updateFile, updateFolder);

				// Delete the zip file.
				File.Delete(updateFile);

				// Execute the updater and exit.
				var updater = Path.Combine(updateFolder, "DanTup.GPlusNotifier.Updater.exe");
				if (File.Exists(updater))
				{
					ProcessStartInfo info = new ProcessStartInfo
					{
						CreateNoWindow = true,
						UseShellExecute = false,
						WorkingDirectory = updateFolder,
						FileName = updater
					};
					Process.Start(info);
					Application.Exit();
				}
			}
			catch (Exception ex)
			{
				// TODO: Add logging/alert.
				MessageBox.Show(ex.ToString());
			}
		}

		private void ExtractZipFile(string filename, string destinationDirectory)
		{
			Shell shell = new Shell();
			var zipFile = shell.NameSpace(filename);
			var targetDirectory = shell.NameSpace(destinationDirectory);

			targetDirectory.CopyHere(zipFile.Items(), 4 & 16 & 512); // NOTE: Flags don't seem to work, shows user anyway!
		}
	}
}
