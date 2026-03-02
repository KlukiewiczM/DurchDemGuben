using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject malePrefab;
    [SerializeField] private GameObject femalePrefab;

    [Header("Optional (for UI characters)")]
    [SerializeField] private Transform uiParent; // przypnij Canvas.transform

    void Start()
    {
        var selected = CharacterSelectController.GetSavedCharacter();
        Debug.Log("Spawning: " + selected);

        GameObject prefabToSpawn =
            (selected == CharacterSelectController.CharacterType.Male)
            ? malePrefab
            : femalePrefab;

        if (uiParent != null)
        {
            Instantiate(prefabToSpawn, uiParent);
        }
        else
        {
            Instantiate(prefabToSpawn, transform.position, Quaternion.identity);
        }
    }
}
