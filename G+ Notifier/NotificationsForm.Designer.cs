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
			((System.ComponentModel.ISupportInitialize)(this.browserPicture)).BeginInit();
			this.SuspendLayout();
			// 
			// NotificationsForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(534, 366);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "NotificationsForm";
			this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.NotificationsForm_KeyUp);
			((System.ComponentModel.ISupportInitialize)(this.browserPicture)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion
	}
}