﻿namespace DanTup.GPlusNotifier
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
			this.clearCookiesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.checkNotificationsTimer = new System.Windows.Forms.Timer(this.components);
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
            this.clearCookiesToolStripMenuItem,
            this.exitToolStripMenuItem});
			this.notificatinIconMenu.Name = "notificatinIconMenu";
			this.notificatinIconMenu.Size = new System.Drawing.Size(153, 70);
			// 
			// clearCookiesToolStripMenuItem
			// 
			this.clearCookiesToolStripMenuItem.Name = "clearCookiesToolStripMenuItem";
			this.clearCookiesToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.clearCookiesToolStripMenuItem.Text = "Clear &Cookies";
			this.clearCookiesToolStripMenuItem.Click += new System.EventHandler(this.clearCookiesToolStripMenuItem_Click);
			// 
			// exitToolStripMenuItem
			// 
			this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
			this.exitToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.exitToolStripMenuItem.Text = "E&xit";
			this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
			// 
			// checkNotificationsTimer
			// 
			this.checkNotificationsTimer.Enabled = true;
			this.checkNotificationsTimer.Interval = 10000;
			this.checkNotificationsTimer.Tick += new System.EventHandler(this.checkNotificationsTimer_Tick);
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
		private System.Windows.Forms.Timer checkNotificationsTimer;
		private System.Windows.Forms.ToolStripMenuItem clearCookiesToolStripMenuItem;
	}
}