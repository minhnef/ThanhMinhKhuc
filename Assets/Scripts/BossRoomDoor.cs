using UnityEngine;
using UnityEngine.SceneManagement;

public class BossRoomDoor : MonoBehaviour
{
    [SerializeField] private RoomTrigger bossRoomTrigger;




    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (bossRoomTrigger != null && bossRoomTrigger.isCleared)
            {
                Debug.Log("Player entered boss room door and room is cleared. Changing map...");
                ChangeMap();
            }
        }
        Debug.Log("Player entered boss room door but room is not cleared yet.");
    }

    private void ChangeMap()
    {
        SceneManager.LoadScene("Map2");
    }
}
