using UnityEngine;

public class TeleportGate : MonoBehaviour
{
    [SerializeField] private Transform destination;
    private bool isActive;
    [SerializeField] private Transform playerPosition;

    void Awake()
    {
        playerPosition = GameObject.FindGameObjectWithTag("Player").transform;
        isActive = false;
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
