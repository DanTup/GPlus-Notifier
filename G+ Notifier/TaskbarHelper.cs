using System;
using System.Runtime.InteropServices;

namespace DanTup.GPlusNotifier
{
	// Adapted from http://winsharp93.wordpress.com/2009/06/29/find-out-size-and-position-of-the-taskbar/


	public enum TaskbarPosition
	{
		Unknown = -1,
		Left,
		Top,
		Right,
		Bottom,
	}

	static class TaskbarHelper
	{
		public static TaskbarPosition GetPosition()
		{
			try
			{
				IntPtr taskbarHandle = FindWindow("Shell_TrayWnd", null);

				APPBARDATA data = new APPBARDATA();
				data.cbSize = (uint)Marshal.SizeOf(typeof(APPBARDATA));
				data.hWnd = taskbarHandle;
				IntPtr result = SHAppBarMessage(ABM.GetTaskbarPos, ref data);
				return (TaskbarPosition)data.uEdge;
			}
			catch
			{
				return TaskbarPosition.Bottom;
			}
		}

		[DllImport("shell32.dll")]
		private static extern IntPtr SHAppBarMessage(ABM dwMessage, [In, Out] ref APPBARDATA pData);

		[DllImport("user32.dll", SetLastError = true)]
		public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct APPBARDATA
	{
		public uint cbSize;
		public IntPtr hWnd;
		public uint uCallbackMessage;
		public ABE uEdge;
		public RECT rc;
		public int lParam;
	}

	public enum ABM : uint
	{
		New = 0x00000000,
		Remove = 0x00000001,
		QueryPos = 0x00000002,
		SetPos = 0x00000003,
		GetState = 0x00000004,
		GetTaskbarPos = 0x00000005,
		Activate = 0x00000006,
		GetAutoHideBar = 0x00000007,
		SetAutoHideBar = 0x00000008,
		WindowPosChanged = 0x00000009,
		SetState = 0x0000000A,
	}

	public enum ABE : uint
	{
		Left = 0,
		Top = 1,
		Right = 2,
		Bottom = 3
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct RECT
	{
		public int left;
		public int top;
		public int right;
		public int bottom;
	}
}
