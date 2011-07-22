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
		bool webViewKeyboardFocused = false;

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
			this.MouseWheel += WebForm_MouseWheel;
			this.KeyDown += WebForm_KeyDown;
			this.KeyUp += WebForm_KeyUp;
			this.KeyPress += WebForm_KeyPress;
			this.webView.KeyboardFocusChanged += new KeyboardFocusChangedEventHandler(webView_KeyboardFocusChanged);
			FormClosed += WebForm_FormClosed;
			Activated += WebForm_Activated;
			Deactivate += WebForm_Deactivate;

			webView.IsDirtyChanged += OnIsDirtyChanged;
			webView.CursorChanged += OnCursorChanged;
			webView.LoadURL("https://www.google.com/accounts/ServiceLogin?service=webupdates&btmpl=mobile&ltmpl=mobile&continue=http%3a%2f%2fwww.google.com%2fwebhp%3ftab%3dww");
			webView.Focus();

			// Flag as needing resize, since we originall created the view elsewhere, without access to the PictureBox.
			needsResize = true;

			browserPicture.Select();
			browserPicture.Focus();
		}

		void webView_KeyboardFocusChanged(object sender, ChangeKeyboardFocusEventArgs e)
		{
			webViewKeyboardFocused = e.IsFocused;
		}

		private void LoginForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			webView.IsDirtyChanged -= OnIsDirtyChanged;
			webView.CursorChanged -= OnCursorChanged;
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
			webView.CursorChanged -= OnCursorChanged;
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

		WebKeyModifiers GetModifiers()
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

		void WebForm_KeyPress(object sender, KeyPressEventArgs e)
		{
			WebKeyboardEvent keyEvent = new WebKeyboardEvent { Type = WebKeyType.Char, Text = new ushort[] { e.KeyChar, 0, 0, 0 }, Modifiers = GetModifiers() };

			if (!webView.IsDisposed)
				webView.InjectKeyboardEvent(keyEvent);
		}

		void WebForm_KeyDown(object sender, KeyEventArgs e)
		{
			WebKeyboardEvent keyEvent = new WebKeyboardEvent { Type = WebKeyType.KeyDown, VirtualKeyCode = (VirtualKey)e.KeyCode, Modifiers = GetModifiers() };

			if (!webView.IsDisposed)
				webView.InjectKeyboardEvent(keyEvent);
		}

		void WebForm_KeyUp(object sender, KeyEventArgs e)
		{
			WebKeyboardEvent keyEvent = new WebKeyboardEvent { Type = WebKeyType.KeyUp, VirtualKeyCode = (VirtualKey)e.KeyCode, Modifiers = GetModifiers() };

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

		void OnCursorChanged(object sender, ChangeCursorEventArgs e) {
			Cursor c = CursorConvertion.GetCursor(e.CursorType);
			if (c != null) {
				browserPicture.Cursor = c;
			}
		}

		#endregion


	}
}
