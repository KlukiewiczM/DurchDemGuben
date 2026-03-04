using UnityEngine;

public class PlayerLife : MonoBehaviour
{
    [Header("Lives")]
    [SerializeField] private int maxLives = 3;
    [SerializeField] private int lives = 3;
    [SerializeField] private HeartsUI heartsUI;

    [Header("Tags")]
    [SerializeField] private string killTag = "KillZone";

    [Header("I-Frames")]
    [SerializeField] private float invincibilityTime = 0.5f;

    private bool isDead;
    private bool invincible;

    private void Start()
    {
        lives = Mathf.Clamp(lives, 0, maxLives);
        if (heartsUI != null) heartsUI.SetHearts(lives);
    }

    public void TakeDamage(int amount)
    {
        if (isDead || invincible) return;

        lives = Mathf.Max(0, lives - amount);
        if (heartsUI != null) heartsUI.SetHearts(lives);

        if (lives <= 0)
        {
            Die();
            return;
        }

        // i-frames
        invincible = true;
        Invoke(nameof(ResetInvincible), invincibilityTime);
    }

    private void ResetInvincible() => invincible = false;

    public void Die()
    {
        if (isDead) return;
        isDead = true;

        RespawnManager.Instance.Respawn(gameObject);

        // po respawnie wracamy do pełnego HP (na razie prosto)
        lives = maxLives;
        if (heartsUI != null) heartsUI.SetHearts(lives);

        Invoke(nameof(ResetDeadFlag), 0.3f);
    }

    private void ResetDeadFlag() => isDead = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(killTag))
            Die();
    }
}
