namespace DanTup.GPlusNotifier
{
	partial class NotificationsForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NotificationsForm));
			this.browserPicture = new System.Windows.Forms.PictureBox();
			((System.ComponentModel.ISupportInitialize)(this.browserPicture)).BeginInit();
			this.SuspendLayout();
			// 
			// webPicture
			// 
			this.browserPicture.Dock = System.Windows.Forms.DockStyle.Fill;
			this.browserPicture.Location = new System.Drawing.Point(0, 0);
			this.browserPicture.Name = "webPicture";
			this.browserPicture.Size = new System.Drawing.Size(534, 366);
			this.browserPicture.TabIndex = 0;
			this.browserPicture.TabStop = false;
			// 
			// NotificationsForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(534, 366);
			this.Controls.Add(this.browserPicture);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.KeyPreview = true;
			this.Name = "NotificationsForm";
			this.Text = "G+ Notifier";
			((System.ComponentModel.ISupportInitialize)(this.browserPicture)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion
	}
}