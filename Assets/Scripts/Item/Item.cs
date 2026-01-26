using DG.Tweening;
using DG.Tweening.Core.Easing;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private string itemName;
    // [SerializeField] private string description;
    [SerializeField] private Sprite icon;
    [SerializeField]private float duration;
    [SerializeField]private float bounceHeight;

    public string ItemName => itemName;
    // public string Description => description;
    public Sprite Icon => icon;

    void Start()
    {
        Bounce();
    }

    void Bounce()
{
    // Di chuyển từ vị trí hiện tại lên bounceHeight
    transform.DOLocalMoveY(transform.localPosition.y + bounceHeight, duration)
        .SetEase(Ease.InOutSine) // Chậm ở hai đầu, nhanh ở giữa
        .SetLoops(-1, LoopType.Yoyo); // -1 là vô hạn, Yoyo là đi lên rồi đi xuống
}
}
