using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Settings")]
    [SerializeField] private float speed = 7f;
    [SerializeField] private float jumpForce = 12f;

    private Rigidbody2D rb;
    private Animator animator;
    private bool isFacingRight = true;
    private float movement;
    public bool isGrounded;

    [Header("Better Jump physic")]
    [SerializeField] private float fallMultiplier = 3f;
    [SerializeField] private float lowJumpMultiplier = 2f;

    [Header("Coyote time")]
    [SerializeField] private float coyoteTime = 0.15f;
    private float coyoteCounter;

    [Header("Jump Buffering")]
    [SerializeField] private float jumpBufferTime = 0.15f;
    private float jumpBufferCounter;
    private int maxHealth;

    void Start()
    {
        maxHealth = 3;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        movement = Input.GetAxisRaw("Horizontal");

        HandleMovement();
        HandleCoyoteTime();
        HandleJumpBuffer();
        HandleJump();
        HandleBetterJump();

        // Animator parameters
        animator.SetBool("isGrounded", isGrounded);
        animator.SetFloat("run", Mathf.Abs(movement));
        animator.SetFloat("verticalVelocity", rb.linearVelocityY);

        // falling logic
        animator.SetBool("isFalling", rb.linearVelocityY < -0.1f);
    }



    private void HandleMovement()
    {
        rb.linearVelocityX = movement * speed;

        if (movement < 0 && isFacingRight) Flip();
        else if (movement > 0 && !isFacingRight) Flip();
    }



    private void HandleCoyoteTime()
    {
        if (isGrounded)
            coyoteCounter = coyoteTime;
        else
            coyoteCounter -= Time.deltaTime;
    }

    private void HandleJumpBuffer()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            jumpBufferCounter = jumpBufferTime;
        else
            jumpBufferCounter -= Time.deltaTime;
    }


    private void HandleJump()
    {
        if (jumpBufferCounter > 0 && coyoteCounter > 0)
        {
            rb.linearVelocityY = jumpForce;
            animator.SetTrigger("jump");

            coyoteCounter = 0;
            jumpBufferCounter = 0;
        }
    }


    private void HandleBetterJump()
    {
        if (rb.linearVelocityY < 0)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rb.linearVelocityY > 0 && !Input.GetKey(KeyCode.Space))
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }



    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }


    public bool takeDamage(int damage)
    {
        if (maxHealth == 0) { return true; }
        else
        {
            maxHealth -= damage;
            return false;
        }


    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            if (!isGrounded) animator.SetTrigger("land");

            isGrounded = true;
            animator.SetBool("isGrounded", true);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
            animator.SetBool("isGrounded", false);
        }
    }
}
