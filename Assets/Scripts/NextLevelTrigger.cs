using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevelTrigger : MonoBehaviour
{
    [SerializeField] private string playerTag = "PlayerTag";

    [Header("Next Scene")]
    [SerializeField] private string nextSceneName = "2LevelScene";

    [Header("Optional Condition")]
    [SerializeField] private GameObject buildingPlaceholder; // jeśli chcesz sprawdzać budynek
    [SerializeField] private bool requireBuilding = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(playerTag)) return;

        if (requireBuilding)
        {
            if (buildingPlaceholder == null || !buildingPlaceholder.activeSelf)
            {
                Debug.Log("Najpierw musisz zbudować budynek!");
                return;
            }
        }

        SceneManager.LoadScene(nextSceneName);
    }
}