using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CatAI : MonoBehaviour
{
    public float walkSpeed = 1f;
    public float runSpeed = 3f;
    public float changeDirectionTime = 5f;
    public float activity = 20f;
    public float actionInterval = 10f; // Интервал между случайными действиями
    private float actionTimer;

    private Animator animator;
    private Rigidbody2D rb;
    private SpriteRenderer sprite;

    private float direction = 1f; // 1 для движения направо, -1 для движения налево    
    private bool isActionActive = false;
    private float speed;

    private bool isDragging = false; // Флаг для отслеживания состояния перетаскивания
    private Vector3 dragOffset;

    public Dictionary<string, float> actionProbabilities = new Dictionary<string, float>
    {
        { "Meow", 0.1f },
        { "Lay", 0.1f },
        { "Itch", 0.1f },
        { "Sleep1", 0.1f },
        { "Sleep2", 0.1f },
        { "Sit", 0.1f },
        { "Lick1", 0.1f },
        { "Lick2", 0.1f },
        { "Stretch", 0.1f }
    };

    void Start()
    {
        sprite = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        actionTimer = actionInterval;        
    }


    private void Update()
    {

        if (isDragging)
        {
            isActionActive = false;
            speed = 0;
            animator.SetFloat("speed", speed);
            animator.SetBool("isActionActive", false);
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(mousePosition.x - dragOffset.x, mousePosition.y - dragOffset.y, transform.position.z);
            return; // Прекращаем выполнение Update, если идет перетаскивание
        }

        actionTimer -= Time.deltaTime;
       
        if (actionTimer <= 0)
        {
            actionTimer = actionInterval;

            if (!isActionActive) // Проверка, что текущее действие не активно
            {
                if (Random.Range(0, 100) < activity) // Проверка активности для выбора действия или движения
                {
                    HandleMovement(); // Выполнение движения
                }
                else
                {
                    StartCoroutine(HandleActions()); // Выполнение действия
                }
            }
        }

        if (!isActionActive) // Если действие не активно
        {
            animator.SetFloat("speed", speed); // Установка скорости в аниматор
            rb.velocity = new Vector2(direction * speed, rb.velocity.y); // Установка скорости движения
        }
        else
        {
            rb.velocity = Vector2.zero; // Остановка движения во время действия
        }

        Debug.Log($"Speed: {speed}, Direction: {direction}");
    }

    private void HandleMovement()
    {

        direction = Random.Range(0, 2) == 0 ? -1 : 1; // Случайное изменение направления
        speed = Random.Range(walkSpeed, runSpeed); // Случайный выбор скорости (ходьба или бег)
        sprite.flipX = direction < 0; // Отражение спрайта в зависимости от направления
        animator.SetFloat("speed", speed);
    }

    IEnumerator HandleActions()
    {
        isActionActive = true;
        speed = 0f;
        animator.SetFloat("speed", speed);

        animator.SetBool("isActionActive", true);


        float randomAction = Random.Range(0f, 1f);
        animator.SetFloat("actionIndex", randomAction);


        float actionDuration = Random.Range(10f, 60f);
        yield return new WaitForSeconds(actionDuration);

        isActionActive = false;
        animator.SetBool("isActionActive", false);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            Debug.Log("wall");
            direction *= -1;
            sprite.flipX = direction < 0;
        }
    }

    void OnMouseDown()
    {
        isDragging = true;
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        dragOffset = mousePosition - transform.position;
    }

    void OnMouseUp()
    {
        isDragging = false;
    }

}
