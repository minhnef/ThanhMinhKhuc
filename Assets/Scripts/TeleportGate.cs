using UnityEngine;

public class TeleportGate : MonoBehaviour
{
    [SerializeField] private Transform destination;
    private bool isActive;

    void Awake()
    {
        isActive = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {

            SetActivateGate(true);
            if (isActive && Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("Teleporting Player...");
                Teleport(collision.gameObject);
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
    }
}
