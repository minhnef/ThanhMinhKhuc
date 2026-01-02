using DG.Tweening;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    public static AnimationManager Instance;
    [SerializeField] private Animator hoXamAnimator;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #region HoXamAnimStates
    public void PlayHoXamAttack1Anim()
    {
        hoXamAnimator.SetTrigger("Attack1");
    }
    public void PlayHoXamAttack2Anim()
    {
        hoXamAnimator.SetTrigger("Attack2");
    }
    public void PlayHoXamAttack3Anim()
    {
        hoXamAnimator.SetTrigger("Attack3");
    }
    public void PlayHoXamHurtAnim()
    {
        hoXamAnimator.SetTrigger("Hurt");
    }
    public void PlayHoXamDieAnim()
    {
        hoXamAnimator.SetTrigger("Die");
        DOVirtual.DelayedCall(1f, () =>
        {
            hoXamAnimator.SetTrigger("Fade");
            hoXamAnimator.gameObject.SetActive(false);
        });
    }
    #endregion




}
