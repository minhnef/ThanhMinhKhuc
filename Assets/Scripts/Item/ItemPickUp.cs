using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    public static ItemPickUp instance;
    [SerializeField] private List<Item> items = new List<Item>(); // Khởi tạo list tránh null
    [SerializeField] private List<GameObject> mirrorParts = new List<GameObject>();
    
    public Item currentItem;
    public GameObject currentMirrorPart;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return; // Dừng thực thi logic phía dưới
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Item"))
        {
            currentItem = collision.GetComponent<Item>();
        }
        else if (collision.CompareTag("MirrorPart"))
        {
            currentMirrorPart = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Kiểm tra xem object đi ra có đúng là object đang lưu không
        if (collision.CompareTag("Item") && currentItem != null && collision.gameObject == currentItem.gameObject)
        {
            currentItem = null;
        }
        else if (collision.CompareTag("MirrorPart") && collision.gameObject == currentMirrorPart)
        {
            currentMirrorPart = null;
        }
    }

    private void Update()
    {
        // Sử dụng phím E để nhặt Item
        if (currentItem != null && Input.GetKeyDown(KeyCode.E))
        {
            items.Add(currentItem);
            Debug.Log("Picked up item: " + currentItem.ItemName);
            
            GameObject itemObj = currentItem.gameObject;
            currentItem = null; // Xóa tham chiếu ngay lập tức để tránh nhặt 2 lần
            itemObj.SetActive(false);
        }

        // Sử dụng phím E để nhặt MirrorPart
        if (currentMirrorPart != null && Input.GetKeyDown(KeyCode.E))
        {
            GameObject partToFade = currentMirrorPart;
            mirrorParts.Add(partToFade);
            currentMirrorPart = null; // Khóa ngay lập tức
            
            StartCoroutine(IEFadeMirror(partToFade));
        }
    }

    private IEnumerator IEFadeMirror(GameObject target)
    {
        if (target == null) yield break;

        
            target.GetComponent<SpriteRenderer>().DOFade(0f, 1.5f); 
        

        yield return new WaitForSeconds(1.5f);

        if (target != null)
        {
            target.SetActive(false);
        }
    }

    // Hàm public để các Script khác lấy danh sách item (ví dụ UI Inventory)
    public List<Item> GetItems() => items;
}