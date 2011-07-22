using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using AwesomiumSharp;

namespace DanTup.GPlusNotifier
{
	public partial class NotificationsForm : Form
	{
		private WebView webView;
		private bool webViewKeyboardFocused = false;
		private bool needsResize = false;
		private Bitmap frameBuffer;
		private Timer animateTimer;
		private int startPosX;
		private int startPosY;

		public NotificationsForm()
		{
			InitializeComponent();

			// HACK: Resize to window to by a reasonable height.
			int padding = 50;
			var newSize = new Size(500, (int)((Screen.PrimaryScreen.WorkingArea.Height - (padding * 2)) * 0.6));
			this.Size = newSize;
			webView = WebCore.CreateWebView(webPicture.Width, webPicture.Height);

			Resize += WebForm_Resize;
			webPicture.MouseMove += WebForm_MouseMove;
			webPicture.MouseDown += WebForm_MouseDown;
			webPicture.MouseUp += WebForm_MouseUp;
			this.MouseWheel += WebForm_MouseWheel;
			this.KeyDown += WebForm_KeyDown;
			this.KeyUp += WebForm_KeyUp;
			this.KeyPress += WebForm_KeyPress;
			this.webView.KeyboardFocusChanged += new KeyboardFocusChangedEventHandler(webView_KeyboardFocusChanged);
			FormClosed += WebForm_FormClosed;
			Activated += WebForm_Activated;
			Deactivate += WebForm_Deactivate;

			// Set up binding for new windows (webView.OpenExternalLink doesn't seem to work)
			webView.CreateObject("GNotifier");
			webView.SetObjectCallback("GNotifier", "launchUrl", OnOpenExternalLink);
			webView.DomReady += new EventHandler(WebView_DomReady);

			webView.IsDirtyChanged += OnIsDirtyChanged;
			webView.CursorChanged += OnCursorChanged;
			webView.LoadURL("https://m.google.com/app/plus/#~loop:view=notifications");
			webView.Focus();

			// Flag as needing resize, since we originall created the view elsewhere, without access to the PictureBox.
			needsResize = true;

			webPicture.Select();
			webPicture.Focus();

			TopMost = true;

			// Pop doesn't need to be shown in task bar
			ShowInTaskbar = false;

			// Create and run timer for animation
			animateTimer = new Timer();
			animateTimer.Interval = 15;
			animateTimer.Tick += animateTimer_Tick;
		}

		protected override void OnLoad(EventArgs e)
		{
			// Move window out of screen
			startPosX = Screen.PrimaryScreen.WorkingArea.Width - Width;
			startPosY = Screen.PrimaryScreen.WorkingArea.Height;
			SetDesktopLocation(startPosX, startPosY);
			base.OnLoad(e);
			// Begin animation
			animateTimer.Start();
		}

		void animateTimer_Tick(object sender, EventArgs e)
		{
			// Lift window by 5 pixels
			startPosY -= 20;
			// If window is fully visible stop the timer
			if (startPosY < Screen.PrimaryScreen.WorkingArea.Height - this.Height)
			{
				// Set the fial position
				SetDesktopLocation(startPosX, Screen.PrimaryScreen.WorkingArea.Height - this.Height);
				animateTimer.Stop();
				this.BringToFront();
				this.Activate();
			}
			else
				SetDesktopLocation(startPosX, startPosY);
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

		// Attempt to bind the notification function once the DOM has finished initializing
		void WebView_DomReady(object sender, EventArgs e)
		{
			BindGplusOpenWindow();
		}

		private void BindGplusOpenWindow()
		{
			// We hijack the _window function that launches new windows
			if (!webView.IsDisposed)
				webView.ExecuteJavascript("_window = function(a) { GNotifier.launchUrl(a) }");
		}

		void OnOpenExternalLink(object sender, JSCallbackEventArgs e)
		{
			if (e.Arguments.Length == 1)
				Process.Start(e.Arguments[0].ToString());
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

			this.Close();
		}
		
		void WebForm_FormClosed(object sender, FormClosedEventArgs e)
		{
			webView.IsDirtyChanged -= OnIsDirtyChanged;
			webView.CursorChanged -= OnCursorChanged;
			webView.Close();
			this.Dispose();
		}

		private void OnIsDirtyChanged(object sender, EventArgs e)
		{
			if (needsResize && !webView.IsDisposed)
			{
				if (!webView.IsResizing)
				{
					webView.Resize(webPicture.Width, webPicture.Height);
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
				UInt32 lEnd = (UInt32)webPicture.Height * (UInt32)(webPicture.Width / 8);

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

			webPicture.Image = frameBuffer;
		}

		void WebForm_Resize(object sender, EventArgs e)
		{
			if (webPicture.Width != 0 && webPicture.Height != 0)
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

		void OnCursorChanged(object sender, ChangeCursorEventArgs e)
		{
			Cursor c = CursorConvertion.GetCursor(e.CursorType);
			if (c != null)
			{
				webPicture.Cursor = c;
			}
		}

		#endregion
	}
}
