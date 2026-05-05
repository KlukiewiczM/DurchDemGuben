using System.Collections;
using TMPro;
using UnityEngine;

public class DialogueAutoScroll : MonoBehaviour
{
    [TextArea]
    [SerializeField] private string[] messages;

    [SerializeField] private TextMeshProUGUI textUI;
    [SerializeField] private float timeBetweenMessages = 2f;

    private int index = 0;

    private void OnEnable()
    {
        index = 0;
        StartCoroutine(PlayDialogue());
    }

    private IEnumerator PlayDialogue()
    {
        while (index < messages.Length)
        {
            textUI.text = messages[index];
            index++;

            yield return new WaitForSeconds(timeBetweenMessages);
        }
    }
}