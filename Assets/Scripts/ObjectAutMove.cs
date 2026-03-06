using UnityEngine;
using DG.Tweening;

public class AutoMove : MonoBehaviour
{
    [SerializeField] private bool isVertical = false; // True: Trên -> Dưới, False: Trái -> Phải
    [SerializeField] private float moveDistance = 10f; // Khoảng cách di chuyển
    [SerializeField] private float duration = 2f;      // Thời gian hoàn thành
    [SerializeField] private Ease moveEase = Ease.Linear;

    [SerializeField] private bool loop = true;        // Có lặp lại hay không

    void Start()
    {
        StartMoving();
    }

    private void StartMoving()
    {
        // Reset vị trí về ban đầu trước khi di chuyển (tùy chọn)
        // transform.localPosition = Vector3.zero;

        if (isVertical)
        {
            // Di chuyển từ trên xuống dưới (Trục Y giảm dần)
            transform.DOLocalMoveY(transform.localPosition.y - moveDistance, duration)
                .SetEase(moveEase)
                .SetLoops(loop ? -1 : 0, LoopType.Yoyo);
        }
        else
        {
            // Di chuyển từ trái sang phải (Trục X tăng dần)
            transform.DOLocalMoveX(transform.localPosition.x + moveDistance, duration)
                .SetEase(moveEase)
                .SetLoops(loop ? -1 : 0, LoopType.Yoyo);
        }
    }

    // Hàm để thay đổi hướng di chuyển từ Script khác nếu cần
    public void ChangeDirection(bool vertical)
    {
        isVertical = vertical;
        DOTween.Kill(transform); // Dừng tween hiện tại
        StartMoving();           // Bắt đầu lại với hướng mới
    }
}