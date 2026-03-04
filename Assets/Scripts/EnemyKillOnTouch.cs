using UnityEngine;

public class EnemyKillOnTouch : MonoBehaviour
{
    [SerializeField] private string playerTag = "PlayerTag";
    [SerializeField] private int damage = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(playerTag)) return;

        // Szukamy PlayerLife na graczu (albo na rodzicu, gdyby collider był childem)
        var life = other.GetComponent<PlayerLife>();
        if (life == null) life = other.GetComponentInParent<PlayerLife>();

        if (life != null)
        {
            life.TakeDamage(damage);
        }
        else
        {
            Debug.LogWarning("Enemy touched player, but PlayerLife not found on player.");
        }
    }
}
