namespace DanTup.GPlusNotifier
{
	partial class LoginForm
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
			this.lblLoginInstructions = new System.Windows.Forms.Label();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnOK = new System.Windows.Forms.Button();
			this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
			this.browserPicture = new DanTup.GPlusNotifier.SelectablePictureBox();
			((System.ComponentModel.ISupportInitialize)(this.browserPicture)).BeginInit();
			this.SuspendLayout();
			// 
			// lblLoginInstructions
			// 
			this.lblLoginInstructions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lblLoginInstructions.Location = new System.Drawing.Point(12, 9);
			this.lblLoginInstructions.Name = "lblLoginInstructions";
			this.lblLoginInstructions.Size = new System.Drawing.Size(760, 17);
			this.lblLoginInstructions.TabIndex = 0;
			this.lblLoginInstructions.Text = "Please login with your Google Plus account and once logged in, click the \"OK\" but" +
    "ton at the bottom of the form.";
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(697, 527);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 2;
			this.btnCancel.TabStop = false;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// btnOK
			// 
			this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.Location = new System.Drawing.Point(616, 527);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(75, 23);
			this.btnOK.TabIndex = 1;
			this.btnOK.TabStop = false;
			this.btnOK.Text = "OK";
			this.btnOK.UseVisualStyleBackColor = true;
			// 
			// browserPicture
			// 
			this.browserPicture.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.browserPicture.Location = new System.Drawing.Point(15, 29);
			this.browserPicture.Name = "browserPicture";
			this.browserPicture.Size = new System.Drawing.Size(757, 492);
			this.browserPicture.TabIndex = 0;
			this.browserPicture.TabStop = false;
			// 
			// LoginForm
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(784, 562);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.lblLoginInstructions);
			this.Controls.Add(this.browserPicture);
			this.Name = "LoginForm";
			this.Text = "LoginForm";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LoginForm_FormClosing);
			((System.ComponentModel.ISupportInitialize)(this.browserPicture)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private SelectablePictureBox browserPicture;
		private System.Windows.Forms.Label lblLoginInstructions;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnOK;
		private System.ComponentModel.BackgroundWorker backgroundWorker1;
	}
}