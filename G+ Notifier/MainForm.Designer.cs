namespace DanTup.GPlusNotifier
{
	partial class MainForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.notificationIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.notificatinIconMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.gNotifierWebsiteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gNotifierOnGoogleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dannyTuppenyOnGoogleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.donateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.feedbackSupportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.loginToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearCookiesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.checkLoginTimer = new System.Windows.Forms.Timer(this.components);
            this.checkForUpdates = new System.Windows.Forms.Timer(this.components);
            this.checkForUpdatesWorker = new System.ComponentModel.BackgroundWorker();
            this.reloadTimer = new System.Windows.Forms.Timer(this.components);
            this.notificatinIconMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // notificationIcon
            // 
            this.notificationIcon.ContextMenuStrip = this.notificatinIconMenu;
            this.notificationIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notificationIcon.Icon")));
            this.notificationIcon.Text = "G+ Notifier";
            this.notificationIcon.Visible = true;
            this.notificationIcon.BalloonTipClicked += new System.EventHandler(this.notificationIcon_BalloonTipClicked);
            this.notificationIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notificationIcon_MouseDoubleClick);
            // 
            // notificatinIconMenu
            // 
            this.notificatinIconMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.gNotifierWebsiteToolStripMenuItem,
            this.gNotifierOnGoogleToolStripMenuItem,
            this.dannyTuppenyOnGoogleToolStripMenuItem,
            this.donateToolStripMenuItem,
            this.feedbackSupportToolStripMenuItem,
            this.toolStripSeparator1,
            this.loginToolStripMenuItem,
            this.clearCookiesToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.notificatinIconMenu.Name = "notificatinIconMenu";
            this.notificatinIconMenu.Size = new System.Drawing.Size(225, 208);
            // 
            // gNotifierWebsiteToolStripMenuItem
            // 
            this.gNotifierWebsiteToolStripMenuItem.Name = "gNotifierWebsiteToolStripMenuItem";
            this.gNotifierWebsiteToolStripMenuItem.Size = new System.Drawing.Size(224, 22);
            this.gNotifierWebsiteToolStripMenuItem.Text = "G+ Notifier &Website";
            this.gNotifierWebsiteToolStripMenuItem.Click += new System.EventHandler(this.gNotifierWebsiteToolStripMenuItem_Click);
            // 
            // gNotifierOnGoogleToolStripMenuItem
            // 
            this.gNotifierOnGoogleToolStripMenuItem.Name = "gNotifierOnGoogleToolStripMenuItem";
            this.gNotifierOnGoogleToolStripMenuItem.Size = new System.Drawing.Size(224, 22);
            this.gNotifierOnGoogleToolStripMenuItem.Text = "&G+ Notifier on Google+";
            this.gNotifierOnGoogleToolStripMenuItem.Click += new System.EventHandler(this.gNotifierOnGoogleToolStripMenuItem_Click);
            // 
            // dannyTuppenyOnGoogleToolStripMenuItem
            // 
            this.dannyTuppenyOnGoogleToolStripMenuItem.Name = "dannyTuppenyOnGoogleToolStripMenuItem";
            this.dannyTuppenyOnGoogleToolStripMenuItem.Size = new System.Drawing.Size(224, 22);
            this.dannyTuppenyOnGoogleToolStripMenuItem.Text = "&Danny Tuppeny on Google+";
            this.dannyTuppenyOnGoogleToolStripMenuItem.Click += new System.EventHandler(this.dannyTuppenyOnGoogleToolStripMenuItem_Click);
            // 
            // donateToolStripMenuItem
            // 
            this.donateToolStripMenuItem.Name = "donateToolStripMenuItem";
            this.donateToolStripMenuItem.Size = new System.Drawing.Size(224, 22);
            this.donateToolStripMenuItem.Text = "D&onate";
            this.donateToolStripMenuItem.Click += new System.EventHandler(this.donateToolStripMenuItem_Click);
            // 
            // feedbackSupportToolStripMenuItem
            // 
            this.feedbackSupportToolStripMenuItem.Name = "feedbackSupportToolStripMenuItem";
            this.feedbackSupportToolStripMenuItem.Size = new System.Drawing.Size(224, 22);
            this.feedbackSupportToolStripMenuItem.Text = "&Feedback && Support";
            this.feedbackSupportToolStripMenuItem.Click += new System.EventHandler(this.feedbackSupportMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(221, 6);
            // 
            // loginToolStripMenuItem
            // 
            this.loginToolStripMenuItem.Name = "loginToolStripMenuItem";
            this.loginToolStripMenuItem.Size = new System.Drawing.Size(224, 22);
            this.loginToolStripMenuItem.Text = "&Login";
            this.loginToolStripMenuItem.Click += new System.EventHandler(this.loginToolStripMenuItem_Click);
            // 
            // clearCookiesToolStripMenuItem
            // 
            this.clearCookiesToolStripMenuItem.Name = "clearCookiesToolStripMenuItem";
            this.clearCookiesToolStripMenuItem.Size = new System.Drawing.Size(224, 22);
            this.clearCookiesToolStripMenuItem.Text = "Clear &Cookies";
            this.clearCookiesToolStripMenuItem.Click += new System.EventHandler(this.clearCookiesToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(224, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // checkLoginTimer
            // 
            this.checkLoginTimer.Enabled = true;
            this.checkLoginTimer.Interval = 2000;
            this.checkLoginTimer.Tick += new System.EventHandler(this.checkNotificationsTimer_Tick);
            // 
            // checkForUpdates
            // 
            this.checkForUpdates.Enabled = true;
            this.checkForUpdates.Interval = 86400000;
            this.checkForUpdates.Tick += new System.EventHandler(this.checkForUpdates_Tick);
            // 
            // checkForUpdatesWorker
            // 
            this.checkForUpdatesWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.checkForUpdatesWorker_DoWork);
            this.checkForUpdatesWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.checkForUpdatesWorker_RunWorkerCompleted);
            // 
            // reloadTimer
            // 
            this.reloadTimer.Enabled = true;
            this.reloadTimer.Interval = 30000;
            this.reloadTimer.Tick += new System.EventHandler(this.reloadTimer_Tick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MainForm";
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.notificatinIconMenu.ResumeLayout(false);
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.NotifyIcon notificationIcon;
		private System.Windows.Forms.ContextMenuStrip notificatinIconMenu;
		private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
		private System.Windows.Forms.Timer checkLoginTimer;
		private System.Windows.Forms.ToolStripMenuItem clearCookiesToolStripMenuItem;
		private System.Windows.Forms.Timer checkForUpdates;
		private System.ComponentModel.BackgroundWorker checkForUpdatesWorker;
		private System.Windows.Forms.ToolStripMenuItem gNotifierOnGoogleToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem dannyTuppenyOnGoogleToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem donateToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem gNotifierWebsiteToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem loginToolStripMenuItem;
        private System.Windows.Forms.Timer reloadTimer;
        private System.Windows.Forms.ToolStripMenuItem feedbackSupportToolStripMenuItem;
	}
}