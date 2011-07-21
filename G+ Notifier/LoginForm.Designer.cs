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
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.browserPicture = new DanTup.GPlusNotifier.SelectablePictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.browserPicture)).BeginInit();
            this.SuspendLayout();
            // 
            // lblLoginInstructions
            // 
            this.lblLoginInstructions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblLoginInstructions.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLoginInstructions.Location = new System.Drawing.Point(12, 9);
            this.lblLoginInstructions.Name = "lblLoginInstructions";
            this.lblLoginInstructions.Size = new System.Drawing.Size(415, 54);
            this.lblLoginInstructions.TabIndex = 0;
            this.lblLoginInstructions.Text = "Please login with your Google Plus account. This window will automatically disapp" +
                "ear once your account is connected.";
            // 
            // browserPicture
            // 
            this.browserPicture.Location = new System.Drawing.Point(1, 66);
            this.browserPicture.Name = "browserPicture";
            this.browserPicture.Size = new System.Drawing.Size(439, 380);
            this.browserPicture.TabIndex = 0;
            this.browserPicture.TabStop = false;
            // 
            // LoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(439, 445);
            this.Controls.Add(this.lblLoginInstructions);
            this.Controls.Add(this.browserPicture);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.Name = "LoginForm";
            this.Text = "Please login...";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LoginForm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.browserPicture)).EndInit();
            this.ResumeLayout(false);

		}

		#endregion

		private SelectablePictureBox browserPicture;
        private System.Windows.Forms.Label lblLoginInstructions;
		private System.ComponentModel.BackgroundWorker backgroundWorker1;
	}
}