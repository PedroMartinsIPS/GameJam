using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float baseSpeed = 6f;
    public float baseJump = 10f;

    [Header("Treasure")]
    public int treasureCount = 0;
    public UIManager uiManager;

    [Header("Ground Check")]
    public Transform groundCheck;
    public LayerMask groundLayer;
    public float groundCheckRadius = 0.2f;

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
        isGrounded = Physics2D.OverlapCircle(
            groundCheck.position,
            groundCheckRadius,
            groundLayer
        );

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
        float currentSpeed = baseSpeed - (treasureCount * 0.5f);

        currentSpeed = Mathf.Clamp(
            currentSpeed,
            2f,
            baseSpeed
        );

        rb.linearVelocity = new Vector2(
            moveInput * currentSpeed,
            rb.linearVelocity.y
        );
    }

    void Jump()
    {
        if (!jumpRequested) return;

        float currentJump = baseJump - (treasureCount * 0.5f);

        currentJump = Mathf.Clamp(
            currentJump,
            4f,
            baseJump
        );

        rb.linearVelocity = new Vector2(
            rb.linearVelocity.x,
            0f
        );

        rb.AddForce(
            Vector2.up * currentJump,
            ForceMode2D.Impulse
        );

        jumpRequested = false;
    }

    public void AddTreasure()
    {
        treasureCount++;

        if (uiManager != null)
        {
            uiManager.UpdateTreasure(treasureCount);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Spikes"))
        {
            Die();
        }
    }

    void Die()
    {
        GameManager.FinalTreasureCount = treasureCount;

        SceneManager.LoadScene("Lose");
    }

    void OnDrawGizmos()
    {
        if (groundCheck == null) return;

        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(
            groundCheck.position,
            groundCheckRadius
        );
    }



    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("MovingPlatform"))
        {
            transform.SetParent(collision.transform);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("MovingPlatform"))
        {
            if(gameObject.activeInHierarchy)
            {
                transform.SetParent(null);
            }
        }
    }
}