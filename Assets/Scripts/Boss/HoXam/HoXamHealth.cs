using UnityEngine;

public class HoXamHealth : BossHealth
{
    private HoXamController hoXamController;
    private float armor = 5f;
    public float Armor => armor;
    private PlayerController playerController;

    protected override void Awake()
    {
        base.Awake();
        hoXamController = GetComponent<HoXamController>();
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    // protected override void OnHurt()
    // {
    //     Debug.Log("HoXam took damage, current health: " + currentHealth);
    //     // Additional hurt logic specific to HoXam can be added here
    //     AnimationManager.Instance.PlayHoXamHurtAnim();
    //     SubtractArmor(playerController.AttackDamage, armor); // Example usage of SubtractArmor
    //     hoXamController.ChangeState(BossState.Hurt);
    // }

    protected override void Die()
    {
        base.Die();
        Debug.Log("HoXam has been defeated!");
        // Additional death logic specific to HoXam can be added here
        hoXamController.ChangeState(BossState.Dead);
        AnimationManager.Instance.PlayHoXamDieAnim();
    }
}
