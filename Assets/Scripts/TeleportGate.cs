using DG.Tweening;
using UnityEngine;

public class TeleportGate : MonoBehaviour
{
    [SerializeField] private Transform destination;
    private bool isActive;
    [SerializeField] private Sprite activeSprite;
    [SerializeField] private Sprite inactiveSprite;
    void Awake()
    {
        isActive = false;
        GetComponent<SpriteRenderer>().sprite = inactiveSprite;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerInteract"))
        {

            SetActivateGate(true);
            // if (isActive && Input.GetKeyDown(KeyCode.E))
            // {
            //     Debug.Log("Teleporting Player...");
            //     Teleport(collision.gameObject);
            // }
        }
    }
    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerInteract"))
        {

            // SetActivateGate(true);
            if (isActive && Input.GetKeyDown(KeyCode.E))
            {
                SFXManager.instance?.PlaySFX(SFXType.TELEPORT);
                AnimationManager.instance?.PlayerTeleAnim();
                DOVirtual.DelayedCall(1.0f, () =>
                {
                    Debug.Log("Teleporting Player...");
                    Teleport(collision.gameObject);
                });
            }
        }
    }
    private void Teleport(GameObject player)
    {

        player.transform.parent.position = destination.position;
    }
    public void SetActivateGate(bool active = true)
    {

        isActive = active;
        GetComponent<SpriteRenderer>().sprite = active ? activeSprite : inactiveSprite;
    }
}
