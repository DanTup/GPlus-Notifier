using System.Drawing;
using System.IO;
using System.Reflection;

namespace DanTup.GPlusNotifier
{
	static class Icons
	{
		// Icons used in the systray.
		static Icon iconLogo, iconNone, iconSome, iconCustom;

		// Used for drawing the number on the icon.
		static Brush brush = new SolidBrush(Color.WhiteSmoke);
		static Font font = new Font("Segoe UI", 10F, FontStyle.Bold);
		static PointF iconCenter = new PointF(8f, 9f); // Offset slightly to make it look better :/

		static Icons()
		{
			// Set up the icons we'll need for the notification area.
			var ass = Assembly.GetExecutingAssembly();
			using (Stream stream = ass.GetManifestResourceStream("DanTup.GPlusNotifier.Icons.Logo.ico"))
			{
				iconLogo = new Icon(stream);
			}
			using (Stream stream = ass.GetManifestResourceStream("DanTup.GPlusNotifier.Icons.None.ico"))
			{
				iconNone = new Icon(stream);
			}
			using (Stream stream = ass.GetManifestResourceStream("DanTup.GPlusNotifier.Icons.Some.ico"))
			{
				iconSome = new Icon(stream);
			}
		}

		public static Icon GetLogo()
		{
			return iconLogo;
		}

		internal static Icon GetIcon(int? notificationCount)
		{
			// Remove any previous icon.
			if (iconCustom != null)
			{
				iconCustom.Dispose();
				iconCustom = null;
			}

			// Create a clone of the icon and add text.
			var baseIcon = notificationCount == 0 ? iconNone : iconSome;

			// Clone the base icon we've picked.
			using (var bmp = baseIcon.ToBitmap())
			using (var img = Graphics.FromImage(bmp))
			{
				// Decide what to display
				var badge = notificationCount > 9 ? "9+" : notificationCount.ToString();

				// Calculate the size of the text so we can center it
				var textSize = img.MeasureString(badge, font);

				// Work out the exact position
				var pos = new PointF(iconCenter.X - textSize.Width / 2, iconCenter.Y - textSize.Height / 2);

				// Draw the text into the icon using the font/brush
				img.DrawString(badge, font, brush, pos);

				// Write into the icon that we'll set on to the NotifyIcon
				iconCustom = Icon.FromHandle(bmp.GetHicon());
			}

			return iconCustom;
		}
	}
}
