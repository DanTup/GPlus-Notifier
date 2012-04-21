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
			this.gbCheckFrequency = new System.Windows.Forms.GroupBox();
			this.lblCheckFrequency = new System.Windows.Forms.Label();
			this.chk30secs = new System.Windows.Forms.RadioButton();
			this.chk10mins = new System.Windows.Forms.RadioButton();
			this.chk1min = new System.Windows.Forms.RadioButton();
			this.chk30mins = new System.Windows.Forms.RadioButton();
			this.label1 = new System.Windows.Forms.Label();
			this.gbAutomaticUpdates.SuspendLayout();
			this.gbIconBehaviour.SuspendLayout();
			this.gbCheckFrequency.SuspendLayout();
			this.SuspendLayout();
			// 
			// chkUpdatesAlert
			// 
			this.chkUpdatesAlert.AutoSize = true;
			this.chkUpdatesAlert.Location = new System.Drawing.Point(455, 46);
			this.chkUpdatesAlert.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.chkUpdatesAlert.Name = "chkUpdatesAlert";
			this.chkUpdatesAlert.Size = new System.Drawing.Size(178, 21);
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
			this.gbAutomaticUpdates.Location = new System.Drawing.Point(16, 15);
			this.gbAutomaticUpdates.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.gbAutomaticUpdates.Name = "gbAutomaticUpdates";
			this.gbAutomaticUpdates.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.gbAutomaticUpdates.Size = new System.Drawing.Size(803, 76);
			this.gbAutomaticUpdates.TabIndex = 1;
			this.gbAutomaticUpdates.TabStop = false;
			this.gbAutomaticUpdates.Text = "Automatic Updates";
			// 
			// lblRecommended
			// 
			this.lblRecommended.AutoSize = true;
			this.lblRecommended.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblRecommended.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
			this.lblRecommended.Location = new System.Drawing.Point(295, 48);
			this.lblRecommended.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.lblRecommended.Name = "lblRecommended";
			this.lblRecommended.Size = new System.Drawing.Size(121, 17);
			this.lblRecommended.TabIndex = 3;
			this.lblRecommended.Text = "(recommended)";
			// 
			// lblAutomaticUpdates
			// 
			this.lblAutomaticUpdates.AutoSize = true;
			this.lblAutomaticUpdates.Location = new System.Drawing.Point(8, 23);
			this.lblAutomaticUpdates.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.lblAutomaticUpdates.Name = "lblAutomaticUpdates";
			this.lblAutomaticUpdates.Size = new System.Drawing.Size(585, 17);
			this.lblAutomaticUpdates.TabIndex = 2;
			this.lblAutomaticUpdates.Text = "When new versions of G+ Notifier are available, would you like to install them au" +
    "tomatically?";
			// 
			// chkUpdatesAutomatic
			// 
			this.chkUpdatesAutomatic.AutoSize = true;
			this.chkUpdatesAutomatic.Location = new System.Drawing.Point(28, 46);
			this.chkUpdatesAutomatic.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.chkUpdatesAutomatic.Name = "chkUpdatesAutomatic";
			this.chkUpdatesAutomatic.Size = new System.Drawing.Size(269, 21);
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
			this.gbIconBehaviour.Location = new System.Drawing.Point(16, 102);
			this.gbIconBehaviour.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.gbIconBehaviour.Name = "gbIconBehaviour";
			this.gbIconBehaviour.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.gbIconBehaviour.Size = new System.Drawing.Size(803, 76);
			this.gbIconBehaviour.TabIndex = 2;
			this.gbIconBehaviour.TabStop = false;
			this.gbIconBehaviour.Text = "Icon Behaviour";
			// 
			// lblIconBehaviour
			// 
			this.lblIconBehaviour.AutoSize = true;
			this.lblIconBehaviour.Location = new System.Drawing.Point(8, 23);
			this.lblIconBehaviour.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.lblIconBehaviour.Name = "lblIconBehaviour";
			this.lblIconBehaviour.Size = new System.Drawing.Size(365, 17);
			this.lblIconBehaviour.TabIndex = 5;
			this.lblIconBehaviour.Text = "When I click the G+ Notifier icon in the notification area...";
			// 
			// chkIconNotifications
			// 
			this.chkIconNotifications.AutoSize = true;
			this.chkIconNotifications.Location = new System.Drawing.Point(28, 46);
			this.chkIconNotifications.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.chkIconNotifications.Name = "chkIconNotifications";
			this.chkIconNotifications.Size = new System.Drawing.Size(274, 21);
			this.chkIconNotifications.TabIndex = 4;
			this.chkIconNotifications.TabStop = true;
			this.chkIconNotifications.Text = "Show the compact Notifications window";
			this.chkIconNotifications.UseVisualStyleBackColor = true;
			// 
			// chkIconBrowser
			// 
			this.chkIconBrowser.AutoSize = true;
			this.chkIconBrowser.Location = new System.Drawing.Point(455, 46);
			this.chkIconBrowser.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.chkIconBrowser.Name = "chkIconBrowser";
			this.chkIconBrowser.Size = new System.Drawing.Size(272, 21);
			this.chkIconBrowser.TabIndex = 3;
			this.chkIconBrowser.TabStop = true;
			this.chkIconBrowser.Text = "Launch Google+ in my default browser";
			this.chkIconBrowser.UseVisualStyleBackColor = true;
			// 
			// btnOk
			// 
			this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOk.Location = new System.Drawing.Point(611, 276);
			this.btnOk.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new System.Drawing.Size(100, 28);
			this.btnOk.TabIndex = 3;
			this.btnOk.Text = "OK";
			this.btnOk.UseVisualStyleBackColor = true;
			this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(719, 276);
			this.btnCancel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(100, 28);
			this.btnCancel.TabIndex = 4;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// gbCheckFrequency
			// 
			this.gbCheckFrequency.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gbCheckFrequency.Controls.Add(this.label1);
			this.gbCheckFrequency.Controls.Add(this.chk30mins);
			this.gbCheckFrequency.Controls.Add(this.chk1min);
			this.gbCheckFrequency.Controls.Add(this.lblCheckFrequency);
			this.gbCheckFrequency.Controls.Add(this.chk30secs);
			this.gbCheckFrequency.Controls.Add(this.chk10mins);
			this.gbCheckFrequency.Location = new System.Drawing.Point(16, 186);
			this.gbCheckFrequency.Margin = new System.Windows.Forms.Padding(4);
			this.gbCheckFrequency.Name = "gbCheckFrequency";
			this.gbCheckFrequency.Padding = new System.Windows.Forms.Padding(4);
			this.gbCheckFrequency.Size = new System.Drawing.Size(803, 76);
			this.gbCheckFrequency.TabIndex = 5;
			this.gbCheckFrequency.TabStop = false;
			this.gbCheckFrequency.Text = "Frequency";
			// 
			// lblCheckFrequency
			// 
			this.lblCheckFrequency.AutoSize = true;
			this.lblCheckFrequency.Location = new System.Drawing.Point(8, 23);
			this.lblCheckFrequency.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.lblCheckFrequency.Name = "lblCheckFrequency";
			this.lblCheckFrequency.Size = new System.Drawing.Size(227, 17);
			this.lblCheckFrequency.TabIndex = 5;
			this.lblCheckFrequency.Text = "Check for new notifications every...";
			// 
			// chk30secs
			// 
			this.chk30secs.AutoSize = true;
			this.chk30secs.Location = new System.Drawing.Point(28, 46);
			this.chk30secs.Margin = new System.Windows.Forms.Padding(4);
			this.chk30secs.Name = "chk30secs";
			this.chk30secs.Size = new System.Drawing.Size(102, 21);
			this.chk30secs.TabIndex = 4;
			this.chk30secs.TabStop = true;
			this.chk30secs.Text = "30 seconds";
			this.chk30secs.UseVisualStyleBackColor = true;
			// 
			// chk10mins
			// 
			this.chk10mins.AutoSize = true;
			this.chk10mins.Location = new System.Drawing.Point(455, 46);
			this.chk10mins.Margin = new System.Windows.Forms.Padding(4);
			this.chk10mins.Name = "chk10mins";
			this.chk10mins.Size = new System.Drawing.Size(78, 21);
			this.chk10mins.TabIndex = 3;
			this.chk10mins.TabStop = true;
			this.chk10mins.Text = "10 mins";
			this.chk10mins.UseVisualStyleBackColor = true;
			// 
			// chk1min
			// 
			this.chk1min.AutoSize = true;
			this.chk1min.Location = new System.Drawing.Point(240, 46);
			this.chk1min.Margin = new System.Windows.Forms.Padding(4);
			this.chk1min.Name = "chk1min";
			this.chk1min.Size = new System.Drawing.Size(63, 21);
			this.chk1min.TabIndex = 6;
			this.chk1min.TabStop = true;
			this.chk1min.Text = "1 min";
			this.chk1min.UseVisualStyleBackColor = true;
			// 
			// chk30mins
			// 
			this.chk30mins.AutoSize = true;
			this.chk30mins.Location = new System.Drawing.Point(670, 46);
			this.chk30mins.Margin = new System.Windows.Forms.Padding(4);
			this.chk30mins.Name = "chk30mins";
			this.chk30mins.Size = new System.Drawing.Size(78, 21);
			this.chk30mins.TabIndex = 7;
			this.chk30mins.TabStop = true;
			this.chk30mins.Text = "30 mins";
			this.chk30mins.UseVisualStyleBackColor = true;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.label1.Location = new System.Drawing.Point(240, 22);
			this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(133, 17);
			this.label1.TabIndex = 4;
			this.label1.Text = "(requires restart)";
			// 
			// SettingsForm
			// 
			this.AcceptButton = this.btnOk;
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(835, 319);
			this.Controls.Add(this.gbCheckFrequency);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOk);
			this.Controls.Add(this.gbIconBehaviour);
			this.Controls.Add(this.gbAutomaticUpdates);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
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
			this.gbCheckFrequency.ResumeLayout(false);
			this.gbCheckFrequency.PerformLayout();
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
		private System.Windows.Forms.GroupBox gbCheckFrequency;
		private System.Windows.Forms.RadioButton chk30mins;
		private System.Windows.Forms.RadioButton chk1min;
		private System.Windows.Forms.Label lblCheckFrequency;
		private System.Windows.Forms.RadioButton chk30secs;
		private System.Windows.Forms.RadioButton chk10mins;
		private System.Windows.Forms.Label label1;
	}
}