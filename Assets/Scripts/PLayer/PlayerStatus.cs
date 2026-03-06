using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatus : MonoBehaviour
{
    public static PlayerStatus Instance { get; private set; }
    public int maxHealth = 30;
    public int currentHealth;
    public PlayerController playerController;
    [SerializeField]
    private RoomTrigger[] roomTriggers;
    [SerializeField]
    private List<CheckPoint> checkpoints = new List<CheckPoint>();
    private Vector3 startPos;

    [Header("Health_UI")]
    public Slider healthSlider;
    public TextMeshProUGUI healthTxt;

    void Awake()
    {
        Instance = this;
    }
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
        UpdateHealthUI();
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
        if (playerController.isInvincible) return;
        AnimationManager.instance?.PlayerHurtAnim();
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
        playerController.EndAttack();
        UpdateHealthUI();
    }

    public void UpdateHealthUI()
    {
        healthSlider.value = currentHealth / (float)maxHealth;
        healthTxt.text = $"{currentHealth} / {maxHealth}";
    }

    public void Die()
    {
        Debug.Log("Player Died");
        RespawnToNearestCheckpoint();
    }

    private void RespawnToNearestCheckpoint()
    {
        CheckPoint nearestCheckpoint = checkpoints.FindLast(cp => cp != null);
        if (nearestCheckpoint != null)
        {
            transform.position = nearestCheckpoint.transform.position;
            Respawn();
        }
        else
        {
            // If no checkpoint is found, respawn at the origin or a default position
            transform.position = startPos;
            Respawn();
        }
    }
}
