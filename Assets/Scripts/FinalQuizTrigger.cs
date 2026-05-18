using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FinalQuizTrigger : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private string playerTag = "PlayerTag";

    [Header("Mayor Dialogue")]
    [SerializeField] private GameObject mayorObject;
    [SerializeField] private GameObject mayorMessageObject;
    [SerializeField] private float dialogueDuration = 6f;

    [Header("Quiz UI")]
    [SerializeField] private GameObject quizPanel;
    [SerializeField] private TMP_Text questionText;
    [SerializeField] private TMP_Text[] answerTexts;
    [SerializeField] private Button[] answerButtons;
    [SerializeField] private TMP_Text wrongAnswerText;
    [SerializeField] private string question = "Wo befinden sich unsere Fahrradboxen?";
    [SerializeField] private string[] answers;
    [SerializeField] private int correctAnswerIndex = 0;

    [Header("After Correct Answer")]
    [SerializeField] private AudioSource backgroundMusicSource;
    [SerializeField] private AudioClip finalMusic;
    [SerializeField] private GameObject constructionBackground;
    [SerializeField] private GameObject buildAnimationObject;
    [SerializeField] private GameObject cityFestivalSceneObject;
    [SerializeField] private GameObject crowdObject;

    [Header("Player Auto Run")]
    [SerializeField] private Transform stageTarget;
    [SerializeField] private float runSpeed = 3f;

    [Header("Finish")]
    [SerializeField] private GameObject congratulationsMessage;
    [SerializeField] private float delayBeforeCongratulations = 10f;
    [SerializeField] private bool quitGameOnSpace = true;
    [SerializeField] private string endSceneName = "";

    private bool alreadyTriggered;
    private bool quizActive;
    private bool waitingForFinishSpace;
    private int selectedIndex;

    private PlayerMovement2D playerMovement;
    private Rigidbody2D playerRb;
    private Animator playerAnimator;

    private void Start()
    {
        if (mayorObject != null) mayorObject.SetActive(false);
        if (mayorMessageObject != null) mayorMessageObject.SetActive(false);
        if (quizPanel != null) quizPanel.SetActive(false);
        if (wrongAnswerText != null) wrongAnswerText.gameObject.SetActive(false);
        if (buildAnimationObject != null) buildAnimationObject.SetActive(false);
        if (cityFestivalSceneObject != null) cityFestivalSceneObject.SetActive(false);
        if (crowdObject != null) crowdObject.SetActive(false);
        if (congratulationsMessage != null) congratulationsMessage.SetActive(false);
    }

    private void Update()
    {
        if (quizActive)
            HandleQuizInput();

        if (waitingForFinishSpace && Input.GetKeyDown(KeyCode.Space))
            FinishGame();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (alreadyTriggered) return;

        PlayerMovement2D movement = other.GetComponentInParent<PlayerMovement2D>();
        if (movement == null) return;
        if (!movement.CompareTag(playerTag)) return;

        playerMovement = movement;
        playerRb = movement.GetComponent<Rigidbody2D>();
        playerAnimator = movement.GetComponent<Animator>();

        alreadyTriggered = true;

        if (CoinManager.Instance != null)
            CoinManager.Instance.ResetCoins();

        StartCoroutine(FinalSequence());
    }

    private IEnumerator FinalSequence()
    {
        FreezePlayer();

        if (mayorObject != null)
            mayorObject.SetActive(true);

        if (mayorMessageObject != null)
            mayorMessageObject.SetActive(true);

        yield return new WaitForSeconds(dialogueDuration);

        if (mayorMessageObject != null)
            mayorMessageObject.SetActive(false);

        ShowQuiz();
    }

    private void ShowQuiz()
    {
        quizActive = true;
        selectedIndex = 0;

        if (quizPanel != null)
            quizPanel.SetActive(true);

        if (questionText != null)
            questionText.text = question;

        for (int i = 0; i < answerTexts.Length; i++)
        {
            if (i < answers.Length)
                answerTexts[i].text = answers[i];
        }

        if (wrongAnswerText != null)
            wrongAnswerText.gameObject.SetActive(false);

        UpdateAnswerSelection();
    }

    private void HandleQuizInput()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            selectedIndex++;
            if (selectedIndex >= answerButtons.Length)
                selectedIndex = 0;

            UpdateAnswerSelection();
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            selectedIndex--;
            if (selectedIndex < 0)
                selectedIndex = answerButtons.Length - 1;

            UpdateAnswerSelection();
        }

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            CheckAnswer(selectedIndex);
    }

    private void CheckAnswer(int index)
    {
        if (index == correctAnswerIndex)
        {
            quizActive = false;

            if (wrongAnswerText != null)
                wrongAnswerText.gameObject.SetActive(false);

            if (quizPanel != null)
                quizPanel.SetActive(false);

            StartCoroutine(CorrectAnswerSequence());
        }
        else
        {
            if (wrongAnswerText != null)
            {
                wrongAnswerText.text = "Versuchen Sie es erneut!";
                wrongAnswerText.gameObject.SetActive(true);
            }
        }
    }

    private IEnumerator CorrectAnswerSequence()
    {

        Debug.Log("DOBRA ODPOWIEDZ - START SEKWENCJI");

        if (backgroundMusicSource != null && finalMusic != null)
        {
            backgroundMusicSource.clip = finalMusic;
            backgroundMusicSource.loop = true;
            backgroundMusicSource.Play();
        }

        if (buildAnimationObject != null)
            buildAnimationObject.SetActive(true);

        else
        {
            Debug.LogWarning("Build Animation Object nie jest podpiety!");
        }


        yield return new WaitForSeconds(1.5f);

        if (buildAnimationObject != null)
            buildAnimationObject.SetActive(false);

        if (constructionBackground != null)
        {
            Debug.Log("Dezaktywuje construction");
            constructionBackground.SetActive(false);
        }

        if (cityFestivalSceneObject != null)
        {
            Debug.Log("Aktywuje Stadtfest");
            cityFestivalSceneObject.SetActive(true);
        }

        if (crowdObject != null)
        {
            Debug.Log("Aktywuje crowd");
            crowdObject.SetActive(true);
        }

        if (mayorObject != null)
            mayorObject.SetActive(false);

        yield return StartCoroutine(AutoRunPlayerToStage());

        yield return new WaitForSeconds(delayBeforeCongratulations);

        FreezePlayer();

        if (congratulationsMessage != null)
            congratulationsMessage.SetActive(true);

        waitingForFinishSpace = true;
    }

    private IEnumerator AutoRunPlayerToStage()
    {
        if (stageTarget == null || playerRb == null)
        {
            UnfreezePlayer();
            yield break;
        }

        if (playerMovement != null)
            playerMovement.enabled = false;

        while (Vector2.Distance(playerRb.position, stageTarget.position) > 0.1f)
        {
            Vector2 direction = ((Vector2)stageTarget.position - playerRb.position).normalized;
            playerRb.linearVelocity = new Vector2(direction.x * runSpeed, playerRb.linearVelocity.y);

            if (playerAnimator != null)
                playerAnimator.SetFloat("Speed", Mathf.Abs(playerRb.linearVelocity.x));

            yield return null;
        }

        playerRb.linearVelocity = Vector2.zero;

        if (playerAnimator != null)
        {
            playerAnimator.SetFloat("Speed", 0f);
            playerAnimator.Play("Idle");
        }
    }

    private void UpdateAnswerSelection()
    {
        for (int i = 0; i < answerButtons.Length; i++)
        {
            if (answerButtons[i] == null) continue;

            answerButtons[i].transform.localScale = i == selectedIndex
                ? Vector3.one * 1.1f
                : Vector3.one;
        }
    }

    private void FreezePlayer()
    {
        if (playerRb != null)
        {
            playerRb.linearVelocity = Vector2.zero;
            playerRb.angularVelocity = 0f;
        }

        if (playerAnimator != null)
        {
            playerAnimator.SetFloat("Speed", 0f);
            playerAnimator.SetBool("IsGrounded", true);
            playerAnimator.ResetTrigger("JumpTrigger");
            playerAnimator.Play("Idle");
        }

        if (playerMovement != null)
            playerMovement.enabled = false;
    }

    private void UnfreezePlayer()
    {
        if (playerMovement != null)
            playerMovement.enabled = true;
    }

    private void FinishGame()
    {
        Time.timeScale = 1f;

        if (!string.IsNullOrEmpty(endSceneName))
        {
            SceneManager.LoadScene(endSceneName);
            return;
        }

        if (quitGameOnSpace)
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}