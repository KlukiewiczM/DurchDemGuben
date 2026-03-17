using UnityEngine;

public class EnemyState : MonoBehaviour
{
    public bool IsDead { get; private set; }

    public void MarkDead()
    {
        IsDead = true;
    }
}
