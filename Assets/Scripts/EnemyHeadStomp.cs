using UnityEngine;

public class EnemyHeadStomp : MonoBehaviour
{
    [SerializeField] private string feetTag = "PlayerFeet";
    [SerializeField] private float bounceForce = 10f;
    [SerializeField] private GameObject enemyRoot;

    [Header("Audio")]
    [SerializeField] private AudioClip enemyKillSound;

    private bool killed;
    private EnemyState state;

    private void Awake()
    {
        if (enemyRoot == null && transform.parent != null)
            enemyRoot = transform.parent.gameObject;

        if (enemyRoot != null)
            state = enemyRoot.GetComponent<EnemyState>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (killed) return;
        if (!other.CompareTag(feetTag)) return;

        killed = true;

        if (state != null)
            state.MarkDead();

        PlayKillSound();

        if (enemyRoot != null)
        {
            foreach (var dmg in enemyRoot.GetComponentsInChildren<EnemyKillOnTouch>())
                dmg.enabled = false;
        }

        var rb = other.attachedRigidbody;

        if (rb != null)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            rb.AddForce(Vector2.up * bounceForce, ForceMode2D.Impulse);
        }

        if (enemyRoot != null)
        {
            foreach (var c in enemyRoot.GetComponentsInChildren<Collider2D>())
                c.enabled = false;

            Destroy(enemyRoot, 0.05f);
        }
    }

    private void PlayKillSound()
    {
        if (enemyKillSound != null)
        {
            AudioSource.PlayClipAtPoint(
                enemyKillSound,
                Camera.main.transform.position,
                1f
            );
        }
    }
}