using System;
using UnityEngine;

public class HoXamController : BossController
{
    [SerializeField] private float attackRange = 1.5f;
    private HoXamHealth hoXamHealth;
    private HoXamCombat hoXamCombat;
    private HoXamMovement hoXamMovement;
    protected override void Awake()
    {
        base.Awake();
        hoXamHealth = GetComponent<HoXamHealth>();
        hoXamCombat = GetComponent<HoXamCombat>();
        hoXamMovement = GetComponent<HoXamMovement>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public override void Update()
    {
        if (bossHealth.CurrentHealth == 0)
        {
            ChangeState(BossState.Dead); return;
        }
        bossMovement.FaceTarget(playerTransform);
        switch (currentState)
        {
            case BossState.Idle:
                HandleIdleState();
                bossMovement.StopMovement();
                if (IsPlayerInRange())
                {
                    ChangeState(BossState.Attack);
                }
                else
                {
                    ChangeState(BossState.Move);
                }
                break;
            case BossState.Move:
                HandleMoveState();
                bossMovement.MoveTowards();
                if (IsPlayerInRange())
                {
                    ChangeState(BossState.Attack);
                }
                break;
            case BossState.Attack:
                HandleAttackState();
                bossMovement.StopMovement();
                if (!IsPlayerInRange())
                {
                    ChangeState(BossState.Move);
                }
                else
                {
                    bossCombat.PerformAttack();
                }
                break;
            case BossState.Hurt:
                HandleHurtState();
                bossMovement.StopMovement();
                break;
            case BossState.Dead:
                HandleDeadState();
                break;
        }
    }

    private void HandleDeadState()
    {
        AnimationManager.Instance.PlayHoXamDieAnim();
        Debug.Log("Boss is dead.");
        return;
    }

    private void HandleHurtState()
    {
        AnimationManager.Instance.PlayHoXamHurtAnim();
        Debug.Log("Boss is hurt.");
        hoXamHealth.SubtractArmor(playerTransform.GetComponent<PlayerController>().AttackDamage, hoXamHealth.Armor);
    }

    private void HandleAttackState()
    {
        if (IsPlayerInRange())
        {
            if (hoXamHealth.phase == BossPhase.Phase2)
            {
                hoXamCombat.PerformAttackPhaseTwo();
                return;
            }
            hoXamCombat.PerformAttack();
        }
        else
        {
            ChangeState(BossState.Move);
        }
    }

    private void HandleMoveState()
    {
        float dist = Vector2.Distance(transform.position, playerTransform.position);

        if (dist > attackRange)
        {
            bossMovement.MoveTowards();
        }
        else
        {
            bossMovement.StopMovement();
            ChangeState(BossState.Attack);
        }
    }

    private void HandleIdleState()
    {
        ChangeState(BossState.Move);
    }

    private bool IsPlayerInRange()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
        return distanceToPlayer <= attackRange;
    }
}
