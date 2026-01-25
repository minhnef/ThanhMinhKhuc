using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class LeQuy : BossBase
{
    [SerializeField] private PlayerStatus playerStatus;
    
    // Sử dụng List cố định để tránh tạo rác (Garbage Collection)
    private ContactFilter2D contactFilter;
    private readonly List<Collider2D> hitResults = new List<Collider2D>();

    protected override void Start()
    {
        base.Start();
        // Cấu hình filter một lần duy nhất tại Start
        contactFilter = new ContactFilter2D();
        contactFilter.SetLayerMask(playerMask);
        contactFilter.useTriggers = true;
        contactFilter.useLayerMask = true;
    }

    protected override void Attack()
    {
        Debug.Log("LeQuy Attack Called");
        // Kiểm tra khoảng cách để quyết định chọn ngẫu nhiên hay chọn theo tầm đánh
        float dist = Vector2.Distance(transform.position, playerTransform.position);
        isAttacking = true; 

        // 1. Chọn loại đòn đánh (Bạn có thể dùng Random hoặc logic dist tùy ý)
        int attackType = Random.Range(1, 4); 

        // 2. Chạy Animation
        animator.SetInteger("AttackType", attackType);
        animator.SetTrigger("Attack");

        // 3. Xử lý gây sát thương bằng Invoke dựa trên thời điểm vung chiêu
        // Bạn nên khớp các giây này với Animation của Lệ Quỷ
        switch (attackType)
        {
            case 1: Invoke(nameof(HandleAttack1), 0.6f); break;
            case 2: Invoke(nameof(HandleAttack2), 0.4f); break;
            case 3: Invoke(nameof(HandleAttack3), 0.4f); break;
        }

        DOVirtual.DelayedCall(1.5f, () => EndAttack());
        Debug.Log("LeQuy Attack Executed");
    }

    // Các hàm wrapper để Invoke
    private void HandleAttack1() => ExecutePolygonDamage(attackPoint1, 10);
    private void HandleAttack2() => ExecutePolygonDamage(attackPoint2, 10);
    private void HandleAttack3() => ExecutePolygonDamage(attackPoint3, 15);

    private void ExecutePolygonDamage(Transform attackPoint, int damage)
    {
        if (attackPoint == null) return;

        PolygonCollider2D poly = attackPoint.GetComponent<PolygonCollider2D>();
        if (poly == null) return;

        // Bật point lên để đảm bảo va chạm chính xác (nếu cần)
        attackPoint.gameObject.SetActive(true);

        // Quét va chạm bằng Polygon
        int count = poly.Overlap(contactFilter, hitResults);

        for (int i = 0; i < count; i++)
        {
            if (hitResults[i].CompareTag("Player"))
            {
                playerStatus.TakeDamage(damage);
                Debug.Log($"Lệ Quỷ đánh trúng Player bằng {attackPoint.name}");
                break; // Chỉ gây dame 1 lần mỗi lần quẹt
            }
        }

        // Tắt point ngay sau khi quét xong để tránh gây dame lặp lại
        attackPoint.gameObject.SetActive(false);
    }

    private void OnDrawGizmosSelected()
    {
        // Vẽ Gizmos cho Polygon Collider để dễ căn chỉnh trong Scene
        DrawPolygonGizmo(attackPoint1, Color.red);
        DrawPolygonGizmo(attackPoint2, Color.green);
        DrawPolygonGizmo(attackPoint3, Color.blue);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }

    private void DrawPolygonGizmo(Transform point, Color col)
    {
        if (point == null) return;
        PolygonCollider2D poly = point.GetComponent<PolygonCollider2D>();
        if (poly == null) return;

        Gizmos.color = col;
        Vector2[] points = poly.points;
        for (int i = 0; i < points.Length; i++)
        {
            Vector3 p1 = point.TransformPoint(points[i]);
            Vector3 p2 = point.TransformPoint(points[(i + 1) % points.Length]);
            Gizmos.DrawLine(p1, p2);
        }
    }
}
