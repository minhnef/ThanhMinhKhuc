using System;
using System.Collections.Generic;
using UnityEngine;

public class RoomTrigger : MonoBehaviour
{

    [SerializeField]
    private List<Enemy> enemies = new List<Enemy>();
    [SerializeField] private BossBase boss;
    public bool isActive = false;

    public bool isCleared = false;
    void Awake()
    {
        enemies.Clear();
        enemies.AddRange(GetComponentsInChildren<Enemy>());

        foreach (Enemy enemy in enemies)
        {
            enemy.gameObject.SetActive(false);

        }
    }
    // void FixedUpdate()
    // {
    //     CheckRoomCleared();
    // }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (isActive == true) return;
        if (collision.CompareTag("Player"))
        {
            ActiveRoom();
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        // Optional: Handle logic when the player exits the room
        CheckRoomCleared();
    }
    private void ActiveRoom()
    {
        isActive = true;
        if (boss != null)
        {
            boss.gameObject.SetActive(true);
        }
        if (enemies != null)
            foreach (Enemy enemy in enemies)
            {
                enemy.gameObject.SetActive(true);
            }
        Debug.Log("Room Activated with " + enemies.Count + " enemies.");
    }
    public void CheckRoomCleared()
    {
        foreach (Enemy enemy in enemies)
        {
            if (enemy != null && enemy.gameObject.activeInHierarchy)
            {
                return; // There are still active enemies
            }
        }
        if (boss != null && boss.gameObject.activeInHierarchy)
        {
            return; // Boss is still active
        }
        isCleared = true;
        Debug.Log("Room Cleared!");
    }
}
