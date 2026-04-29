using UnityEngine;

public class PlayerLife : MonoBehaviour
{
    [Header("Lives")]
    [SerializeField] private int maxLives = 3;
    [SerializeField] private int lives = 3;
    [SerializeField] private HeartsUI heartsUI;

    [Header("Tags")]
    [SerializeField] private string killTag = "KillZone";
    [SerializeField] private string heartUITag = "HeartUI";

    [Header("I-Frames")]
    [SerializeField] private float invincibilityTime = 0.5f;

    private bool isDead;
    private bool invincible;

    private void Start()
    {
        AssignHeartsUI();

        lives = Mathf.Clamp(lives, 0, maxLives);

        if (heartsUI != null)
            heartsUI.SetHearts(lives);
    }

    private void AssignHeartsUI()
    {
        if (heartsUI != null) return;

        GameObject uiObject = GameObject.FindGameObjectWithTag(heartUITag);

        if (uiObject != null)
        {
            heartsUI = uiObject.GetComponent<HeartsUI>();
        }

        if (heartsUI == null)
        {
            Debug.LogWarning("HeartsUI not found!");
        }
    }

    public void TakeDamage(int amount)
    {
        if (isDead || invincible) return;

        lives = Mathf.Max(0, lives - amount);

        if (heartsUI != null)
            heartsUI.SetHearts(lives);

        if (lives <= 0)
        {
            Die();
            return;
        }

        invincible = true;
        Invoke(nameof(ResetInvincible), invincibilityTime);
    }

    private void ResetInvincible()
    {
        invincible = false;
    }

    public void Die()
    {
        if (isDead) return;

        isDead = true;

        RespawnManager.Instance.Respawn(gameObject);

        lives = maxLives;

        if (heartsUI != null)
            heartsUI.SetHearts(lives);

        Invoke(nameof(ResetDeadFlag), 0.3f);
    }

    private void ResetDeadFlag()
    {
        isDead = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(killTag))
        {
            Die();
        }
    }
}