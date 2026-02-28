using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    public static ItemPickUp instance;

    [Header("Inventory Data")]
    [SerializeField] private List<Item> items = new List<Item>(); 
    [SerializeField] private List<GameObject> mirrorParts = new List<GameObject>();

    [Header("Current Interaction")]
    public Item currentItem;
    public GameObject currentMirrorPart;

    private void Awake()
    {
        // Hệ thống Singleton chuẩn để giữ dữ liệu qua các Scene
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return; 
        }
    }

    private void Update()
    {

        HandleInteractions();
    }

    private void HandleInteractions()
    {
        if (!Input.GetKeyDown(KeyCode.E)) return;

        // 1. Xử lý nhặt Item thường
        if (currentItem != null)
        {
            items.Add(currentItem);

            GameObject target = currentItem.gameObject;
            currentItem = null; 
            target.SetActive(false);
        }

        // 2. Xử lý nhặt Mảnh gương (có hiệu ứng Fade)
        if (currentMirrorPart != null)
        {
            GameObject partToFade = currentMirrorPart;
            mirrorParts.Add(partToFade);
            currentMirrorPart = null; 

            StartCoroutine(IEFadeMirror(partToFade));
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Item"))
        {
            currentItem = collision.GetComponent<Item>();
            Debug.Log("Đang đứng gần Item: " + currentItem.ItemName);
        }
        else if (collision.CompareTag("MirrorPart"))
        {
            currentMirrorPart = collision.gameObject;
            Debug.Log("Đang đứng gần Mảnh gương");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Chỉ xóa currentItem nếu chính cái đó đi ra khỏi vùng Trigger
        if (collision.CompareTag("Item") && currentItem != null && collision.gameObject == currentItem.gameObject)
        {
            currentItem = null;
        }
        else if (collision.CompareTag("MirrorPart") && currentMirrorPart != null && collision.gameObject == currentMirrorPart)
        {
            currentMirrorPart = null;
        }
    }

    private IEnumerator IEFadeMirror(GameObject target)
    {
        if (target == null) yield break;

        // Lấy SpriteRenderer để làm mờ
        if (target.TryGetComponent<SpriteRenderer>(out SpriteRenderer sr))
        {
            sr.DOFade(0f, 1.5f);
        }

        yield return new WaitForSeconds(1.5f);

        if (target != null)
        {
            target.SetActive(false);
            Debug.Log("<color=blue>Mảnh gương đã biến mất hoàn toàn</color>");
        }
    }

    public List<Item> GetItems() => items;
}