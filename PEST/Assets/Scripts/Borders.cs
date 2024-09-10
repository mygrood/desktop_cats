using UnityEngine;

public class Borders : MonoBehaviour
{
    public GameObject ground; // ���
    public GameObject leftWall; // ����� �����
    public GameObject rightWall; // ������ �����

    private void Start()
    {
        GroundAndWallsPosition();
    }

    private void GroundAndWallsPosition()
    {
        // �������� ������� ������� � ������� ������
        var workingArea = ScreenUtils.GetWorkingArea();
        var screenSize = ScreenUtils.GetScreenSize();

        // ��������� ������� �������� ������������ � ������� Unity
        float workingWidth = workingArea.right - workingArea.left;
        float workingHeight = workingArea.bottom - workingArea.top;
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        // ���������� ������� � ������� ����
        float groundWidth = workingWidth;
        float groundHeight = workingHeight;
        float groundPositionY = -screenHeight / 2 + groundHeight / 2;

        // ������� � ������ ����
        ground.transform.position = new Vector3(0, groundPositionY + groundHeight / 2, 0);
        ground.transform.localScale = new Vector3(groundWidth, groundHeight, 1);

        // ������������� ������� ����
        float wallThickness = 1.0f; // ������� ����
        float screenHalfWidth = screenWidth / 2;

        // ����� �����
        if (workingArea.left > 0) // ������ ����� �����
        {
            leftWall.transform.position = new Vector3(-screenHalfWidth + workingArea.left / 2, 0, 0);
            leftWall.transform.localScale = new Vector3(workingArea.left, screenHeight, 1);
        }
        else // ����� ����� �� ���������
        {
            leftWall.transform.position = new Vector3(-screenHalfWidth - wallThickness / 2, 0, 0);
            leftWall.transform.localScale = new Vector3(wallThickness, screenHeight, 1);
        }

        // ������ �����
        if (workingArea.right < screenWidth) // ������ ����� ������
        {
            rightWall.transform.position = new Vector3(screenHalfWidth - (screenWidth - workingArea.right) / 2, 0, 0);
            rightWall.transform.localScale = new Vector3(screenWidth - workingArea.right, screenHeight, 1);
        }
        else // ����� ������ �� ���������
        {
            rightWall.transform.position = new Vector3(screenHalfWidth + wallThickness / 2, 0, 0);
            rightWall.transform.localScale = new Vector3(wallThickness, screenHeight, 1);
        }
    }
}
