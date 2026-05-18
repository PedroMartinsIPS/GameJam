using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float baseSpeed = 6f;
    public float baseJump = 10f;

    public int treasureCount = 0;

    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Move();
        Jump();
    }

    void Move()
    {
        float move = Input.GetAxis("Horizontal");

        float speed = baseSpeed - (treasureCount * 0.3f);

        speed = Mathf.Clamp(speed, 2f, baseSpeed);

        rb.linearVelocity = new Vector2(move * speed, rb.linearVelocity.y);
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            float jumpForce = baseJump - (treasureCount * 0.4f);

            jumpForce = Mathf.Clamp(jumpForce, 4f, baseJump);

            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    public void AddTreasure()
    {
        treasureCount++;
    }
}