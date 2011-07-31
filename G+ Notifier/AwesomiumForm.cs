using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using AwesomiumSharp;

namespace DanTup.GPlusNotifier
{
	public class AwesomiumForm : Form
	{
		protected WebView webView;
		protected Bitmap frameBuffer;
		protected bool needsResize;
		protected PictureBox browserPicture;

		protected void SetupBrowser()
		{
			// Create the WebView used for the user to login.
			webView = WebCore.CreateWebView(browserPicture.Width, browserPicture.Height);

			// Set up any event handlers we need to forward on to Awesomium.
			Resize += WebForm_Resize;
			browserPicture.MouseMove += WebForm_MouseMove;
			browserPicture.MouseDown += WebForm_MouseDown;
			browserPicture.MouseUp += WebForm_MouseUp;
			this.MouseWheel += WebForm_MouseWheel;
			this.KeyDown += WebForm_KeyDown;
			this.KeyUp += WebForm_KeyUp;
			this.KeyPress += WebForm_KeyPress;
			FormClosed += WebForm_FormClosed;
			Activated += WebForm_Activated;
			Deactivate += WebForm_Deactivate;
			webView.IsDirtyChanged += OnIsDirtyChanged;
			webView.CursorChanged += OnCursorChanged;

			needsResize = true;

			browserPicture.Select();
			browserPicture.Focus();
		}

		#region Events to pass-through to browser

		protected void WebForm_Activated(object sender, EventArgs e)
		{
			if (!webView.IsDisposed)
				webView.Focus();
		}

		protected void WebForm_Deactivate(object sender, EventArgs e)
		{
			if (!webView.IsDisposed)
				webView.Unfocus();
		}

		protected void WebForm_FormClosed(object sender, FormClosedEventArgs e)
		{
			webView.IsDirtyChanged -= OnIsDirtyChanged;
			webView.CursorChanged -= OnCursorChanged;
			webView.Close();
		}

		protected void OnIsDirtyChanged(object sender, EventArgs e)
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

		protected void Render()
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

		protected void WebForm_Resize(object sender, EventArgs e)
		{
			if (browserPicture.Width != 0 && browserPicture.Height != 0)
				needsResize = true;
		}

		protected WebKeyModifiers GetModifiers()
		{
			int modifiers = 0;

			if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
				modifiers |= (int)WebKeyModifiers.ControlKey;

			if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
				modifiers |= (int)WebKeyModifiers.ShiftKey;

			if ((Control.ModifierKeys & Keys.Alt) == Keys.Alt)
				modifiers |= (int)WebKeyModifiers.AltKey;

			return (WebKeyModifiers)modifiers;
		}

		protected void WebForm_KeyPress(object sender, KeyPressEventArgs e)
		{
			WebKeyboardEvent keyEvent = new WebKeyboardEvent { Type = WebKeyType.Char, Text = new ushort[] { e.KeyChar, 0, 0, 0 }, Modifiers = GetModifiers() };

			if (!webView.IsDisposed)
				webView.InjectKeyboardEvent(keyEvent);
		}

		protected void WebForm_KeyDown(object sender, KeyEventArgs e)
		{
			WebKeyboardEvent keyEvent = new WebKeyboardEvent { Type = WebKeyType.KeyDown, VirtualKeyCode = (VirtualKey)e.KeyCode, Modifiers = GetModifiers() };

			if (!webView.IsDisposed)
				webView.InjectKeyboardEvent(keyEvent);
		}

		protected void WebForm_KeyUp(object sender, KeyEventArgs e)
		{
			WebKeyboardEvent keyEvent = new WebKeyboardEvent { Type = WebKeyType.KeyUp, VirtualKeyCode = (VirtualKey)e.KeyCode, Modifiers = GetModifiers() };

			if (!webView.IsDisposed)
				webView.InjectKeyboardEvent(keyEvent);
		}

		protected void WebForm_MouseUp(object sender, MouseEventArgs e)
		{
			if (!webView.IsDisposed)
				webView.InjectMouseUp(MouseButton.Left);
		}

		protected void WebForm_MouseDown(object sender, MouseEventArgs e)
		{
			if (!webView.IsDisposed)
				webView.InjectMouseDown(MouseButton.Left);
		}

		protected void WebForm_MouseMove(object sender, MouseEventArgs e)
		{
			if (!webView.IsDisposed)
				webView.InjectMouseMove(e.X, e.Y);
		}

		protected void WebForm_MouseWheel(object sender, MouseEventArgs e)
		{
			if (!webView.IsDisposed)
				webView.InjectMouseWheel(e.Delta);
		}

		protected void OnCursorChanged(object sender, ChangeCursorEventArgs e)
		{
			Cursor c = CursorConvertion.GetCursor(e.CursorType);
			if (c != null)
			{
				browserPicture.Cursor = c;
			}
		}

		#endregion
	}
}
