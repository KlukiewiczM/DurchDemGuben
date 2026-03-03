using UnityEngine;

public class EnemyPatrol2D : MonoBehaviour
{
    [Header("Patrol")]
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
    [SerializeField] private float speed = 2f;
    [SerializeField] private float arriveDistance = 0.05f;

    private Transform target;

    private void Start()
    {
        target = pointB;
    }

    private void Update()
    {
        if (pointA == null || pointB == null) return;

        transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, target.position) <= arriveDistance)
        {
            target = (target == pointA) ? pointB : pointA;
        }
    }
}
