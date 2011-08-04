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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginForm));
			this.lblLoginInstructions = new System.Windows.Forms.Label();
			this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
			((System.ComponentModel.ISupportInitialize)(this.browserPicture)).BeginInit();
			this.SuspendLayout();
			// 
			// browserPicture
			// 
			this.browserPicture.Location = new System.Drawing.Point(0, 47);
			this.browserPicture.Size = new System.Drawing.Size(439, 398);
			// 
			// lblLoginInstructions
			// 
			this.lblLoginInstructions.Dock = System.Windows.Forms.DockStyle.Top;
			this.lblLoginInstructions.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblLoginInstructions.Location = new System.Drawing.Point(0, 0);
			this.lblLoginInstructions.Name = "lblLoginInstructions";
			this.lblLoginInstructions.Padding = new System.Windows.Forms.Padding(5);
			this.lblLoginInstructions.Size = new System.Drawing.Size(439, 47);
			this.lblLoginInstructions.TabIndex = 0;
			this.lblLoginInstructions.Text = "Please login to your Google Plus account below. \r\nThis window will disappear once" +
    " your account is connected.";
			// 
			// LoginForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(439, 445);
			this.Controls.Add(this.lblLoginInstructions);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "LoginForm";
			this.Text = "G+ Notifier - Login Now";
			this.Controls.SetChildIndex(this.lblLoginInstructions, 0);
			this.Controls.SetChildIndex(this.browserPicture, 0);
			((System.ComponentModel.ISupportInitialize)(this.browserPicture)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label lblLoginInstructions;
		private System.ComponentModel.BackgroundWorker backgroundWorker1;
	}
}