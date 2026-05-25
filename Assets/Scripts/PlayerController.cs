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

    [Header("Audio")]
    public AudioClip coinSound; 

    [Header("Ground Check")]
    public Transform groundCheck;
    public LayerMask groundLayer;
    public float groundCheckRadius = 0.2f;
    

    private Rigidbody2D rb;
    private Animator anim; 

    private bool isGrounded;
    private float moveInput;
    private bool jumpRequested;
    private bool isFacingRight = true; 

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>(); 
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

        if (moveInput > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (moveInput < 0 && isFacingRight)
        {
            Flip();
        }
        anim.SetFloat("speed", Mathf.Abs(rb.linearVelocity.x));
        
        anim.SetBool("isGrounded", isGrounded);
    }

    void FixedUpdate()
    {
        Move();
        Jump();
    }

    void Move()
    {
        float currentSpeed = baseSpeed - (treasureCount * 0.5f);
        currentSpeed = Mathf.Clamp(currentSpeed, 2f, baseSpeed);

        rb.linearVelocity = new Vector2(moveInput * currentSpeed, rb.linearVelocity.y);
    }

    void Jump()
    {
        if (!jumpRequested) return;

        float currentJump = baseJump - (treasureCount * 0.5f);
        currentJump = Mathf.Clamp(currentJump, 4f, baseJump);

        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
        rb.AddForce(Vector2.up * currentJump, ForceMode2D.Impulse);

        jumpRequested = false;
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }

    public void AddTreasure()
    {
        treasureCount++;
        if (uiManager != null)
        {
            uiManager.UpdateTreasure(treasureCount);
        }

        if (coinSound != null)
        {
            AudioSource.PlayClipAtPoint(coinSound, transform.position);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Spikes"))
        {
            Die();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("MovingPlatform"))
        {
            transform.SetParent(collision.transform);
        }

        foreach (ContactPoint2D ponto in collision.contacts)
        {
            if (ponto.normal.y < 0.5f && rb.linearVelocity.y > 0f)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("MovingPlatform"))
        {
            if (gameObject.activeInHierarchy)
            {
                transform.SetParent(null);
            }
        }
    }

    void Die()
    {
        GameManager.FinalTreasureCount = treasureCount;
        MenuManager.currentLevel = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene("Lose");
    }

    void OnDrawGizmos()
    {
        if (groundCheck == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}