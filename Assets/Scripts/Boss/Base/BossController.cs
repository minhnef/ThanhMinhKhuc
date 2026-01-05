using UnityEngine;

public enum BossState
{
    Idle,
    Move,
    Attack,
    Hurt,
    Dead
}

//<summary>
// The BossController class manages the overall behavior and state of the boss character.
public class BossController : MonoBehaviour
{
    public BossState currentState;
    protected BossHealth bossHealth;
    protected BossMovement bossMovement;
    protected BossCombat bossCombat;
    protected Transform playerTransform;
    protected virtual void Awake()
    {
        bossHealth = GetComponent<BossHealth>();
        bossMovement = GetComponent<BossMovement>();
        bossCombat = GetComponent<BossCombat>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    protected virtual void Start()
    {
        currentState = BossState.Idle;
        ChangeState(currentState);
    }

    public virtual void ChangeState(BossState newState)
    {
        currentState = newState;
        // Additional logic for entering the new state can be added here.
    }
    public virtual void Update()
    {
        // State machine logic can be implemented here.
        switch (currentState)
        {
            case BossState.Idle:
                // Idle behavior
                break;
            case BossState.Move:
                // Moving behavior
                break;
            case BossState.Attack:
                // Attacking behavior
                break;
            case BossState.Hurt:
                // Hurt behavior
                break;
            case BossState.Dead:
                // Dead behavior
                AnimationManager.Instance.PlayHoXamDieAnim();
                Debug.Log("Boss is dead.");
                break;
        }
    }
    

    public virtual void OnDeath()
    {
        ChangeState(BossState.Dead);
        // Additional death logic
    }

}
