using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    public static AnimationManager instance;
    public Animator playerAnimator;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    #region Player Anim

    public void PlayerTeleAnim()
    {
        playerAnimator.SetTrigger("isTele");
    }
    public void PlayerHurtAnim()
    {
        playerAnimator.SetTrigger("hitted");
    }
    public void PlayDashAnim()
    {
        playerAnimator.SetTrigger("Dash");
    }
    #endregion
}
