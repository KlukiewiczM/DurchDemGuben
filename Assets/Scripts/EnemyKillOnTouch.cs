using System.Collections;
using UnityEngine;

public class EnemyKillOnTouch : MonoBehaviour
{
    [SerializeField] private int damage = 1;

    private EnemyState state;

    private void Awake()
    {
        state = GetComponentInParent<EnemyState>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerLife life = other.GetComponentInParent<PlayerLife>();

        if (life == null) return;

        StartCoroutine(DealDamageNextFrame(life));
    }

    private IEnumerator DealDamageNextFrame(PlayerLife life)
    {
        yield return null;

        if (state != null && state.IsDead)
            yield break;

        if (life != null)
            life.TakeDamage(damage);
    }
}