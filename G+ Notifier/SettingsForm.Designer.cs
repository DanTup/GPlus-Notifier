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
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.chkUpdatesAutomatic = new System.Windows.Forms.RadioButton();
			this.label1 = new System.Windows.Forms.Label();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.label2 = new System.Windows.Forms.Label();
			this.chkIconNotifications = new System.Windows.Forms.RadioButton();
			this.chkIconBrowser = new System.Windows.Forms.RadioButton();
			this.btnOk = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.label3 = new System.Windows.Forms.Label();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
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
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.chkUpdatesAutomatic);
			this.groupBox1.Controls.Add(this.chkUpdatesAlert);
			this.groupBox1.Location = new System.Drawing.Point(12, 12);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(602, 62);
			this.groupBox1.TabIndex = 1;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Automatic Updates";
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
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(6, 19);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(439, 13);
			this.label1.TabIndex = 2;
			this.label1.Text = "When new versions of G+ Notifier are available, would you like to install them au" +
    "tomatically?";
			// 
			// groupBox2
			// 
			this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox2.Controls.Add(this.label2);
			this.groupBox2.Controls.Add(this.chkIconNotifications);
			this.groupBox2.Controls.Add(this.chkIconBrowser);
			this.groupBox2.Location = new System.Drawing.Point(12, 83);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(602, 62);
			this.groupBox2.TabIndex = 2;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Icon Behaviour";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(6, 19);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(277, 13);
			this.label2.TabIndex = 5;
			this.label2.Text = "When I click the G+ Notifier icon in the notification area...";
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
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
			this.label3.Location = new System.Drawing.Point(221, 39);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(93, 13);
			this.label3.TabIndex = 3;
			this.label3.Text = "(recommended)";
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
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "SettingsForm";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.Text = "G+ Notifier Settings";
			this.Load += new System.EventHandler(this.SettingsForm_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.RadioButton chkUpdatesAlert;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.RadioButton chkUpdatesAutomatic;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.RadioButton chkIconNotifications;
		private System.Windows.Forms.RadioButton chkIconBrowser;
		private System.Windows.Forms.Button btnOk;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Label label3;
	}
}