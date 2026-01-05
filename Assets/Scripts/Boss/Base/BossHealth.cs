using UnityEngine;
public enum BossPhase
{
    Phase1,
    Phase2
}

public abstract class BossHealth : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] protected float maxHealth;
    [SerializeField] protected float currentHealth;
    public float CurrentHealth => currentHealth;
    public float HealthPercent => currentHealth / maxHealth;

    protected bool isDead;
    public BossPhase phase = BossPhase.Phase1;

    protected virtual void Awake()
    {
        maxHealth = 200;
        currentHealth = maxHealth;
        isDead = false;
    }

    protected virtual void TakeDamage(float damage)
    {
        if (isDead) return;

        damage = Mathf.Max(0, damage);
        currentHealth -= damage;

        if(currentHealth <= maxHealth / 2 && phase == BossPhase.Phase1)
        {
            AnimationManager.Instance.PlayHoXamAttack3Anim();
            phase = BossPhase.Phase2;
            Debug.Log($"{gameObject.name} has entered Phase 2!");
        }

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
    }
    public virtual void SubtractArmor(float damage, float armor)
    {
        float reducedDamage = damage * (1 - armor) / 100;
        TakeDamage(reducedDamage);
    }
    protected virtual void OnHurt() { }

    protected virtual void Die()
    {
        isDead = true;
        Debug.Log($"{gameObject.name} defeated");
    }
}
