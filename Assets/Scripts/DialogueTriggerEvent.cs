using System.Collections;
using UnityEngine;

public class DialogueTriggerEvent : MonoBehaviour
{
    [Header("Trigger")]
    [SerializeField] private string playerTag = "PlayerTag";

    [Header("Objects to show")]
    [SerializeField] private GameObject storyNpc;
    [SerializeField] private GameObject storyMessage;

    [Header("Player control")]
    [SerializeField] private MonoBehaviour playerMovementScript;

    [Header("Settings")]
    [SerializeField] private bool triggerOnlyOnce = true;
    [SerializeField] private bool waitForSpace = true;
    [SerializeField] private float autoHideAfterSeconds = 3f;
    [SerializeField] private float delayBeforeDialogue = 2f;

    private bool hasTriggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasTriggered && triggerOnlyOnce) return;
        if (!other.CompareTag(playerTag)) return;

        hasTriggered = true;
        StartCoroutine(PlayDialogue());
    }

    private IEnumerator PlayDialogue()
    {
        if (playerMovementScript != null)
            playerMovementScript.enabled = false;

        yield return new WaitForSeconds(delayBeforeDialogue);

        if (storyNpc != null)
            storyNpc.SetActive(true);

        if (storyMessage != null)
            storyMessage.SetActive(true);

        if (waitForSpace)
        {
            while (!Input.GetKeyDown(KeyCode.Space))
                yield return null;
        }
        else
        {
            yield return new WaitForSeconds(autoHideAfterSeconds);
        }

        if (storyNpc != null)
            storyNpc.SetActive(false);

        if (storyMessage != null)
            storyMessage.SetActive(false);

        if (playerMovementScript != null)
            playerMovementScript.enabled = true;
    }
}