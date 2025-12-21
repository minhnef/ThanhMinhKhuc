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
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private float maxHealth = 5f;

    [Header("Animation")]
    [SerializeField] private Animator animator;

    private bool inRange;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
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

        Collider2D col = Physics2D.OverlapCircle(attackPoint.position, attackRadius, playerMask);
        if (col != null && col.TryGetComponent<PlayerController>(out var playerController))
        {
            playerController.takeDamage(1);
        }
    }

    // 
    // Hurt & Die
    // 
    public void TakeDamage(int damage)
    {
        if (maxHealth <= 0) return;

        maxHealth -= damage;
        if (maxHealth <= 0)
            Die();
    }

    private void Die()
    {
        animator.SetTrigger("Died");
        Destroy(gameObject, 0.6f); // Chờ animation 1 chút
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
// using Unity.VisualScripting;
// using UnityEngine;
// using UnityEngine.UI;

// public class Enemies_Scripts : MonoBehaviour
// {
//     [SerializeField]
//     private float speed = 3f;
//     [SerializeField]
//     private Transform checkPoint;
//     [SerializeField]
//     private float distance = 5f;
//     [SerializeField]
//     private LayerMask GroundLayerMask;
//     [SerializeField]
//     private bool facingLeft;


//     [SerializeField]
//     private Transform playerTransform;
//     private bool inRange = false;
//     private float attackRange = 6f;
//     private float retrieveDistance = 2.5f;
//     private float chaseSpeed = 5f;

//     [SerializeField]
//     private Animator animator;
//     [SerializeField]
//     private Transform attackPoint;
//     [SerializeField]
//     private float attackRadius = 1f;
//     [SerializeField]
//     private LayerMask playerLayerMask;
//     [SerializeField]
//     private float maxHealth = 5f;

  

//     // Update is called once per frame
//     void Update()
//     {
//     //    if(FindFirstObjectByType<GameManger>().isGameAcive == false)
//     //     {
//     //         return;
//     //     }

//         if (maxHealth <= 0f)
//         {
//             animator.SetBool("Died", true);
//             Died();
//         }
        
//         if (Vector2.Distance(transform.position, playerTransform.position) <= attackRange)
//         {
//             inRange = true;
//         }
//         else
//         {
//             inRange = false;
//         }

//         if (inRange == true)
//         {
//             //xu ly khi player trong tam
//             if (playerTransform.position.x > transform.position.x && facingLeft == true)
//             {
//                 transform.eulerAngles = new Vector3(0, 0, 0);
//                 facingLeft = false;
//             }
//             else if (playerTransform.position.x < transform.position.x && facingLeft == false)
//             {
//                 transform.eulerAngles = new Vector3(0, 180, 0);
//                 facingLeft = true;
//             }

//             if (Vector2.Distance(transform.position, playerTransform.position) > retrieveDistance)
//             {
//                 animator.SetBool("Attack1", false);
//                 transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, chaseSpeed * Time.deltaTime);
//             }
//             else
//             {
//                 animator.SetBool("Attack1", true);
//             }

//         }
//         else
//         {
//             //xu ly khi player ngoai tam

//             transform.Translate(Vector2.left * Time.deltaTime * speed);
//             RaycastHit2D hit = Physics2D.Raycast(checkPoint.position, Vector2.down, distance, GroundLayerMask);

//             if (hit == false && facingLeft == true)
//             {
//                 transform.eulerAngles = new Vector3(0f, 0f, 0f);
//                 facingLeft = false;
//             }
//             else if (hit == false && facingLeft == false)
//             {
//                 transform.eulerAngles = new Vector3(0f, 180f, 0f);
//                 facingLeft = true;
//             }
//         }
//     }

//     private void Attack()
//     {
//         Collider2D colliInfo = Physics2D.OverlapCircle(attackPoint.position, attackRadius, playerLayerMask);

//         if(colliInfo)
//         {
//             if (colliInfo.gameObject.GetComponent<PlayerController>() != null)
//             {
                
//                 colliInfo.gameObject.GetComponent<PlayerController>().takeDamage(1);
                
//             }
//         }
        
//     }
//     private void Died()
//     {
//         Debug.Log(this.transform.name + " died");
//         Destroy(this.gameObject);
//     }
//     public bool takeDamage(int damage)
//     {
//         if (maxHealth == 0) { return true; }
//         else
//         {
//             maxHealth -= damage;
//             return false;
//         }


//     }
//     private void OnDrawGizmosSelected()
//     {
//         if (checkPoint == null)
//         {
//             Debug.Log("checkPoint is null");
//             return;
//         }
//         Gizmos.color = Color.yellow;
//         Gizmos.DrawRay(checkPoint.position, Vector2.down * distance);

//         Gizmos.color = Color.red;
//         Gizmos.DrawWireSphere(transform.position, attackRange);

//         if (attackPoint == null) return;
//         Gizmos.color = Color.green;
//         Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
//     }

// }