using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class BossRoomEntryDoor : MonoBehaviour
{
    [SerializeField] private List<Item> requiredItems;
    [SerializeField] private GameObject boss;

    void Start()
    {
        boss.SetActive(false);
    }
    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")&&Input.GetKeyDown(KeyCode.E))
        {
            List<Item> playerItems = collision.GetComponent<ItemPickUp>()?.GetItems();
            if (playerItems != null && HasRequiredItems(playerItems))
            {
                OpenDoor();
                boss.SetActive(true);
            }
        }
    }
    private bool HasRequiredItems(List<Item> playerItems)
    {
        foreach (Item requiredItem in requiredItems)
        {
            if (!playerItems.Contains(requiredItem))
            {
                NotitManage.Instance.ShowNotification("Còn thiếu vật phẩm cần thiết!");
                return false;
            }
        }
        return true;
    }

    private void OpenDoor()
    {
        transform.parent.DOLocalMoveY(transform.parent.localPosition.y+10, 2);
        DOVirtual.DelayedCall(0.5f,null);
        gameObject.SetActive(false);
    }
}


