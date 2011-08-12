using System;
using System.Windows.Forms;
using DanTup.GPlusNotifier.Properties;

namespace DanTup.GPlusNotifier
{
	public partial class SettingsForm : Form
	{
		UpdaterHelper updater = new UpdaterHelper();

		public SettingsForm()
		{
			InitializeComponent();
		}

		private void SettingsForm_Load(object sender, EventArgs e)
		{
			// Set up the radio options based on the current settings.

			// Auto-updates
			chkUpdatesAutomatic.Checked = Settings.Default.AutomaticallyInstallUpdates;
			chkUpdatesAlert.Checked = !chkUpdatesAutomatic.Checked;

			// Icon action
			chkIconNotifications.Checked = Settings.Default.UseNotificationWindow;
			chkIconBrowser.Checked = !chkIconNotifications.Checked;
		}

		private void btnOk_Click(object sender, EventArgs e)
		{
			Settings.Default.AutomaticallyInstallUpdates = chkUpdatesAutomatic.Checked;
			Settings.Default.UseNotificationWindow = chkIconNotifications.Checked;
			Settings.Default.Save();

			this.Close();
		}

		private void chkUpdatesAutomatic_CheckedChanged(object sender, EventArgs e)
		{
			if (chkUpdatesAutomatic.Checked)
			{
				if (!updater.CanWriteToApplicationFolder())
				{
					MessageBox.Show("Unable to write to the folder G+ Notifier is installed in. Automatic updates will be unavailable.", "Automatic Updates");
					chkUpdatesAlert.Checked = true;
					chkUpdatesAutomatic.Checked = false;
					return;
				}

				// Can use the Shell to unzip (although XP supports it, this class barfs)
				if (!updater.CanUnzipFiles())
				{
					MessageBox.Show("Automatic updates are not supported on this version of Windows :-(", "Automatic Updates");
					chkUpdatesAlert.Checked = true;
					chkUpdatesAutomatic.Checked = false;
					return;
				}
			}
		}
	}
}
