using System.Collections;
using UnityEngine;

public class EnemyKillOnTouch : MonoBehaviour
{
    [SerializeField] private string playerTag = "PlayerTag";
    [SerializeField] private int damage = 1;

    private EnemyState state;

    private void Awake()
    {
        state = GetComponentInParent<EnemyState>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(playerTag)) return;

        StartCoroutine(DealDamageNextFrame(other));
    }

    private IEnumerator DealDamageNextFrame(Collider2D other)
    {
        yield return null; // czekamy 1 klatkę

        // jeśli wróg został zabity stompem, nie zadajemy obrażeń
        if (state != null && state.IsDead)
            yield break;

        var life = other.GetComponent<PlayerLife>();
        if (life == null)
            life = other.GetComponentInParent<PlayerLife>();

        if (life != null)
            life.TakeDamage(damage);
        else
            Debug.LogWarning("Enemy touched player, but PlayerLife not found.");
    }
}
