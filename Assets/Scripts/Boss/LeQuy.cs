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
// using System;
// using System.Collections.Generic;
// using DG.Tweening;
// using UnityEngine;
// using Random = UnityEngine.Random;

// public class LeQuy : BossBase
// {
//     [SerializeField] private PlayerStatus playerStatus;
//     private ContactFilter2D contactFilter;
//     private List<Collider2D> hitResults = new List<Collider2D>();

//     protected override void Start()
//     {
//         base.Start();
//         contactFilter = new ContactFilter2D();
//         contactFilter.SetLayerMask(playerMask);
//         contactFilter.useTriggers = true;
//         contactFilter.useLayerMask = true;
//     }
//     protected override void Attack()
// {
//     float dist = Vector2.Distance(transform.position, playerTransform.position);
//     isAttacking = true; 

//     // 1. Chọn loại đòn đánh
//     int attackType = Random.Range(1, 4); // Mặc định là 3
//     // if (dist <= attackRange2) attackType = 2; 
//     // else if (dist <= attackRange1) attackType = 1; 
//     // else attackType = 3; 

//     // 2. Chạy Animation
//     animator.SetInteger("AttackType", attackType);
//     animator.SetTrigger("Attack");

//     // 3. Xử lý gây sát thương 
//     float delay = (attackType == 1) ? 0.6f : 0.4f;
//     Invoke(nameof(ExecutePolygonDamage), delay);
// }

// private void ExecutePolygonDamage()
// {
//     // Lấy AttackType hiện tại từ Animator để biết dùng Point nào
//     int type = animator.GetInteger("AttackType");
//     Transform currentPoint = (type == 1) ? attackPoint1 : (type == 2 ? attackPoint2 : attackPoint3);
    
//     if (currentPoint == null) return;

//     PolygonCollider2D poly = currentPoint.GetComponent<PolygonCollider2D>();
    
//     // Quét va chạm
//     ContactFilter2D filter = new ContactFilter2D();
//     filter.SetLayerMask(playerMask);
//     filter.useLayerMask = true;
    
//     Collider2D[] results = new Collider2D[5];
//     int count = poly.Overlap(filter, results);

//     for (int i = 0; i < count; i++)
//     {
//         if (results[i].CompareTag("Player"))
//         {
//             playerStatus.TakeDamage(10); // Sát thương tùy chỉnh
//         }
//     }
// }
//     // protected override void Attack()
//     // {
//     //     float dist = Vector2.Distance(transform.position, playerTransform.position);
//     //     isAttacking = true;

//     //     int attackType;
//     //     if (dist <= attackRange2) attackType = 2;
//     //     else if (dist <= attackRange1) attackType = 1;
//     //     else attackType = 3;

//     //     animator.SetInteger("AttackType", attackType);
//     //     animator.SetTrigger("Attack");
//     //     switch (attackType)
//     //     {
//     //         case 1:
//     //             Invoke(nameof(Attack1Damage), 0.6f);
//     //             break;
//     //         case 2:
//     //             Invoke(nameof(Attack2Damage), 0.4f);
//     //             break;
//     //         case 3:
//     //             Invoke(nameof(Attack3Damage), 0.4f);
//     //             break;
//     //     }
//     //     DOVirtual.DelayedCall(2f, () => EndAttack());

//     // }

//     private void CheckPolygonDamage(Transform attackPoint)
//     {
//         // This method can be used to check for polygon collider damage if needed
//         PolygonCollider2D polyCollider = attackPoint.GetComponent<PolygonCollider2D>();
//         if (polyCollider == null) return;

//         //quet tat ca cac doi tuong cham vao polygon
//         int hitCount = polyCollider.Overlap(contactFilter, hitResults);

//         for (int i = 0; i < hitCount; i++)
//         {
//             if (hitResults[i].CompareTag("Player"))
//             {
//                 hitResults[i].GetComponent<PlayerStatus>().TakeDamage(10);
//                 Debug.Log("Player hit by polygon attack");
//             }
//         }


//     }
//     private void Attack3Damage()
//     {
//         attackPoint3.gameObject.SetActive(true);
//         CheckPolygonDamage(attackPoint3);
//         Invoke(nameof(DisableAttackPoints), 0.2f);
//     }



//     private void Attack2Damage()
//     {
//         attackPoint2.gameObject.SetActive(true);
//         CheckPolygonDamage(attackPoint2);
//         Invoke(nameof(DisableAttackPoints), 0.2f);
//     }

//     private void Attack1Damage()
//     {
//         attackPoint1.gameObject.SetActive(true);
//         CheckPolygonDamage(attackPoint1);
//         Invoke(nameof(DisableAttackPoints), 0.2f);
//     }
//     private void DisableAttackPoints()
//     {
//         attackPoint3.gameObject.SetActive(false);
//         attackPoint2.gameObject.SetActive(false);
//         attackPoint1.gameObject.SetActive(false);
//     }
//     private void OnDrawGizmosSelected()
//     {
//         // Attack Ranges
//         Gizmos.color = Color.red;
//         Gizmos.DrawWireSphere(attackPoint1.position, attackRange1);
//         Gizmos.DrawWireSphere(attackPoint2.position, attackRange2);
//         Gizmos.DrawWireSphere(attackPoint3.position, attackRange3);

//         Gizmos.color = Color.blue;
//         Gizmos.DrawWireSphere(transform.position, detectionRange);
//     }
// }
