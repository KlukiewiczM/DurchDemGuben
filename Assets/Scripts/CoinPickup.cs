using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [SerializeField] private string playerTag = "PlayerTag";
    [SerializeField] private int value = 1;

    [Header("Audio")]
    [SerializeField] private AudioClip collectSound;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(playerTag))
            return;

        if (CoinManager.Instance != null)
            CoinManager.Instance.AddCoin(value);

        PlayCollectSound();

        Destroy(gameObject);
    }

    private void PlayCollectSound()
    {
        if (collectSound != null)
        {
            AudioSource.PlayClipAtPoint(
                collectSound,
                Camera.main.transform.position,
                1f
            );
        }
    }
}