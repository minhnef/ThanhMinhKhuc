using System;
using System.Threading;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public abstract class BossBase : MonoBehaviour
{
    [Header("Boss Stats")]
    [SerializeField] private float maxHealth;
    [SerializeField] private float currentHealth;
    [SerializeField] private float armor;

    public float currentHealthValue => currentHealth;

    [SerializeField] protected float detectionRange;
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float attackCooldown;
    private float lastAttackTime;
    [Header("References")]
    [SerializeField] protected Animator animator;
    [SerializeField] protected Transform playerTransform;
    [SerializeField] protected Transform attackPoint1;
    [SerializeField] protected Transform attackPoint2;
    [SerializeField] protected Transform attackPoint3;
    [SerializeField] protected LayerMask playerMask;
    protected Rigidbody2D rb;
    [SerializeField] protected bool isAttacking;
    [SerializeField] private GameObject mirrorPart;
    protected bool isDead;

    public RoomTrigger bossRoomTrigger;
    private CancellationTokenSource cancellationTokenSource;

    protected virtual void Start()
    {
        cancellationTokenSource = new CancellationTokenSource();
        currentHealth = maxHealth;
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        if (playerTransform == null) playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        if (animator == null) animator = GetComponent<Animator>();
        mirrorPart.SetActive(false);
    }

    protected virtual void Update()
    {
        if (isDead || playerTransform == null) return;

        float distance = Vector2.Distance(transform.position, playerTransform.position);

        if (distance > detectionRange)
        {
            MoveTowardsPlayer();
        }
        else
        {
            if (!isAttacking && Time.time - lastAttackTime >= attackCooldown)
            {
                Attack();
                MoveTowardsPlayer();
            }
        }
    }


    private void MoveTowardsPlayer()
    {
        Vector2 direction = (playerTransform.position - transform.position).normalized;

        rb.linearVelocity = new Vector2(direction.x * moveSpeed, rb.linearVelocity.y);
        Flip();
    }
    protected void Flip()
    {
        if (playerTransform.position.x > transform.position.x)
            transform.eulerAngles = new Vector3(0, 180, 0);
        else
            transform.eulerAngles = Vector3.zero;
    }
    protected abstract void Attack();

    public virtual void EndAttack()
    {
        isAttacking = false;
        lastAttackTime = Time.time;
    }

    public virtual void TakeDamage(float damage)
    {
        EndAttack();
        if (isDead) return;
        float effectiveDamage = Mathf.Max(damage - armor, 1);
        currentHealth -= effectiveDamage;

        if (!isAttacking)
            animator.SetTrigger("Hurt");

        if (currentHealth <= 0) Die();

        if (currentHealth == maxHealth / 2)
        {
            //TODO: change phase 2
            attackCooldown = attackCooldown / 2;
            moveSpeed = moveSpeed * 2;
            armor += 5;
        }
    }

    private async void Die()
    {
        Debug.Log("Boss Died");

        isDead = true;

        Time.timeScale = 0.4f;
        await PlayDieAnimation();
        await Task.Delay(500, cancellationTokenSource.Token);
        gameObject.SetActive(false);
        Time.timeScale = 1f;
        rb.linearVelocity = Vector2.zero; 
        enabled = false; 
        bossRoomTrigger.CheckRoomCleared();
        mirrorPart.SetActive(true);
    }
    private async Task PlayDieAnimation()
    {
        animator.SetTrigger("Die");
        await Task.Delay(50);
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        float animationDuration = stateInfo.length / stateInfo.speed;
        await Task.Delay((int)(animationDuration * 1000));
    }

    
    void OnDestroy()
    {
        cancellationTokenSource.Dispose();
        cancellationTokenSource.Cancel();
    }
}
