using UnityEngine;

public abstract class BossHealth : MonoBehaviour
{
    [SerializeField]
    protected float maxHealth = 100;
    protected float currentHealth;
    public float CurrentHealth => currentHealth;

    protected virtual void Awake()
    {
        currentHealth = maxHealth;
    }
    public virtual void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Debug.Log("Boss defeated.");
            // Additional logic for boss defeat can be added here.
        }
    }
}
