using UnityEngine;

public class HoXamCombat : BossCombat
{
    public override void PerformAttack()
    {
        int attackType = Random.Range(0, 2);
        switch (attackType)
        {
            case 0:
                Debug.Log("HoXam performs a melee attack!");
                AnimationManager.Instance.PlayHoXamAttack1Anim();
                CameraShaking.Instance.ShakeCamera(0.5f);
                break;
            case 1:
                Debug.Log("HoXam performs attack 2!");
                AnimationManager.Instance.PlayHoXamAttack2Anim();
                CameraShaking.Instance.ShakeCamera(1.5f);
                break;
        }
    }

    public override void PerformAttackPhaseTwo()
    {
        int attackType = Random.Range(0, 3);
        switch (attackType)
        {
            case 0:
                Debug.Log("HoXam performs a powerful slam attack!");
                AnimationManager.Instance.PlayHoXamAttack1Anim();
                CameraShaking.Instance.ShakeCamera(1f);
                break;
            case 1:
                Debug.Log("HoXam performs a ranged fireball attack!");
                AnimationManager.Instance.PlayHoXamAttack2Anim();
                CameraShaking.Instance.ShakeCamera(3f);
                break;
            case 2:
                Debug.Log("HoXam performs a whirlwind attack!");
                AnimationManager.Instance.PlayHoXamAttack3Anim();
                CameraShaking.Instance.ShakeCamera(5f);
                break;
        }
    }
}
