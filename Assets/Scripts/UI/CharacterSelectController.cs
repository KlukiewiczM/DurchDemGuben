using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelectController : MonoBehaviour
{
    public enum CharacterType
    {
        Male = 0,
        Female = 1
    }

    [Header("UI")]
    [SerializeField] private RectTransform selector;
    [SerializeField] private Vector2 maleSelectorPosition = new Vector2(-500f, -210f);
    [SerializeField] private Vector2 femaleSelectorPosition = new Vector2(500f, -210f);

    [Header("Character RectTransforms")]
    [SerializeField] private RectTransform maleCharacter;
    [SerializeField] private RectTransform femaleCharacter;

    [Header("Character Animators")]
    [SerializeField] private Animator maleAnimator;
    [SerializeField] private Animator femaleAnimator;

    [Header("Jump Visual")]
    [SerializeField] private float jumpHeight = 80f;
    [SerializeField] private float jumpDuration = 0.35f;

    [Header("Scene")]
    [SerializeField] private string level1SceneName = "Level1Scene";
    [SerializeField] private float loadDelay = 0.5f;

    [Header("Input")]
    [SerializeField] private float inputCooldown = 0.15f;

    private CharacterType selected = CharacterType.Male;
    private float lastInputTime;
    private bool isConfirming = false;

    private const string PrefKey = "SelectedCharacter";
    private const string JumpTriggerName = "JumpTrigger";

    private void Start()
    {
        selected = (CharacterType)PlayerPrefs.GetInt(PrefKey, 0);
        ApplySelectionVisual();
    }

    private void Update()
    {
        if (isConfirming) return;

        HandleLeftRight();
        HandleConfirm();
    }

    private void HandleLeftRight()
    {
        if (Time.unscaledTime - lastInputTime < inputCooldown)
            return;

        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            selected = CharacterType.Male;
            lastInputTime = Time.unscaledTime;
            ApplySelectionVisual();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            selected = CharacterType.Female;
            lastInputTime = Time.unscaledTime;
            ApplySelectionVisual();
        }
    }

    private void HandleConfirm()
    {
        if (!Input.GetKeyDown(KeyCode.Return) && !Input.GetKeyDown(KeyCode.Space))
            return;

        isConfirming = true;

        PlayerPrefs.SetInt(PrefKey, (int)selected);
        PlayerPrefs.Save();

        PlaySelectedCharacterJump();

        Invoke(nameof(LoadSelectedScene), loadDelay);
    }

    private void PlaySelectedCharacterJump()
    {
        if (selected == CharacterType.Male)
        {
            if (maleAnimator != null)
                maleAnimator.SetTrigger(JumpTriggerName);

            if (maleCharacter != null)
                StartCoroutine(JumpUICharacter(maleCharacter));
        }
        else
        {
            if (femaleAnimator != null)
                femaleAnimator.SetTrigger(JumpTriggerName);

            if (femaleCharacter != null)
                StartCoroutine(JumpUICharacter(femaleCharacter));
        }
    }

    private IEnumerator JumpUICharacter(RectTransform character)
    {
        Vector2 startPosition = character.anchoredPosition;
        Vector2 topPosition = startPosition + new Vector2(0f, jumpHeight);

        float halfDuration = jumpDuration / 2f;
        float timer = 0f;

        while (timer < halfDuration)
        {
            timer += Time.unscaledDeltaTime;
            float t = timer / halfDuration;
            character.anchoredPosition = Vector2.Lerp(startPosition, topPosition, t);
            yield return null;
        }

        timer = 0f;

        while (timer < halfDuration)
        {
            timer += Time.unscaledDeltaTime;
            float t = timer / halfDuration;
            character.anchoredPosition = Vector2.Lerp(topPosition, startPosition, t);
            yield return null;
        }

        character.anchoredPosition = startPosition;
    }

    private void ApplySelectionVisual()
    {
        if (selector == null) return;

        selector.anchoredPosition = selected == CharacterType.Male
            ? maleSelectorPosition
            : femaleSelectorPosition;
    }

    private void LoadSelectedScene()
    {
        SceneManager.LoadScene(level1SceneName);
    }

    public static CharacterType GetSavedCharacter()
    {
        return (CharacterType)PlayerPrefs.GetInt(PrefKey, 0);
    }
}