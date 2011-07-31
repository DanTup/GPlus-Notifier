﻿
namespace DanTup.GPlusNotifier
{
	public partial class LoginForm : AwesomiumForm
	{
		public LoginForm()
		{
			InitializeComponent();

			SetupBrowser();

			// Subscribe to DomReady so we can pass the event up.
			this.webView.DomReady += PageChanged;

			// Load the mobile login page.
			webView.LoadURL("https://www.google.com/accounts/ServiceLogin?service=webupdates&btmpl=mobile&ltmpl=mobile&continue=http%3a%2f%2fwww.google.com%2fwebhp%3ftab%3dww");
			webView.Focus();
		}

		public event System.EventHandler PageChanged;

	}
}
