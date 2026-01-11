using System;
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
    [SerializeField] protected float attackRange1 = 0.5f;
    [SerializeField] protected float attackRange2 = 0.3f;
    [SerializeField] protected float attackRange3 = 2f;
    [SerializeField] protected float moveSpeed;
    // [SerializeField] protected float attackDamage;
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
    protected bool isAttacking;
    protected bool isDead;

    public RoomTrigger bossRoomTrigger;

    protected virtual void Start()
    {
        currentHealth = maxHealth;
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        if (playerTransform == null) playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        if (animator == null) animator = GetComponent<Animator>();
    }

    protected virtual void Update()
{
    if (isDead || playerTransform == null) return;

    float distance = Vector2.Distance(transform.position, playerTransform.position);

    // if (distance > detectionRange)
    // {
    //     Idle();
    //     return;
    // }

    if (distance > attackRange3)
    {
        MoveTowardsPlayer();
    }
    else
    {
        if (!isAttacking && Time.time - lastAttackTime >= attackCooldown)
        {
            Attack();
        }
    }
}


    private void Idle()
    {
        // animator.SetBool("isMoving", false);
    }

    private void MoveTowardsPlayer()
    {
        // animator.SetBool("isMoving", true);
        Vector2 direction = (playerTransform.position - transform.position).normalized;

        rb.linearVelocity = new Vector2(direction.x * moveSpeed, rb.linearVelocity.y);
        Flip();
    }

    protected abstract void Attack();

    public virtual void EndAttack()
    {
        isAttacking = false;
        lastAttackTime = Time.time;
    }
    protected void Flip()
    {
        if (playerTransform.position.x > transform.position.x)
            transform.eulerAngles = new Vector3(0, 180, 0);
        else
            transform.eulerAngles = Vector3.zero;
    }
    public virtual void TakeDamage(float damage)
    {
        if (isDead) return;
        float effectiveDamage = Mathf.Max(damage - armor, 1);
        currentHealth -= effectiveDamage;

        animator.SetTrigger("Hurt");

        if (currentHealth <= 0) Die();
    }

    private async void Die()
    {
        Debug.Log("Boss Died");

        isDead = true;
        
        Time.timeScale = 0.5f;
        DOVirtual.DelayedCall(0.5f, () => animator.SetTrigger("Die"));
        await DOVirtual.DelayedCall(1.5f, () => gameObject.SetActive(false)).AsyncWaitForCompletion();
        Time.timeScale = 1f;
        rb.linearVelocity = Vector2.zero; // Dừng mọi chuyển động khi chết
        this.enabled = false; // Tắt script Boss
        bossRoomTrigger.CheckRoomCleared();
    }
    
}
