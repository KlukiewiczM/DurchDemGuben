using UnityEngine;
using TMPro;

public class BlinkingText : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private float speed = 2f; // szybkość migania

    void Update()
    {
        if (text == null) return;

        float alpha = Mathf.Abs(Mathf.Sin(Time.unscaledTime * speed));
        Color c = text.color;
        c.a = alpha;
        text.color = c;
    }
}