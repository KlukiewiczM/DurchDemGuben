using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


public class FadeManager : MonoBehaviour
{
    [Header("Fade")]
    [SerializeField] private CanvasGroup fadeCanvasGroup;
    [SerializeField] private float fadeDuration = 1f;

    private void Start()
    {
        StartCoroutine(FadeIn());
    }

    public IEnumerator FadeIn()
    {
        if (fadeCanvasGroup == null) yield break;

        fadeCanvasGroup.alpha = 1f;

        float time = 0f;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            fadeCanvasGroup.alpha = Mathf.Lerp(1f, 0f, time / fadeDuration);
            yield return null;
        }

        fadeCanvasGroup.alpha = 0f;
        fadeCanvasGroup.blocksRaycasts = false;
    }

    public IEnumerator FadeOut(string sceneName)
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;

            fadeCanvasGroup.alpha =
                Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);

            yield return null;
        }

        fadeCanvasGroup.alpha = 1f;

        SceneManager.LoadScene(sceneName);
    }
}