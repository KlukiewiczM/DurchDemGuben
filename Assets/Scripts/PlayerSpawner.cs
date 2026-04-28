using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [Header("Player Prefabs")]
    [SerializeField] private GameObject malePlayerPrefab;
    [SerializeField] private GameObject femalePlayerPrefab;

    [Header("Spawn")]
    [SerializeField] private Transform spawnPoint;

    private const string PrefKey = "SelectedCharacter";

    private void Start()
    {
        SpawnSelectedPlayer();
    }

    private void SpawnSelectedPlayer()
    {
        int selectedCharacter = PlayerPrefs.GetInt(PrefKey, 0);

        GameObject prefabToSpawn = selectedCharacter == 0
            ? malePlayerPrefab
            : femalePlayerPrefab;

        if (prefabToSpawn == null)
        {
            Debug.LogError("Player prefab is missing!");
            return;
        }

        Vector3 spawnPosition = spawnPoint != null
            ? spawnPoint.position
            : transform.position;

        GameObject spawnedPlayer = Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);

        SetupCamera(spawnedPlayer);
    }

    private void SetupCamera(GameObject player)
    {
        CameraFollow2D cameraFollow = Camera.main.GetComponent<CameraFollow2D>();

        if (cameraFollow != null)
        {
            cameraFollow.SetTarget(player.transform);
        }
    }
}