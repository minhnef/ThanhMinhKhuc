using UnityEngine;

public class BossRoomDoor : MonoBehaviour
{
    [SerializeField] private RoomTrigger bossRoomTrigger;
    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (bossRoomTrigger != null && bossRoomTrigger.isCleared)
            {
                OpenDoor();
            }
        }
    }

    private void OpenDoor()
    {
        animator.SetBool("isOpen", true);
    }
}
