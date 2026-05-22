using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float baseSpeed = 6f;
    public float baseJump = 10f;

    public int treasureCount = 0;

    public UIManager uiManager; 

    private Rigidbody2D rb;
    private bool isGrounded;
    
    private float moveInput;
    private bool jumpRequested;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        moveInput = Input.GetAxis("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            jumpRequested = true;
        }
    }

    void FixedUpdate()
    {
        Move();
        Jump();
    }

    void Move()
    {
        float currentSpeed = baseSpeed - (treasureCount * 0.3f);
        currentSpeed = Mathf.Clamp(currentSpeed, 2f, baseSpeed);

        rb.linearVelocity = new Vector2(moveInput * currentSpeed, rb.linearVelocity.y);
    }

    void Jump()
    {
        if (jumpRequested)
        {
            float currentJump = baseJump - (treasureCount * 0.4f);
            currentJump = Mathf.Clamp(currentJump, 4f, baseJump);

            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);

            rb.AddForce(Vector2.up * currentJump, ForceMode2D.Impulse);
            
            jumpRequested = false; 
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    public void AddTreasure()
    {
        treasureCount++;
        
        if (uiManager != null)
        {
            uiManager.UpdateTreasure(treasureCount);
        }
    }  
}