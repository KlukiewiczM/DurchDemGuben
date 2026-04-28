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

    [Header("Character Animators")]
    [SerializeField] private Animator maleAnimator;
    [SerializeField] private Animator femaleAnimator;

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
        if (!Input.GetKeyDown(KeyCode.Return) && !Input.GetKeyDown(KeyCode.KeypadEnter))
            return;

        isConfirming = true;

        PlayerPrefs.SetInt(PrefKey, (int)selected);
        PlayerPrefs.Save();

        PlaySelectedCharacterJump();

        Invoke(nameof(LoadSelectedScene), loadDelay);
    }

    //private void PlaySelectedCharacterJump()
    //{
    //    if (selected == CharacterType.Male)
    //    {
    //        if (maleAnimator != null)
    //            maleAnimator.SetTrigger(JumpTriggerName);
    //    }
    //    else
    //    {
    //        if (femaleAnimator != null)
    //            femaleAnimator.SetTrigger(JumpTriggerName);
    //    }
    //}

    private void PlaySelectedCharacterJump()
    {
        if (selected == CharacterType.Male)
        {
            Debug.Log("Male selected - jump animation");

            if (maleAnimator != null)
                maleAnimator.SetTrigger(JumpTriggerName);
            else
                Debug.LogWarning("Male Animator is not assigned!");
        }
        else if (selected == CharacterType.Female)
        {
            Debug.Log("Female selected - jump animation");

            if (femaleAnimator != null)
                femaleAnimator.SetTrigger(JumpTriggerName);
            else
                Debug.LogWarning("Female Animator is not assigned!");
        }
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