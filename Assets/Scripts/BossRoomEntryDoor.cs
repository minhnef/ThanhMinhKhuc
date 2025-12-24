using System;
using System.Collections.Generic;
using UnityEngine;

public class BossRoomEntryDoor : MonoBehaviour
{
    [SerializeField] private List<Item> requiredItems;
    private Animator animator;
    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            List<Item> playerItems = collision.GetComponent<ItemPickUp>()?.GetItems();
            if (playerItems != null && HasRequiredItems(playerItems))
            {
                OpenDoor();
            }
        }
    }

    private bool HasRequiredItems(List<Item> playerItems)
    {
        foreach (Item requiredItem in requiredItems)
        {
            if (!playerItems.Contains(requiredItem))
            {
                return false;
            }
        }
        return true;
    }

    private void OpenDoor()
    {
        animator.SetBool("isOpen", true);
    }
}
