using UnityEngine;

public class HoXam : BossBase
{
    protected override void Attack()
    {
        float dist = Vector2.Distance(transform.position, playerTransform.position);
        isAttacking = true;

        int attackType;
        if (dist <= attackRange2) attackType = 2; // Rất gần thì cào
        else if (dist <= attackRange1) attackType = 1; // Hơi xa thì nhảy lao
        else attackType = 3; // Tầm xa nhất thì gầm

        animator.SetInteger("AttackType", attackType);
        animator.SetTrigger("Attack");
    }
    // protected override void Attack()
    // {
    //     isAttacking = true;
    //     int attackType = Random.Range(1, 4);
    //     animator.SetInteger("AttackType", attackType);
    //     animator.SetTrigger("Attack");
    //     if (attackType == 1)
    //     {
    //         // Leap Attack
    //         Invoke(nameof(LeapLogic), 0.5f);
    //     }
    //     else if (attackType == 2)
    //     {
    //         // Scratch Attack
    //         Invoke(nameof(ScratchDamage), 0.4f);
    //     }
    //     else if (attackType == 3)
    //     {
    //         // Roar Attack
    //         Invoke(nameof(RoarDamage), 0.6f);
    //     }
    // }
    public void LeapLogic()
    {
        float leapForce = 7f;
        Vector2 direction = (playerTransform.position - transform.position).normalized;
        rb.AddForce(new Vector2(direction.x * leapForce, 2f), ForceMode2D.Impulse);

        Collider2D hitPlayer = Physics2D.OverlapCircle(attackPoint1.position, 1.5f, playerMask);
        if (hitPlayer != null)
            hitPlayer.GetComponent<PlayerStatus>().TakeDamage(10);
    }

    public void ScratchDamage()
    {
        Collider2D hitPlayer = Physics2D.OverlapCircle(attackPoint2.position, 1.5f, playerMask);
        if (hitPlayer != null)
            hitPlayer.GetComponent<PlayerStatus>().TakeDamage(10);
    }

    public void RoarDamage()
    {
        Collider2D hitPlayer = Physics2D.OverlapCircle(attackPoint3.position, 2f, playerMask);
        if (hitPlayer != null)
            hitPlayer.GetComponent<PlayerStatus>().TakeDamage(15);
    }
    private void OnDrawGizmosSelected()
    {


        // Attack
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

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
