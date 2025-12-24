using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private string itemName;
    [SerializeField] private string description;
    [SerializeField] private Sprite icon;
    public string ItemName => itemName;
    public string Description => description;
    public Sprite Icon => icon;
    
}
