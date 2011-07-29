using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;

namespace DanTup.GPlusNotifier
{
	public partial class AboutForm : Form
	{
		public AboutForm()
		{
			InitializeComponent();

			// Set the title to have the version number in it
			lblTitle.Text = "G+ Notifier " + Assembly.GetExecutingAssembly().GetName().Version.ToString(2);
		}

		private void lnkDanny_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			Process.Start("https://plus.google.com/116849139972638476037");
		}

		private void lnkAdam_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			Process.Start("https://plus.google.com/109531055133116706775");
		}

		private void lnkConfigurator_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			Process.Start("https://plus.google.com/112139137835193833820");
		}

		private void lnkAndrew_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			Process.Start("https://plus.google.com/114121441074506314281");
		}
	}
}
