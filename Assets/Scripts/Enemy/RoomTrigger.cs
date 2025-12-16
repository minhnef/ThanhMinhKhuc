using System;
using System.Collections.Generic;
using UnityEngine;

public class RoomTrigger : MonoBehaviour
{
    
    [SerializeField]
    private List<Enemy> enemies = new List<Enemy>();
    [SerializeField]
    private bool isActive = false;
    void Awake()
    {
        enemies.Clear();
        enemies.AddRange(GetComponentsInChildren<Enemy>());
        
        foreach (Enemy enemy in enemies)
        {
            enemy.gameObject.SetActive(false);
            
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(isActive==true)return;
        if (collision.CompareTag("Player"))
        {
            ActiveRoom();
        }
    }

    private void ActiveRoom()
    {
        isActive=true;
        foreach (Enemy enemy in enemies)
        {
            enemy.gameObject.SetActive(true);
        }
        Debug.Log("Room Activated with " + enemies.Count + " enemies.");
    }
}
