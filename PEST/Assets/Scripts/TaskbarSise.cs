using UnityEngine;
using System.Runtime.InteropServices;

public class TaskbarSize : MonoBehaviour
{
    [DllImport("User32.dll")]
    private static extern bool SystemParametersInfo(uint uAction, uint uParam, out RECT lpvParam, uint fuWinIni);

    private const uint SPI_GETWORKAREA = 48;

    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public int left;
        public int top;
        public int right;
        public int bottom;
    }

    void Start()
    {
        // ��������� ������� ������� (������ � ������� �����)
        RECT workArea;
        SystemParametersInfo(SPI_GETWORKAREA, 0, out workArea, 0);

        // ������� ������
        int screenWidth = Screen.width;
        int screenHeight = Screen.height;

        // ������� ������ �����
        int taskbarHeight = screenHeight - (workArea.bottom - workArea.top);

        // ������ ��������� ������� � ������� �������
        RectTransform rt = GetComponent<RectTransform>();
        if (rt != null)
        {
            rt.sizeDelta = new Vector2(screenWidth, screenHeight - taskbarHeight);
            rt.anchoredPosition = new Vector2(0, -(screenHeight - taskbarHeight) / 2);
        }
    }
}
