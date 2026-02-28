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
        int attackType = Random.Range(1, 4);
        

        animator.SetInteger("AttackType", attackType);
        animator.SetTrigger("Attack");

        // Gọi logic gây sát thương theo thời điểm Animation vung đòn
        switch (attackType)
        {
            case 1:
                Invoke(nameof(LeapLogic), 0.4f);
                SFXManager.instance.PlaySFX(SFXType.TIGER_SCRAP);
                break;
            case 2:
                Invoke(nameof(ScratchDamage), 0.5f);
                SFXManager.instance.PlaySFX(SFXType.TIGER_SCRAP);
                break;
            case 3:
                Invoke(nameof(RoarDamage), 0.6f);
                SFXManager.instance.PlaySFX(SFXType.TIGER_ROAR);
                break;
        }

    }

    // Hàm dùng chung để quét sát thương bằng Polygon Collider
    private void CheckPolygonDamage(Transform attackPoint, int damage)
    {
        if (attackPoint == null) return;

        PolygonCollider2D poly = attackPoint.GetComponent<PolygonCollider2D>();
        if (poly == null)
        {
            Debug.LogWarning($"Missing PolygonCollider2D on {attackPoint.name}");
            return;
        }


        int hitCount = poly.Overlap(playerFilter, hitResults);

        for (int i = 0; i < hitCount; i++)
        {
            if (hitResults[i].CompareTag("Player"))
            {
                AnimationManager.instance?.PlayerHurtAnim();
                playerStatus.TakeDamage(damage);
                Debug.Log($"Hit Player with {attackPoint.name}");
                break; 
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
        // if (attackPoint1 != null)
        // {
        //     Gizmos.color = Color.green;
        //     Gizmos.DrawWireSphere(attackPoint1.position, attackRange1);
        // }
        // if (attackPoint2 != null)
        // {
        //     Gizmos.color = Color.red;
        //     Gizmos.DrawWireSphere(attackPoint2.position, attackRange2);
        // }
        // if (attackPoint3 != null)
        // {
        //     Gizmos.color = Color.blue;
        //     Gizmos.DrawWireSphere(attackPoint3.position, attackRange3);
        // }
    }

}

