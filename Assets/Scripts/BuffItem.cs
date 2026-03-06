using DG.Tweening;
using UnityEngine;

public class BuffItem : MonoBehaviour
{
    public int healthBuffAmount = 10;
    public int damageBuffAmount;
    public float speedBuffAmount;
    public float jumpBuffAmount;
    void Start()
    {
        Bounce();
    }
    void Bounce()
    {
        // Di chuyển từ vị trí hiện tại lên bounceHeight
        transform.DOLocalMoveY(transform.localPosition.y + 1.5f, 1.5f)
            .SetEase(Ease.InOutSine) // Chậm ở hai đầu, nhanh ở giữa
            .SetLoops(-1, LoopType.Yoyo); // -1 là vô hạn, Yoyo là đi lên rồi đi xuống
    }
}
