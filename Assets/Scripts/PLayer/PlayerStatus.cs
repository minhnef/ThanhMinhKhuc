using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    public int maxHealth = 3;
    public int currentHealth;
    private PlayerController playerController;
    [SerializeField]
    private RoomTrigger[] roomTriggers;
    private List<CheckPoint> checkpoints = new List<CheckPoint>();
    private Vector3 startPos;

    void Start()
    {
        checkpoints.Clear();
        currentHealth = maxHealth;
        startPos = transform.position;
        playerController = GetComponent<PlayerController>();
        if (roomTriggers == null || roomTriggers.Length == 0)
        {
            roomTriggers = FindObjectsOfType<RoomTrigger>();
        }
    }

    public void Respawn()
    {
        currentHealth = maxHealth;
        // Additional respawn logic can be added here

        foreach (var room in roomTriggers)
        {
            if (room.isActive == false) continue;
            else
            {
                if (room.isCleared)
                {
                    continue;
                }
                else if (room.isActive && !room.isCleared)
                {
                    foreach (var enemy in room.GetComponentsInChildren<Enemy>())
                    {
                        if (enemy != null && !enemy.gameObject.activeInHierarchy)
                        {
                            enemy.gameObject.SetActive(true);
                        }
                    }
                }
            }
        }


    }

    public void RegisterCheckpoint(CheckPoint checkpoint)
    {
        if (!checkpoints.Contains(checkpoint))
        {
            checkpoints.Add(checkpoint);
        }
    }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        // Handle player death (e.g., respawn, game over, etc.)
        Debug.Log("Player Died");
        RespawnToNearestCheckpoint();
    }

    private void RespawnToNearestCheckpoint()
    {
        CheckPoint nearestCheckpoint = checkpoints.FindLast(cp => cp != null);
        if (nearestCheckpoint != null)
        {
            playerController.transform.position = nearestCheckpoint.transform.position;
            Respawn();
        }
        else
        {
            // If no checkpoint is found, respawn at the origin or a default position
            playerController.transform.position = startPos;
            Respawn();
        }
    }
}
