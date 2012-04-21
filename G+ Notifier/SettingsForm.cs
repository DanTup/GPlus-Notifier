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

			SetupTranslations();
		}

		private void SetupTranslations()
		{
			this.Text = Translations.SettingsTitle;
			gbAutomaticUpdates.Text = Translations.AutomaticUpdatesGroupText;
			lblAutomaticUpdates.Text = Translations.AutomaticUpdatesText;
			chkUpdatesAutomatic.Text = Translations.AutomaticUpdatesYes;
			chkUpdatesAlert.Text = Translations.AutomaticUpdatesNo;
			lblRecommended.Text = Translations.RecommendedText;
			gbIconBehaviour.Text = Translations.IconBehaviourGroupText;
			lblIconBehaviour.Text = Translations.IconBehaviourText;
			chkIconNotifications.Text = Translations.IconBehaviourNotifications;
			chkIconBrowser.Text = Translations.IconBehaviourBrowser;
			gbCheckFrequency.Text = Translations.CheckFrequencyGroupText;
			lblCheckFrequency.Text = Translations.CheckFrequencyText;
			chk30secs.Text = Translations.CheckFrequency30secs;
			chk1min.Text = Translations.CheckFrequency1min;
			chk10mins.Text = Translations.CheckFrequency10mins;
			chk30mins.Text = Translations.CheckFrequency30mins;
			btnOk.Text = Translations.OkButtonText;
			btnCancel.Text = Translations.CancelButtonText;
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

			// Check frequency
			chk30secs.Checked = Settings.Default.CheckFrequencySeconds == 30;
			chk1min.Checked = Settings.Default.CheckFrequencySeconds == 60;
			chk10mins.Checked = Settings.Default.CheckFrequencySeconds == 60 * 10;
			chk30mins.Checked = Settings.Default.CheckFrequencySeconds == 60 * 30;
		}

		private void btnOk_Click(object sender, EventArgs e)
		{
			Settings.Default.AutomaticallyInstallUpdates = chkUpdatesAutomatic.Checked;
			Settings.Default.UseNotificationWindow = chkIconNotifications.Checked;
			Settings.Default.CheckFrequencySeconds =
				(chk30secs.Checked ? 30 :
				(chk1min.Checked ? 60 :
				(chk10mins.Checked ? 60 * 10 :
				(chk30mins.Checked ? 60 * 30 :
				30)))); // Default
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
