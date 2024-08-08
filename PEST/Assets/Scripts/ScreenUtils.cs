using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class ScreenUtils : MonoBehaviour
{
    [DllImport("user32.dll")]
    private static extern IntPtr MonitorFromWindow(IntPtr hwnd, uint dwFlags);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool GetMonitorInfo(IntPtr hMonitor, ref MONITORINFO lpmi);

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct MONITORINFO
    {
        public uint cbSize;
        public RECT rcMonitor;
        public RECT rcWork;
        public uint dwFlags;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public int left;
        public int top;
        public int right;
        public int bottom;
    }

    public static RECT GetWorkingArea()
    {
        IntPtr monitor = MonitorFromWindow(IntPtr.Zero, 2); // MonitorFromWindow
        MONITORINFO monitorInfo = new MONITORINFO();
        monitorInfo.cbSize = (uint)Marshal.SizeOf(monitorInfo);

        if (GetMonitorInfo(monitor, ref monitorInfo))
        {
            return monitorInfo.rcWork;
        }

        return new RECT() { left = 0, top = 0, right = Screen.width, bottom = Screen.height };
    }
     public static Vector2 GetScreenSize()
    {
        RECT workArea = GetWorkingArea();
        return new Vector2(workArea.right - workArea.left, workArea.bottom - workArea.top);
    }

    public static Vector2 ConvertWorldToScreenPoint(Vector3 worldPosition, Camera camera)
    {
        // Преобразование мировых координат в координаты экрана
        Vector3 screenPos = camera.WorldToScreenPoint(worldPosition);
        // Корректировка с учетом рабочего пространства
        Vector2 screenSize = GetScreenSize();
        screenPos.x = Mathf.Clamp(screenPos.x, 0, screenSize.x);
        screenPos.y = Mathf.Clamp(screenPos.y, 0, screenSize.y);
        return new Vector2(screenPos.x, screenPos.y);
    }
}


