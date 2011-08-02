using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;

namespace DanTup.GPlusNotifier.Updater
{
	class Program
	{
		static DirectoryInfo applicationFolder, updateFolder;

		static void Main(string[] args)
		{
			// Wait 1 second before doing anything, to ensure G+ Notifier has closed before trying to update it.
			Thread.Sleep(1000);

			updateFolder = GetApplicationPath();
			applicationFolder = updateFolder.Parent;

			// Log any errors
			AppDomain.CurrentDomain.UnhandledException += LogException;

			throw new Exception();

			// Do some basic sanity checks.
			CheckUpdatesValid(updateFolder, applicationFolder);

			// Delete everything from the parent folder except the Update folder or crash log.
			foreach (var item in applicationFolder.GetFileSystemInfos())
			{
				// If it's a file, or it's (a folder and...) not called Update
				if (item is FileInfo && !string.Equals(item.Name, "ErrorLog.txt", StringComparison.OrdinalIgnoreCase))
					item.Delete();
				else if (item is DirectoryInfo && !string.Equals(item.Name, "Update", StringComparison.OrdinalIgnoreCase))
					((DirectoryInfo)item).Delete(true);
			}

			// Move everything except DanTup.GPlusNotifier.Update.exe into the parent folder.
			foreach (var item in updateFolder.GetFileSystemInfos())
			{
				// If it's a folder, or it's (a file and...) not called DanTup.GPlusNotifier.Update.exe
				if (item is DirectoryInfo)
				{
					// Move the folder (and it's contents) up into the application folder
					Directory.Move(item.FullName, Path.Combine(applicationFolder.FullName, item.Name));
				}
				else if (item is FileInfo)
				{
					// Move the folder (and it's contents) up into the application folder
					File.Move(item.FullName, Path.Combine(applicationFolder.FullName, item.Name));
				}
			}

			// Execute the newly-placed DanTup.GPlusNotifier.exe.
			Process.Start(Path.Combine(applicationFolder.FullName, "DanTup.GPlusNotifier.exe"), "-u");
		}

		static void LogException(object sender, UnhandledExceptionEventArgs e)
		{
			var file = Path.Combine(applicationFolder.FullName, "ErrorLog.txt");
			var exception = e.ExceptionObject as Exception;
			var errorText = exception != null ? exception.ToString() : "Unknown error";

			File.AppendAllText(file, errorText);
		}

		/// <summary>
		/// Do some basic sanity checks before installing the update.
		/// </summary>
		private static void CheckUpdatesValid(DirectoryInfo updateFolder, DirectoryInfo applicationFolder)
		{
			// Ensure we're running from an Update folder that contains a version of the software
			if (updateFolder.Name != "Update")
				Error("The updater was not run from the correct folder.");

			// Ensure the parent folder contains a copy of the software
			if (applicationFolder.GetFiles("DanTup.GPlusNotifier.exe").Length == 0)
				Error("G+ Notifier was not found in the parent folder.");

			// Ensure the update folder contains a copy of the software
			if (updateFolder.GetFiles("DanTup.GPlusNotifier.exe").Length == 0)
				Error("G+ Notifier was not found in the update folder.");

			// Ensure we can write to the parent folder
			try
			{
				bool canWrite = CanWriteToFolder(applicationFolder);

				if (!canWrite)
					Error("Unable to read correctly from the folder that G+ Notifier was installed to.");
			}
			catch (Exception ex)
			{
				Error("Unable to write to the folder that G+ Notifier was installed to. Debug info included below:\r\n\r\n" + ex.ToString());
			}

			// Check for any running instances of the version we're going to overwrite, or the version we will move.
			if (CheckExistingProcesses(applicationFolder) || CheckExistingProcesses(updateFolder))
				Error("Instances of G+ Notifier are still running. Please close any duplicates and execute DanTup.GPlusNotifier.Updater.exe from the Updates folder.");
		}

		/// <summary>
		/// Checks for any instances running from the given folder.
		/// </summary>
		private static bool CheckExistingProcesses(DirectoryInfo applicationFolder)
		{
			foreach (var proc in Process.GetProcessesByName("DanTup.GPlusNotifier"))
			{
				var file = new FileInfo(proc.MainModule.FileName);
				if (string.Equals(file.Directory.FullName, applicationFolder.FullName, StringComparison.OrdinalIgnoreCase))
					return true;
			}

			return false;
		}

		private static void Error(string message)
		{
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine("An error occured while trying to update G+ Notifier.\r\n");
			Console.ForegroundColor = ConsoleColor.White;
			Console.WriteLine("Please send the contents of this dialog to the developers for investigation. In the meantime, you can upgrade manually.\r\n");
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine(message);
			Console.WriteLine();
			Console.ForegroundColor = ConsoleColor.White;
			Environment.Exit(1);
		}

		/// <summary>
		/// Check whether or not we'll be able to install updates automatically (eg. can write to the folder we're in).
		/// </summary>
		public static bool CanWriteToFolder(DirectoryInfo folder)
		{
			var testFile = Path.Combine(folder.FullName, "TestWriteAccess.txt");

			// Test message to write/read.
			string testMessage = "Test Write";

			// Write the file, read it back, then delete it.
			File.WriteAllText(testFile, testMessage);
			var contents = File.ReadAllText(testFile);
			File.Delete(testFile);

			// Success if the file was written/read successfully.
			return contents == testMessage;
		}

		/// <summary>
		/// Gets the folder the application is install in/running from.
		/// </summary>
		/// <returns></returns>
		public static DirectoryInfo GetApplicationPath()
		{
			var codeBase = Assembly.GetExecutingAssembly().CodeBase;
			var uri = new UriBuilder(codeBase);
			var path = Uri.UnescapeDataString(uri.Path);
			var folder = Path.GetDirectoryName(path);
			return new DirectoryInfo(folder);
		}
	}
}
