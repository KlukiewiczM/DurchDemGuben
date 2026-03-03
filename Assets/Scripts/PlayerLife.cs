using UnityEngine;

public class PlayerLife : MonoBehaviour
{
    [SerializeField] private string killTag = "KillZone"; // opcjonalnie
    /*[SerializeField] private string hazardTag = "Hazard";*/ // kolce/przeciwnik

    private bool isDead;

    public void Die()
    {
        if (isDead) return;
        isDead = true;

        RespawnManager.Instance.Respawn(gameObject);

        // odblokuj po krótkiej chwili (żeby nie zabiło 10x pod rząd)
        Invoke(nameof(ResetDeadFlag), 0.3f);
    }

    private void ResetDeadFlag() => isDead = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // KillZone / Hazard jako trigger
        if (other.CompareTag(killTag)) /*|| other.CompareTag(hazardTag))*/
            Die();
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    // Hazard jako collider (nie trigger)
    //    if (collision.collider.CompareTag(hazardTag))
    //        Die();
    //}
}
