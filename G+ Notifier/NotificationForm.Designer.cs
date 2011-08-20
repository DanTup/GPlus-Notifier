namespace DanTup.GPlusNotifier
{
	partial class NotificationForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NotificationForm));
			this.lblTitle = new System.Windows.Forms.Label();
			this.fadeInTimer = new System.Windows.Forms.Timer(this.components);
			this.fadeOutTimer = new System.Windows.Forms.Timer(this.components);
			this.timeoutTimer = new System.Windows.Forms.Timer(this.components);
			this.lblMessage = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// lblTitle
			// 
			this.lblTitle.BackColor = System.Drawing.Color.Transparent;
			this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblTitle.ForeColor = System.Drawing.Color.White;
			this.lblTitle.Location = new System.Drawing.Point(79, 11);
			this.lblTitle.Name = "lblTitle";
			this.lblTitle.Size = new System.Drawing.Size(209, 20);
			this.lblTitle.TabIndex = 0;
			this.lblTitle.Text = "Lorem ipsum dolor sit";
			// 
			// fadeInTimer
			// 
			this.fadeInTimer.Interval = 10;
			this.fadeInTimer.Tick += new System.EventHandler(this.fadeTimer_Tick);
			// 
			// fadeOutTimer
			// 
			this.fadeOutTimer.Interval = 10;
			this.fadeOutTimer.Tick += new System.EventHandler(this.fadeOutTimer_Tick);
			// 
			// timeoutTimer
			// 
			this.timeoutTimer.Tick += new System.EventHandler(this.timeoutTimer_Tick);
			// 
			// lblMessage
			// 
			this.lblMessage.BackColor = System.Drawing.Color.Transparent;
			this.lblMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblMessage.ForeColor = System.Drawing.Color.White;
			this.lblMessage.Location = new System.Drawing.Point(80, 33);
			this.lblMessage.Name = "lblMessage";
			this.lblMessage.Size = new System.Drawing.Size(209, 38);
			this.lblMessage.TabIndex = 1;
			this.lblMessage.Text = "Suspendisse dictum blandit elit, vel egestas diam gravida sed!";
			// 
			// NotificationForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
			this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
			this.ClientSize = new System.Drawing.Size(300, 80);
			this.ControlBox = false;
			this.Controls.Add(this.lblMessage);
			this.Controls.Add(this.lblTitle);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "NotificationForm";
			this.Opacity = 0D;
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "G+ Notifier Notification";
			this.TopMost = true;
			this.TransparencyKey = System.Drawing.SystemColors.Control;
			this.Shown += new System.EventHandler(this.NotificationForm_Shown);
			this.Click += new System.EventHandler(this.NotificationForm_Click);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label lblTitle;
		private System.Windows.Forms.Timer fadeInTimer;
		private System.Windows.Forms.Timer fadeOutTimer;
		private System.Windows.Forms.Timer timeoutTimer;
		private System.Windows.Forms.Label lblMessage;
	}
}