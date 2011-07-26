namespace DanTup.GPlusNotifier
{
	partial class AboutForm
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
			this.lblTitle = new System.Windows.Forms.Label();
			this.lnkDanny = new System.Windows.Forms.LinkLabel();
			this.label3 = new System.Windows.Forms.Label();
			this.lnkConfigurator = new System.Windows.Forms.LinkLabel();
			this.lnkAdam = new System.Windows.Forms.LinkLabel();
			this.label7 = new System.Windows.Forms.Label();
			this.lnkAndrew = new System.Windows.Forms.LinkLabel();
			this.btnClose = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// lblTitle
			// 
			this.lblTitle.Dock = System.Windows.Forms.DockStyle.Top;
			this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblTitle.Location = new System.Drawing.Point(0, 0);
			this.lblTitle.Name = "lblTitle";
			this.lblTitle.Size = new System.Drawing.Size(284, 47);
			this.lblTitle.TabIndex = 0;
			this.lblTitle.Text = "G+ Notifier";
			this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lnkDanny
			// 
			this.lnkDanny.AutoSize = true;
			this.lnkDanny.Location = new System.Drawing.Point(120, 47);
			this.lnkDanny.Name = "lnkDanny";
			this.lnkDanny.Size = new System.Drawing.Size(83, 13);
			this.lnkDanny.TabIndex = 4;
			this.lnkDanny.TabStop = true;
			this.lnkDanny.Text = "Danny Tuppeny";
			this.lnkDanny.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkDanny_LinkClicked);
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(12, 47);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(102, 13);
			this.label3.TabIndex = 3;
			this.label3.Text = "Created by:";
			this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// lnkConfigurator
			// 
			this.lnkConfigurator.AutoSize = true;
			this.lnkConfigurator.Location = new System.Drawing.Point(120, 99);
			this.lnkConfigurator.Name = "lnkConfigurator";
			this.lnkConfigurator.Size = new System.Drawing.Size(86, 13);
			this.lnkConfigurator.TabIndex = 12;
			this.lnkConfigurator.TabStop = true;
			this.lnkConfigurator.Text = "The Configurator";
			this.lnkConfigurator.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkConfigurator_LinkClicked);
			// 
			// lnkAdam
			// 
			this.lnkAdam.AutoSize = true;
			this.lnkAdam.Location = new System.Drawing.Point(120, 73);
			this.lnkAdam.Name = "lnkAdam";
			this.lnkAdam.Size = new System.Drawing.Size(79, 13);
			this.lnkAdam.TabIndex = 8;
			this.lnkAdam.TabStop = true;
			this.lnkAdam.Text = "Adam Simmons";
			this.lnkAdam.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkAdam_LinkClicked);
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(12, 73);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(102, 13);
			this.label7.TabIndex = 7;
			this.label7.Text = "Contributions from:";
			this.label7.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// lnkAndrew
			// 
			this.lnkAndrew.AutoSize = true;
			this.lnkAndrew.Location = new System.Drawing.Point(120, 125);
			this.lnkAndrew.Name = "lnkAndrew";
			this.lnkAndrew.Size = new System.Drawing.Size(74, 13);
			this.lnkAndrew.TabIndex = 16;
			this.lnkAndrew.TabStop = true;
			this.lnkAndrew.Text = "Andrew Nurse";
			this.lnkAndrew.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkAndrew_LinkClicked);
			// 
			// btnClose
			// 
			this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnClose.Location = new System.Drawing.Point(197, 150);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(75, 23);
			this.btnClose.TabIndex = 17;
			this.btnClose.Text = "&Close";
			this.btnClose.UseVisualStyleBackColor = true;
			// 
			// AboutForm
			// 
			this.AcceptButton = this.btnClose;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnClose;
			this.ClientSize = new System.Drawing.Size(284, 185);
			this.Controls.Add(this.btnClose);
			this.Controls.Add(this.lnkAndrew);
			this.Controls.Add(this.lnkConfigurator);
			this.Controls.Add(this.lnkAdam);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.lnkDanny);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.lblTitle);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "AboutForm";
			this.Text = "About G+ Notifier";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lblTitle;
		private System.Windows.Forms.LinkLabel lnkDanny;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.LinkLabel lnkConfigurator;
		private System.Windows.Forms.LinkLabel lnkAdam;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.LinkLabel lnkAndrew;
		private System.Windows.Forms.Button btnClose;
	}
}