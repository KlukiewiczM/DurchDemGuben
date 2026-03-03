using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    public static RespawnManager Instance { get; private set; }

    [Header("Defaults")]
    [SerializeField] private Transform defaultSpawnPoint;
    [SerializeField] private float respawnDelay = 0.2f;

    private Transform currentSpawnPoint;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        currentSpawnPoint = defaultSpawnPoint;
    }

    public void SetCheckpoint(Transform checkpoint)
    {
        currentSpawnPoint = checkpoint != null ? checkpoint : defaultSpawnPoint;
    }

    public void Respawn(GameObject player)
    {
        if (player == null) return;
        StartCoroutine(RespawnRoutine(player));
    }

    private System.Collections.IEnumerator RespawnRoutine(GameObject player)
    {
        // opcjonalnie: chwilowe "zniknięcie" / blokada ruchu
        var rb = player.GetComponent<Rigidbody2D>();
        if (rb != null) rb.linearVelocity = Vector2.zero;

        yield return new WaitForSeconds(respawnDelay);

        Vector3 pos = (currentSpawnPoint != null) ? currentSpawnPoint.position :
                      (defaultSpawnPoint != null) ? defaultSpawnPoint.position :
                      Vector3.zero;

        player.transform.position = pos;

        if (rb != null) rb.linearVelocity = Vector2.zero;
    }
}
