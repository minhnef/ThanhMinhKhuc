using UnityEngine;

//<summary>
// The BossMovement class handles the movement logic for the boss character.
public abstract class BossMovement : MonoBehaviour
{
    public abstract void MoveTowards();
    public abstract void StopMovement();
    public abstract void FaceTarget(Transform target);
}
