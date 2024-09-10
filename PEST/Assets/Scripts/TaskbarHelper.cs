using System;
using System.Runtime.InteropServices;

public class TaskbarHelper
{
    private const uint ABM_GETTASKBARPOS = 5;

    [DllImport("shell32.dll", CharSet = CharSet.Auto)]
    private static extern IntPtr SHAppBarMessage(uint dwMessage, ref APPBARDATA pData);

    [StructLayout(LayoutKind.Sequential)]
    public struct APPBARDATA
    {
        public uint cbSize;
        public IntPtr hWnd;
        public uint uCallbackMessage;
        public uint uFlags;        
        public IntPtr hIcon;
        public IntPtr hInstance;
        public uint dwStyle;
        public RECT rc;
        public IntPtr lParam;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public int left;
        public int top;
        public int right;
        public int bottom;
    }

    public static RECT GetTaskbarPosition()
    {
        APPBARDATA abd = new APPBARDATA
        {
            cbSize = (uint)Marshal.SizeOf(typeof(APPBARDATA))
        };
        SHAppBarMessage(ABM_GETTASKBARPOS, ref abd);
        return abd.rc;
    }
}
