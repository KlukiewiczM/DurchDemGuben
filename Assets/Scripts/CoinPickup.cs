using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [SerializeField] private string playerTag = "PlayerTag";
    [SerializeField] private int value = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(playerTag)) return;

        if (CoinManager.Instance != null)
            CoinManager.Instance.AddCoin(value);

        Destroy(gameObject);
    }
}
