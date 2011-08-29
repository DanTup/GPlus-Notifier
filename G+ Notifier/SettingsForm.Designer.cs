namespace DanTup.GPlusNotifier
{
	partial class SettingsForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsForm));
			this.chkUpdatesAlert = new System.Windows.Forms.RadioButton();
			this.gbAutomaticUpdates = new System.Windows.Forms.GroupBox();
			this.lblRecommended = new System.Windows.Forms.Label();
			this.lblAutomaticUpdates = new System.Windows.Forms.Label();
			this.chkUpdatesAutomatic = new System.Windows.Forms.RadioButton();
			this.gbIconBehaviour = new System.Windows.Forms.GroupBox();
			this.lblIconBehaviour = new System.Windows.Forms.Label();
			this.chkIconNotifications = new System.Windows.Forms.RadioButton();
			this.chkIconBrowser = new System.Windows.Forms.RadioButton();
			this.btnOk = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.gbAutomaticUpdates.SuspendLayout();
			this.gbIconBehaviour.SuspendLayout();
			this.SuspendLayout();
			// 
			// chkUpdatesAlert
			// 
			this.chkUpdatesAlert.AutoSize = true;
			this.chkUpdatesAlert.Location = new System.Drawing.Point(341, 37);
			this.chkUpdatesAlert.Name = "chkUpdatesAlert";
			this.chkUpdatesAlert.Size = new System.Drawing.Size(136, 17);
			this.chkUpdatesAlert.TabIndex = 0;
			this.chkUpdatesAlert.TabStop = true;
			this.chkUpdatesAlert.Text = "No thanks, just alert me";
			this.chkUpdatesAlert.UseVisualStyleBackColor = true;
			// 
			// gbAutomaticUpdates
			// 
			this.gbAutomaticUpdates.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gbAutomaticUpdates.Controls.Add(this.lblRecommended);
			this.gbAutomaticUpdates.Controls.Add(this.lblAutomaticUpdates);
			this.gbAutomaticUpdates.Controls.Add(this.chkUpdatesAutomatic);
			this.gbAutomaticUpdates.Controls.Add(this.chkUpdatesAlert);
			this.gbAutomaticUpdates.Location = new System.Drawing.Point(12, 12);
			this.gbAutomaticUpdates.Name = "gbAutomaticUpdates";
			this.gbAutomaticUpdates.Size = new System.Drawing.Size(602, 62);
			this.gbAutomaticUpdates.TabIndex = 1;
			this.gbAutomaticUpdates.TabStop = false;
			this.gbAutomaticUpdates.Text = "Automatic Updates";
			// 
			// lblRecommended
			// 
			this.lblRecommended.AutoSize = true;
			this.lblRecommended.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblRecommended.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
			this.lblRecommended.Location = new System.Drawing.Point(221, 39);
			this.lblRecommended.Name = "lblRecommended";
			this.lblRecommended.Size = new System.Drawing.Size(93, 13);
			this.lblRecommended.TabIndex = 3;
			this.lblRecommended.Text = "(recommended)";
			// 
			// lblAutomaticUpdates
			// 
			this.lblAutomaticUpdates.AutoSize = true;
			this.lblAutomaticUpdates.Location = new System.Drawing.Point(6, 19);
			this.lblAutomaticUpdates.Name = "lblAutomaticUpdates";
			this.lblAutomaticUpdates.Size = new System.Drawing.Size(439, 13);
			this.lblAutomaticUpdates.TabIndex = 2;
			this.lblAutomaticUpdates.Text = "When new versions of G+ Notifier are available, would you like to install them au" +
    "tomatically?";
			// 
			// chkUpdatesAutomatic
			// 
			this.chkUpdatesAutomatic.AutoSize = true;
			this.chkUpdatesAutomatic.Location = new System.Drawing.Point(21, 37);
			this.chkUpdatesAutomatic.Name = "chkUpdatesAutomatic";
			this.chkUpdatesAutomatic.Size = new System.Drawing.Size(204, 17);
			this.chkUpdatesAutomatic.TabIndex = 1;
			this.chkUpdatesAutomatic.TabStop = true;
			this.chkUpdatesAutomatic.Text = "Yes, automatically install new versions";
			this.chkUpdatesAutomatic.UseVisualStyleBackColor = true;
			this.chkUpdatesAutomatic.CheckedChanged += new System.EventHandler(this.chkUpdatesAutomatic_CheckedChanged);
			// 
			// gbIconBehaviour
			// 
			this.gbIconBehaviour.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gbIconBehaviour.Controls.Add(this.lblIconBehaviour);
			this.gbIconBehaviour.Controls.Add(this.chkIconNotifications);
			this.gbIconBehaviour.Controls.Add(this.chkIconBrowser);
			this.gbIconBehaviour.Location = new System.Drawing.Point(12, 83);
			this.gbIconBehaviour.Name = "gbIconBehaviour";
			this.gbIconBehaviour.Size = new System.Drawing.Size(602, 62);
			this.gbIconBehaviour.TabIndex = 2;
			this.gbIconBehaviour.TabStop = false;
			this.gbIconBehaviour.Text = "Icon Behaviour";
			// 
			// lblIconBehaviour
			// 
			this.lblIconBehaviour.AutoSize = true;
			this.lblIconBehaviour.Location = new System.Drawing.Point(6, 19);
			this.lblIconBehaviour.Name = "lblIconBehaviour";
			this.lblIconBehaviour.Size = new System.Drawing.Size(277, 13);
			this.lblIconBehaviour.TabIndex = 5;
			this.lblIconBehaviour.Text = "When I click the G+ Notifier icon in the notification area...";
			// 
			// chkIconNotifications
			// 
			this.chkIconNotifications.AutoSize = true;
			this.chkIconNotifications.Location = new System.Drawing.Point(21, 37);
			this.chkIconNotifications.Name = "chkIconNotifications";
			this.chkIconNotifications.Size = new System.Drawing.Size(214, 17);
			this.chkIconNotifications.TabIndex = 4;
			this.chkIconNotifications.TabStop = true;
			this.chkIconNotifications.Text = "Show the compact Notifications window";
			this.chkIconNotifications.UseVisualStyleBackColor = true;
			// 
			// chkIconBrowser
			// 
			this.chkIconBrowser.AutoSize = true;
			this.chkIconBrowser.Location = new System.Drawing.Point(341, 37);
			this.chkIconBrowser.Name = "chkIconBrowser";
			this.chkIconBrowser.Size = new System.Drawing.Size(206, 17);
			this.chkIconBrowser.TabIndex = 3;
			this.chkIconBrowser.TabStop = true;
			this.chkIconBrowser.Text = "Launch Google+ in my default browser";
			this.chkIconBrowser.UseVisualStyleBackColor = true;
			// 
			// btnOk
			// 
			this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOk.Location = new System.Drawing.Point(458, 154);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new System.Drawing.Size(75, 23);
			this.btnOk.TabIndex = 3;
			this.btnOk.Text = "OK";
			this.btnOk.UseVisualStyleBackColor = true;
			this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(539, 154);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 4;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// SettingsForm
			// 
			this.AcceptButton = this.btnOk;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(626, 189);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOk);
			this.Controls.Add(this.gbIconBehaviour);
			this.Controls.Add(this.gbAutomaticUpdates);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "SettingsForm";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.Text = "G+ Notifier Settings";
			this.Load += new System.EventHandler(this.SettingsForm_Load);
			this.gbAutomaticUpdates.ResumeLayout(false);
			this.gbAutomaticUpdates.PerformLayout();
			this.gbIconBehaviour.ResumeLayout(false);
			this.gbIconBehaviour.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.RadioButton chkUpdatesAlert;
		private System.Windows.Forms.GroupBox gbAutomaticUpdates;
		private System.Windows.Forms.Label lblAutomaticUpdates;
		private System.Windows.Forms.RadioButton chkUpdatesAutomatic;
		private System.Windows.Forms.GroupBox gbIconBehaviour;
		private System.Windows.Forms.Label lblIconBehaviour;
		private System.Windows.Forms.RadioButton chkIconNotifications;
		private System.Windows.Forms.RadioButton chkIconBrowser;
		private System.Windows.Forms.Button btnOk;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Label lblRecommended;
	}
}