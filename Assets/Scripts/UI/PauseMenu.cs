using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject pauseMenuPanel;

    [Header("Buttons")]
    [SerializeField] private GameObject continueButton;
    [SerializeField] private GameObject exitButton;

    private bool isPaused = false;
    private int selectedIndex = 0;

    private void Start()
    {
        if (pauseMenuPanel != null)
            pauseMenuPanel.SetActive(false);

        Time.timeScale = 1f;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ContinueGame();
            else
                PauseGame();
        }

        if (!isPaused) return;

        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            selectedIndex = selectedIndex == 0 ? 1 : 0;
            UpdateSelection();
        }

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            if (selectedIndex == 0)
                ContinueGame();
            else
                ExitGame();
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        selectedIndex = 0;

        if (pauseMenuPanel != null)
            pauseMenuPanel.SetActive(true);

        UpdateSelection();

        Time.timeScale = 0f;
    }

    public void ContinueGame()
    {
        isPaused = false;

        if (pauseMenuPanel != null)
            pauseMenuPanel.SetActive(false);

        Time.timeScale = 1f;
    }

    public void ExitGame()
    {
        Time.timeScale = 1f;

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void UpdateSelection()
    {
        if (continueButton != null)
            continueButton.transform.localScale = selectedIndex == 0 ? Vector3.one * 1.15f : Vector3.one;

        if (exitButton != null)
            exitButton.transform.localScale = selectedIndex == 1 ? Vector3.one * 1.15f : Vector3.one;
    }
}