using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    [SerializeField] private List<Item> items;

    internal List<Item> GetItems()
    {
        return items;
    }

    private void Awake()
    {
        items.Clear();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Item") && Input.GetKeyDown(KeyCode.E))
        {
            Item item = collision.GetComponent<Item>();
            if (item != null)
            {
                items.Add(item);
                Debug.Log("Picked up item: " + item.name);
                item.gameObject.SetActive(false);
            }
        }
    }
    
}

