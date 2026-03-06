using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Rigidbody2D rb;
    [Header("Movement")]
    [SerializeField] private int direction = 1; // -1: left, 1: right
    [SerializeField] private float patrolSpeed = 3f;
    [SerializeField] private Transform checkPoint;
    [SerializeField] private float groundCheckDistance = 5f;
    [SerializeField] private LayerMask groundMask;
    private bool facingRight = true;

    [Header("Player Chase")]
    [SerializeField] private Transform player;
    [SerializeField] private float chaseRange = 6f;
    [SerializeField] private float chaseStopDistance = 2.5f;
    [SerializeField] private float chaseSpeed = 5f;

    [Header("Combat")]
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRadius = 1f;
    [SerializeField]private int attackDamage = 1;
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private float maxHealth = 5f;

    [Header("Animation")]
    [SerializeField] private Animator animator;

    private bool inRange;

    void Awake()
    {
        
    }
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        player = FindAnyObjectByType<PlayerController>().transform;
    }
    void Update()
    {
        if (maxHealth <= 0)
        {
            Die();
            return;
        }

        inRange = Vector2.Distance(transform.position, player.position) <= chaseRange;

        if (inRange)
            HandleChase();
        else
            HandlePatrol();
    }

    //
    // PATROL
    //
    private void HandlePatrol()
    {
        animator.SetBool("Run", true);
  
        rb.linearVelocity =new Vector2( direction * patrolSpeed, rb.linearVelocity.y);
        // Check ground
        bool groundDetected = Physics2D.Raycast(checkPoint.position, Vector2.down, groundCheckDistance, groundMask);

        if (!groundDetected)
            Flip();
    }

    //
    // Chase and Attack
    //
    private void HandleChase()
    {
        FacePlayer();

        float dist = Vector2.Distance(transform.position, player.position);

        if (dist > chaseStopDistance)
        {
            animator.SetBool("Run", true);
            transform.position = Vector2.MoveTowards(transform.position, player.position, chaseSpeed * Time.deltaTime);
        }
        else
        {
            animator.SetBool("Run", false);
            Attack();
        }
    }

    private void FacePlayer()
    {
        if (player.position.x > transform.position.x && !facingRight)
            Flip();
        else if (player.position.x < transform.position.x && facingRight)
            Flip();
    }

    // 
    // Attack
    //
    private void Attack()
    {
        animator.SetTrigger("Attack");

        
    }
    public void Dodamage()
    {
        Collider2D col = Physics2D.OverlapCircle(attackPoint.position, attackRadius, playerMask);
        if (col != null && col.TryGetComponent<PlayerStatus>(out var playerStatus))
        {
            playerStatus.TakeDamage(attackDamage);
            Debug.Log("Player hit!");
        }
    }

    // 
    // Hurt & Die
    // 
    public void TakeDamage(int damage)
    {
        if (maxHealth <= 0) return;
        HitStop.Instance?.TriggerHitStop(0.06f);
        animator.SetTrigger("hit");
        Knockback(5f, (Vector2.right * -direction).normalized);
        maxHealth -= damage;
        if (maxHealth <= 0)
            Die();
    }

    public void Knockback(float knockbackForce, Vector2 knockbackDirection)
    {
        rb.AddForce(knockbackDirection.normalized * knockbackForce, ForceMode2D.Impulse);
    }
    private async void Die()
    {
        animator.SetTrigger("Died");
        await System.Threading.Tasks.Task.Delay(500); // Chờ animation kết thúc
        gameObject.SetActive(false); // Chờ animation 1 chút
    }

    //
    // UTIL
    // 
    private void Flip()
    {
        facingRight = !facingRight;
        transform.eulerAngles = facingRight ? Vector3.zero : new Vector3(0, 180, 0);
        direction *= -1;
    }

    private void OnDrawGizmosSelected()
    {
        // Ground check
        if (checkPoint != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(checkPoint.position, Vector2.down * groundCheckDistance);
        }

        // Chase range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRange);

        // Attack
        if (attackPoint != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
        }
    }
}
