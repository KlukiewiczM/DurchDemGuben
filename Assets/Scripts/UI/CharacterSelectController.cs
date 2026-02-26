using System.Diagnostics;
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
        if (PlayerPrefs.HasKey(PrefKey))
            selected = (CharacterType)PlayerPrefs.GetInt(PrefKey);
        else
            selected = CharacterType.Male;

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
            UnityEngine.Debug.Log("Selected = " + selected);
            lastInputTime = Time.unscaledTime;
            ApplySelectionVisual();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            selected = CharacterType.Female;
            UnityEngine.Debug.Log("Selected = " + selected);
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

        // canvas (kamera może być null przy Screen Space Overlay)
        Canvas canvas = selector.GetComponentInParent<Canvas>();
        Camera cam = (canvas != null && canvas.renderMode != RenderMode.ScreenSpaceOverlay)
            ? canvas.worldCamera
            : null;

        // bierzemy screen position targetu
        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(cam, target.position);

        // i konwertujemy na local point w przestrzeni rodzica selector’a
        RectTransform parent = selector.parent as RectTransform;
        if (parent == null) return;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(parent, screenPoint, cam, out Vector2 localPoint);

        // offset pod postacią
        localPoint += new Vector2(0f, -80f);

        selector.anchoredPosition = localPoint;
    }

    public static CharacterType GetSavedCharacter()
    {
        int val = PlayerPrefs.GetInt(PrefKey, 0);
        return (CharacterType)val;
    }
}
