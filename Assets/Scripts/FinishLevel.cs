using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishLevel : MonoBehaviour
{
    [SerializeField] private string nextSceneName = "2LevelScene";

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Trigger hit by: " + collision.name + " tag=" + collision.tag);

        if (collision.CompareTag("PlayerTag"))
        {
            Debug.Log("Loading scene: " + nextSceneName);
            SceneManager.LoadScene(nextSceneName);
        }
    }
}