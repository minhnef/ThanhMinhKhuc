using System;
using System.Collections;
using Unity.Cinemachine;
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
    public CinemachineImpulseSource impulseSource;
    [Header("Player State")]

    private PlayerStatus playerStatus;

    [Header("Better Jump physic")]
    [SerializeField] private float fallMultiplier = 3f;
    [SerializeField] private float lowJumpMultiplier = 2f;

    [Header("Coyote time")]
    [SerializeField] private float coyoteTime = 0.15f;
    private float coyoteCounter;

    [Header("Jump Buffering")]
    [SerializeField] private float jumpBufferTime = 0.15f;
    private float jumpBufferCounter;
    [Header("Health & Fighting")]
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRange = 1f;

    [Header("Combo Attack")]
    [SerializeField] private float comboResetTime = 0.4f;

    private int attackIndex = 0;
    private int damage = 1;
    public int AttackDamage => damage;
    private float comboTimer;
    private Vector3 respawnPoint;

    [Header("Dash Settings")]
    private bool isDashing;
    [SerializeField] private float dashSpeed = 20f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldown = 1f;
    private bool canDash = true;
    [Header("Invincibility Settings")]
    private int originalLayer;
    public bool isInvincible; // Kiểm tra biến này trong hàm TakeDamage của Player
    private bool isAttacking;
    private bool canCombo;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        animator.speed = 1.3f;
        playerStatus = GetComponent<PlayerStatus>();
    }

    void Update()
    {
        movement = Input.GetAxisRaw("Horizontal");
        StartDash();
        HandleMovement();
        HandleComboAttack();
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
        if (comboTimer > 0)
            comboTimer -= Time.deltaTime;
        else
            attackIndex = 0;
    }



    // private void HandleComboAttack()
    // {
    //     if (Input.GetKeyDown(KeyCode.J))
    //     {
    //         comboTimer = comboResetTime;

    //         attackIndex++;
    //         if (attackIndex > 2)
    //             attackIndex = 1;

    //         animator.SetInteger("AttackIndex", attackIndex);
    //         animator.SetTrigger("Attack");

    //         DoAttackDamage();
    //     }
    // }
    private void HandleComboAttack()
{
    if (Input.GetKeyDown(KeyCode.J))
    {
        // Nếu đang không trong đòn đánh nào, bắt đầu đòn 1
        if (!isAttacking)
        {
            StartAttack();
        }
        // Nếu đang trong đòn đánh và được phép nối combo
        else if (canCombo)
        {
            StartAttack();
        }
    }
}

private void StartAttack()
{
    isAttacking = true;
    canCombo = false; // Reset lại quyền nối combo cho đòn mới
    
    comboTimer = comboResetTime;
    attackIndex++;
    
    if (attackIndex > 3) attackIndex = 1;

    animator.SetInteger("AttackIndex", attackIndex);
    animator.SetTrigger("Attack");

    DoAttackDamage(); // Gây sát thương
}
    private void DoAttackDamage()
    {
        EnableHitbox();
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(
            attackPoint.position,
            attackRange,
            LayerMask.GetMask("Enemy")
        );
        Collider2D hitBoss = Physics2D.OverlapCircle(
            attackPoint.position,
            attackRange,
            LayerMask.GetMask("Boss")
        );

        foreach (Collider2D enemy in hitEnemies)
        {
            impulseSource.GenerateImpulse(Vector3.right);
            Enemy enemyScript = enemy.GetComponent<Enemy>();
            if (enemyScript != null)
            {
                enemyScript.TakeDamage(damage);
            }
        }
        if (hitBoss != null)
        {
            impulseSource.GenerateImpulse(Vector3.right);
            BossBase bossScript = hitBoss.GetComponent<BossBase>();
            if (bossScript != null)
            {
                bossScript.TakeDamage(damage);
            }
        }
        DisableHitbox();
    }
    public void SetCanCombo()
{
    canCombo = true;
}
public void EndAttack() 
    {
        isAttacking = false;
        canCombo = false;
    }
    private void OnDrawGizmosSelected()
    {
        // Attack
        if (attackPoint != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        }
    }
    private void HandleMovement()
    {
        rb.linearVelocityX = movement * speed;

        if (movement < 0 && isFacingRight) Flip();
        else if (movement > 0 && !isFacingRight) Flip();
    }


    private IEnumerator DashCoroutine()
    {
        canDash = false;
        isDashing = true;
        isInvincible = true; // Bắt đầu bất tử

        // 1. Lưu Layer cũ và chuyển sang Layer xuyên thấu
        originalLayer = gameObject.layer;
        gameObject.layer = LayerMask.NameToLayer("PlayerDash");

        // 2. Logic vật lý (như đã hướng dẫn trước đó)
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.AddForce(new Vector2(isFacingRight ? dashSpeed : -dashSpeed, 0f), ForceMode2D.Impulse);

        AnimationManager.instance?.PlayDashAnim();

        yield return new WaitForSeconds(dashDuration);

        // 3. Trả lại trạng thái bình thường
        rb.gravityScale = originalGravity;
        rb.linearVelocity = Vector2.zero;
        gameObject.layer = originalLayer; // Trả lại Layer gốc

        isDashing = false;
        isInvincible = false; // Kết thúc bất tử

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
    // Gọi hàm này khi nhấn nút Dash
    public void StartDash()
    {
        if (canDash && Input.GetKeyDown(KeyCode.L)) StartCoroutine(DashCoroutine());
    }

    #region Better Jumping
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


    #endregion
    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
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
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("DeadZone"))
        {
            // Reset player position or handle death
            transform.position = new Vector3(0, 0, 0); // Example respawn position
            playerStatus.Die();
        }
    }



    public void EnableHitbox()
    {
        attackPoint.gameObject.SetActive(true);
    }

    public void DisableHitbox()
    {
        attackPoint.gameObject.SetActive(false);
    }

    internal void SetRespawnPoint(Vector3 position)
    {
        respawnPoint = position;
    }
}
