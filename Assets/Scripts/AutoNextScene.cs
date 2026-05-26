using UnityEngine;
using UnityEngine.SceneManagement;

public class AutoNextScene : MonoBehaviour
{
    [SerializeField] private string nextSceneName;
    [SerializeField] private float delayBeforeNextScene = 7f;

    private void Start()
    {
        Invoke(nameof(LoadNextScene), delayBeforeNextScene);
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}