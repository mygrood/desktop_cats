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
    public float actionInterval = 10f; // интервал между случайными действиями
    private float actionTimer=0;

    private Animator animator;
    private Rigidbody2D rb;
    private SpriteRenderer sprite;

    private float direction = 1f; // 1 для движения направо, -1 для движения налево    
    private bool isActionActive = false;
    private float speed;

    private bool isDragging = false; 
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
        //actionTimer = actionInterval;        
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
            return; 
        }

        actionTimer -= Time.deltaTime;
       
        if (actionTimer <= 0)
        {
            actionTimer = actionInterval;

            if (!isActionActive) 
            {
                if (Random.Range(0, 100) < activity) 
                {
                    HandleMovement(); 
                }
                else
                {
                    StartCoroutine(HandleActions()); 
                }
            }
        }

        if (!isActionActive) // Если действие не активно
        {
            animator.SetFloat("speed", speed); 
            rb.velocity = new Vector2(direction * speed, rb.velocity.y); 
        }
        else
        {
            rb.velocity = Vector2.zero; // Остановка движения во время действия
        }

        Debug.Log($"Speed: {speed}, Direction: {direction}");
    }

    private void HandleMovement()
    {

        direction = Random.Range(0, 2) == 0 ? -1 : 1; 
        speed = Random.Range(walkSpeed, runSpeed); 
        sprite.flipX = direction < 0; 
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
