using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using AwesomiumSharp;

namespace DanTup.GPlusNotifier
{
	public partial class LoginForm : Form
	{
		/// <summary>
		/// The Awesomium WebView rendering our web pages.
		/// </summary>
		private WebView webView;
		Bitmap frameBuffer;
		bool needsResize;

		public LoginForm()
		{
			InitializeComponent();
		}

		public LoginForm(WebView webView)
			: this()
		{
			this.webView = webView;

			Resize += WebForm_Resize;
			browserPicture.MouseMove += WebForm_MouseMove;
			browserPicture.MouseDown += WebForm_MouseDown;
			browserPicture.MouseUp += WebForm_MouseUp;
			browserPicture.MouseWheel += WebForm_MouseWheel;
			browserPicture.KeyDown += WebForm_KeyDown;
			browserPicture.KeyUp += WebForm_KeyUp;
			browserPicture.KeyPress += WebForm_KeyPress;
			FormClosed += WebForm_FormClosed;
			Activated += WebForm_Activated;
			Deactivate += WebForm_Deactivate;

			webView.IsDirtyChanged += OnIsDirtyChanged;
			webView.LoadURL("https://www.google.com/accounts/ServiceLogin?hl=en&continue=http://www.google.com/webhp%3Ftab%3DXw%26authuser%3D0");
			webView.Focus();

			// Flag as needing resize, since we originall created the view elsewhere, without access to the PictureBox.
			needsResize = true;

			browserPicture.Select();
			browserPicture.Focus();
		}

		private void LoginForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			webView.IsDirtyChanged -= OnIsDirtyChanged;
		}

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			// TODO: Need to try and find a better way of doing this (and one that supports all the keys!)
			if (browserPicture.Focused && (
				keyData == Keys.ShiftKey
				|| keyData == Keys.Tab
				|| keyData == Keys.Left
				|| keyData == Keys.Right
				|| keyData == Keys.Enter
			))
			{
				webView.InjectKeyboardEvent(new WebKeyboardEvent { Type = WebKeyType.KeyDown, VirtualKeyCode = (VirtualKey)keyData });
				WebCore.Update();
				webView.InjectKeyboardEvent(new WebKeyboardEvent { Type = WebKeyType.KeyUp, VirtualKeyCode = (VirtualKey)keyData });
				return true;
			}
			else
				return base.ProcessCmdKey(ref msg, keyData);
		}

		#region Events to pass-through to browser

		void WebForm_Activated(object sender, EventArgs e)
		{
			if (!webView.IsDisposed)
				webView.Focus();
		}

		void WebForm_Deactivate(object sender, EventArgs e)
		{
			if (!webView.IsDisposed)
				webView.Unfocus();
		}

		void WebForm_FormClosed(object sender, FormClosedEventArgs e)
		{
			webView.IsDirtyChanged -= OnIsDirtyChanged;
		}

		private void OnIsDirtyChanged(object sender, EventArgs e)
		{
			if (needsResize && !webView.IsDisposed)
			{
				if (!webView.IsResizing)
				{
					webView.Resize(browserPicture.Width, browserPicture.Height);
					needsResize = false;
				}
			}

			if (webView.IsDirty)
				Render();
		}

		void Render()
		{
			if (webView.IsDisposed)
				return;

			RenderBuffer rBuffer = webView.Render();

			if (frameBuffer == null)
			{
				frameBuffer = new Bitmap(rBuffer.Width, rBuffer.Height, PixelFormat.Format32bppArgb);
			}
			else if (frameBuffer.Width != rBuffer.Width || frameBuffer.Height != rBuffer.Height)
			{
				frameBuffer.Dispose();
				frameBuffer = new Bitmap(rBuffer.Width, rBuffer.Height, PixelFormat.Format32bppArgb);
			}

			BitmapData bits = frameBuffer.LockBits(new Rectangle(0, 0, rBuffer.Width, rBuffer.Height),
								ImageLockMode.ReadWrite, frameBuffer.PixelFormat);


			unsafe
			{
				UInt64* ptrBase = (UInt64*)((byte*)bits.Scan0);
				UInt64* datBase = (UInt64*)rBuffer.Buffer;
				UInt32 lOffset = 0;
				UInt32 lEnd = (UInt32)browserPicture.Height * (UInt32)(browserPicture.Width / 8);

				// copy 64 bits at a time, 4 times (since we divided by 8)
				for (lOffset = 0; lOffset < lEnd; lOffset++)
				{
					*ptrBase++ = *datBase++;
					*ptrBase++ = *datBase++;
					*ptrBase++ = *datBase++;
					*ptrBase++ = *datBase++;
				}
			}

			frameBuffer.UnlockBits(bits);

			browserPicture.Image = frameBuffer;
		}

		void WebForm_Resize(object sender, EventArgs e)
		{
			if (browserPicture.Width != 0 && browserPicture.Height != 0)
				needsResize = true;
		}

		void WebForm_KeyPress(object sender, KeyPressEventArgs e)
		{
			WebKeyboardEvent keyEvent = new WebKeyboardEvent { Type = WebKeyType.Char, Text = new ushort[] { e.KeyChar, 0, 0, 0 } };

			if (!webView.IsDisposed)
				webView.InjectKeyboardEvent(keyEvent);
		}

		void WebForm_KeyDown(object sender, KeyEventArgs e)
		{
			WebKeyboardEvent keyEvent = new WebKeyboardEvent { Type = WebKeyType.KeyDown, VirtualKeyCode = (VirtualKey)e.KeyCode };

			if (!webView.IsDisposed)
				webView.InjectKeyboardEvent(keyEvent);
		}

		void WebForm_KeyUp(object sender, KeyEventArgs e)
		{
			WebKeyboardEvent keyEvent = new WebKeyboardEvent { Type = WebKeyType.KeyUp, VirtualKeyCode = (VirtualKey)e.KeyCode };

			if (!webView.IsDisposed)
				webView.InjectKeyboardEvent(keyEvent);
		}

		void WebForm_MouseUp(object sender, MouseEventArgs e)
		{
			if (!webView.IsDisposed)
				webView.InjectMouseUp(MouseButton.Left);
		}

		void WebForm_MouseDown(object sender, MouseEventArgs e)
		{
			if (!webView.IsDisposed)
				webView.InjectMouseDown(MouseButton.Left);
		}

		void WebForm_MouseMove(object sender, MouseEventArgs e)
		{
			if (!webView.IsDisposed)
				webView.InjectMouseMove(e.X, e.Y);
		}

		void WebForm_MouseWheel(object sender, MouseEventArgs e)
		{
			if (!webView.IsDisposed)
				webView.InjectMouseWheel(e.Delta);
		}

		#endregion
	}
}
