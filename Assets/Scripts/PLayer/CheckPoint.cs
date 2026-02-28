using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [SerializeField]
    private Sprite activatedSprite;
    private SpriteRenderer spriteRenderer;  
    private bool isActivated = false;

    public int playerMaxHealth;

    

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")&& !isActivated)
        {
            spriteRenderer.sprite = activatedSprite;
            isActivated = true;
            PlayerStatus playerStatus = collision.GetComponent<PlayerStatus>();
            playerStatus.RegisterCheckpoint(this);
            
            playerMaxHealth = collision.GetComponent<PlayerStatus>().maxHealth;
            PlayerController player = collision.GetComponent<PlayerController>();
            if (player != null)
            {
                player.SetRespawnPoint(transform.position);
            }
        }
    }
}
