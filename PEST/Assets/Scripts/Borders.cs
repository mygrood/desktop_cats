using UnityEngine;

public class Borders : MonoBehaviour
{
    public GameObject ground; // Пол
    public GameObject leftWall; // Левая стена
    public GameObject rightWall; // Правая стена

    private void Start()
    {
        GroundAndWallsPosition();
    }

    private void GroundAndWallsPosition()
    {
        // Получаем рабочую область и размеры экрана
        var workingArea = ScreenUtils.GetWorkingArea();
        var screenSize = ScreenUtils.GetScreenSize();

        // Переводим размеры рабочего пространства в размеры Unity
        float workingWidth = workingArea.right - workingArea.left;
        float workingHeight = workingArea.bottom - workingArea.top;
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        // Определяем размеры и позицию пола
        float groundWidth = workingWidth;
        float groundHeight = workingHeight;
        float groundPositionY = -screenHeight / 2 + groundHeight / 2;

        // Позиция и размер пола
        ground.transform.position = new Vector3(0, groundPositionY + groundHeight / 2, 0);
        ground.transform.localScale = new Vector3(groundWidth, groundHeight, 1);

        // Устанавливаем позиции стен
        float wallThickness = 1.0f; // Толщина стен
        float screenHalfWidth = screenWidth / 2;

        // Левая стена
        if (workingArea.left > 0) // Панель задач слева
        {
            leftWall.transform.position = new Vector3(-screenHalfWidth + workingArea.left / 2, 0, 0);
            leftWall.transform.localScale = new Vector3(workingArea.left, screenHeight, 1);
        }
        else // Стена слева по умолчанию
        {
            leftWall.transform.position = new Vector3(-screenHalfWidth - wallThickness / 2, 0, 0);
            leftWall.transform.localScale = new Vector3(wallThickness, screenHeight, 1);
        }

        // Правая стена
        if (workingArea.right < screenWidth) // Панель задач справа
        {
            rightWall.transform.position = new Vector3(screenHalfWidth - (screenWidth - workingArea.right) / 2, 0, 0);
            rightWall.transform.localScale = new Vector3(screenWidth - workingArea.right, screenHeight, 1);
        }
        else // Стена справа по умолчанию
        {
            rightWall.transform.position = new Vector3(screenHalfWidth + wallThickness / 2, 0, 0);
            rightWall.transform.localScale = new Vector3(wallThickness, screenHeight, 1);
        }
    }
}
