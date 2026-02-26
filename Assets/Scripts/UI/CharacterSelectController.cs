using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelectController : MonoBehaviour
{
    public enum CharacterType { Male = 0, Female = 1 }

    [Header("UI References")]
    [SerializeField] private RectTransform selector;      // strzałka / ramka
    [SerializeField] private RectTransform maleTarget;    // pozycja nad męską postacią
    [SerializeField] private RectTransform femaleTarget;  // pozycja nad żeńską postacią

    [Header("Scene")]
    [SerializeField] private string level1SceneName = "Level1Scene";

    private CharacterType selected = CharacterType.Male;
    private float inputCooldown = 0.15f;
    private float lastInputTime;

    private const string PrefKey = "SelectedCharacter";

    private void Start()
    {
        // domyślnie ustaw na męską
        ApplySelectionVisual();
    }

    private void Update()
    {
        HandleLeftRight();
        HandleConfirm();
    }

    private void HandleLeftRight()
    {
        if (Time.unscaledTime - lastInputTime < inputCooldown) return;

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
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            PlayerPrefs.SetInt(PrefKey, (int)selected);
            PlayerPrefs.Save();

            SceneManager.LoadScene(level1SceneName);
        }
    }

    private void ApplySelectionVisual()
    {
        if (selector == null) return;

        RectTransform target = (selected == CharacterType.Male) ? maleTarget : femaleTarget;
        if (target == null) return;

        // offset w dół (w pikselach UI)
        Vector2 offset = new Vector2(0f, -100f);

        // jeśli to UI w tym samym Canvasie, to lepiej używać anchoredPosition
        selector.anchoredPosition = target.anchoredPosition + offset;
    }

    public static CharacterType GetSavedCharacter()
    {
        int val = PlayerPrefs.GetInt(PrefKey, 0);
        return (CharacterType)val;
    }
}
