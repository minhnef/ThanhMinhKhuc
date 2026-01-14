using DG.Tweening;
using UnityEngine;
using System.Collections.Generic;

public class HoXam : BossBase
{
    [SerializeField] private PlayerStatus playerStatus;
    
    // Tạo bộ lọc và danh sách để tối ưu bộ nhớ, tránh tạo rác (GC)
    private ContactFilter2D playerFilter;
    private readonly List<Collider2D> hitResults = new List<Collider2D>();

    protected override void Start()
    {
        base.Start();
        // Thiết lập bộ lọc chỉ quét Layer Player
        playerFilter.SetLayerMask(playerMask);
        playerFilter.useLayerMask = true;
        playerFilter.useTriggers = true; // Quét cả nếu Player có trigger
    }

    protected override void Attack()
    {
        float dist = Vector2.Distance(transform.position, playerTransform.position);
        isAttacking = true;

        int attackType = 0;
        if (dist <= attackRange1) attackType = 1; // Cào
        else if (dist <= attackRange2) attackType = Random.Range(2,4); // Nhảy lao// Gầm
        // else attackType = 3; // Gầm

        animator.SetInteger("AttackType", attackType);
        animator.SetTrigger("Attack");

        // Gọi logic gây sát thương theo thời điểm Animation vung đòn
        switch (attackType)
        {
            case 1: Invoke(nameof(ScratchDamage), 0.4f); break; 
            case 2: Invoke(nameof(LeapLogic), 0.5f); break; 
            case 3: Invoke(nameof(RoarDamage), 0.6f); break;
        }

        // Lưu ý: Thay vì DelayedCall cố định, hãy dùng Animation Event gọi EndAttack() 
        // ở cuối mỗi Clip sẽ mượt hơn. Tạm thời giữ lại theo ý bạn:
        DOVirtual.DelayedCall(1.5f, () => EndAttack());
    }

    // Hàm dùng chung để quét sát thương bằng Polygon Collider
    private void CheckPolygonDamage(Transform attackPoint, int damage)
    {
        if (attackPoint == null) return;
        
        PolygonCollider2D poly = attackPoint.GetComponent<PolygonCollider2D>();
        if (poly == null)
        {
            Debug.LogWarning($"Thiếu PolygonCollider2D trên {attackPoint.name}");
            return;
        }

        // Quét va chạm theo hình dạng của Polygon
        int hitCount = poly.Overlap(playerFilter, hitResults);

        for (int i = 0; i < hitCount; i++)
        {
            if (hitResults[i].CompareTag("Player"))
            {
                AnimationManager.instance?.PlayerHurtAnim();
                playerStatus.TakeDamage(damage);
                Debug.Log($"Đã trúng Player bằng {attackPoint.name}");
                break; // Chỉ gây dame 1 lần mỗi đòn
            }
        }
    }

    public void LeapLogic()
    {
        float leapForce = 50f;
        Vector2 direction = (playerTransform.position - transform.position).normalized;
        rb.AddForce(new Vector2(direction.x * leapForce, 2f), ForceMode2D.Impulse);

        CheckPolygonDamage(attackPoint1, 10);
        
    }

    public void ScratchDamage()
    {
        CheckPolygonDamage(attackPoint2, 10);
    }

    public void RoarDamage()
    {
        CheckPolygonDamage(attackPoint3, 15);
    }

    private void OnDrawGizmosSelected()
    {
        // Vẽ lại các Polygon Collider trong Scene để dễ debug
        DrawPolygonGizmo(attackPoint1, Color.green);
        DrawPolygonGizmo(attackPoint2, Color.red);
        DrawPolygonGizmo(attackPoint3, Color.blue);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }

    private void DrawPolygonGizmo(Transform point, Color color)
    {
        if (point == null) return;
        PolygonCollider2D poly = point.GetComponent<PolygonCollider2D>();
        if (poly == null) return;

        Gizmos.color = color;
        Vector2[] points = poly.points;
        for (int i = 0; i < points.Length; i++)
        {
            Vector3 p1 = point.TransformPoint(points[i]);
            Vector3 p2 = point.TransformPoint(points[(i + 1) % points.Length]);
            Gizmos.DrawLine(p1, p2);
        }
        if (attackPoint1 != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(attackPoint1.position, attackRange1);
        }
        if (attackPoint2 != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint2.position, attackRange2);
        }
        if (attackPoint3 != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(attackPoint3.position, attackRange3);
        }
    }
    
}
// using DG.Tweening;
// using UnityEngine;

// public class HoXam : BossBase
// {
//     [SerializeField] private PlayerStatus playerStatus;
//     protected override void Attack()
//     {
//         float dist = Vector2.Distance(transform.position, playerTransform.position);
//         isAttacking = true;

//         int attackType ;// Mặc định là 3
//         if (dist <= attackRange2) attackType = 2; // Rất gần thì cào
//         else if (dist <= attackRange1) attackType = 1; // Hơi xa thì nhảy lao
//         else attackType = 3; // Tầm xa nhất thì gầm

//         animator.SetInteger("AttackType", attackType);
//         animator.SetTrigger("Attack");
//         switch (attackType)
//         {
//             case 1:
//                 Invoke(nameof(ScratchDamage), 0.5f);
//                 break;
//             case 2:

//                 Invoke(nameof(LeapLogic), 0.4f);
//                 break;
//             case 3:
//                 Invoke(nameof(RoarDamage), 0.6f);
//                 break;
//         }
//         DOVirtual.DelayedCall(2f, () => EndAttack());
//     }

//     public void LeapLogic()
//     {
//         float leapForce = 7f;
//         Vector2 direction = (playerTransform.position - transform.position).normalized;
//         rb.AddForce(new Vector2(direction.x * leapForce, 2f), ForceMode2D.Impulse);

//         Collider2D hitPlayer = Physics2D.OverlapCircle(attackPoint1.position, attackRange1, playerMask);
//         if (hitPlayer != null)
//             playerStatus.TakeDamage(10);
//     }

//     public void ScratchDamage()
//     {
//         Collider2D hitPlayer = Physics2D.OverlapCircle(attackPoint2.position, attackRange2, playerMask);
//         if (hitPlayer != null)
//             playerStatus.TakeDamage(10);
//     }

//     public void RoarDamage()
//     {
//         Collider2D hitPlayer = Physics2D.OverlapCircle(attackPoint3.position, attackRange3, playerMask);
//         if (hitPlayer != null)
//             playerStatus.TakeDamage(15);
//     }
   
// }
