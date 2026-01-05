using UnityEngine;

public class HoXamMovement : BossMovement
{
    [SerializeField] private float moveSpeed = 2f;
    private Rigidbody2D rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    public override void FaceTarget(Transform target)
    {
        if(transform.position.x < target.position.x)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    public override void MoveTowards()
    {
        Vector3 direction = (FindTarget().position - transform.position).normalized;
        rb.linearVelocityX = direction.x * moveSpeed;
    }

    public override void StopMovement()
    {
        rb.linearVelocityX = 0;
    }
    private Transform FindTarget()
    {
        return GameObject.FindGameObjectWithTag("Player").transform;
    }

    
}
