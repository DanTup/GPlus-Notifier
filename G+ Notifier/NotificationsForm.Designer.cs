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
            this.webPicture = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.webPicture)).BeginInit();
            this.SuspendLayout();
            // 
            // webPicture
            // 
            this.webPicture.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webPicture.Location = new System.Drawing.Point(0, 0);
            this.webPicture.Name = "webPicture";
            this.webPicture.Size = new System.Drawing.Size(396, 279);
            this.webPicture.TabIndex = 0;
            this.webPicture.TabStop = false;
            // 
            // NotificationsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(396, 279);
            this.Controls.Add(this.webPicture);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "NotificationsForm";
            this.Text = "G+ Notifier";
            ((System.ComponentModel.ISupportInitialize)(this.webPicture)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox webPicture;
    }
}