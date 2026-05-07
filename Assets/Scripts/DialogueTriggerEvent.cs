using System.Collections;
using UnityEngine;

public class DialogueTriggerEvent : MonoBehaviour
{
    [Header("Trigger")]
    [SerializeField] private string playerTag = "PlayerTag";

    [Header("Objects to show")]
    [SerializeField] private GameObject storyNpc;
    [SerializeField] private GameObject storyMessage;

    [Header("Settings")]
    [SerializeField] private bool triggerOnlyOnce = true;
    [SerializeField] private bool waitForSpace = true;
    [SerializeField] private float autoHideAfterSeconds = 3f;
    [SerializeField] private float delayBeforeDialogue = 2f;

    [Header("Space unlock")]
    [SerializeField] private float secondsBeforeSpaceAllowed = 6f;

    private bool hasTriggered = false;

    private PlayerMovement2D playerMovement;
    private Rigidbody2D playerRb;
    private Animator playerAnimator;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasTriggered && triggerOnlyOnce) return;

        PlayerMovement2D movement = other.GetComponentInParent<PlayerMovement2D>();
        if (movement == null) return;

        if (!movement.CompareTag(playerTag)) return;

        playerMovement = movement;
        playerRb = movement.GetComponent<Rigidbody2D>();
        playerAnimator = movement.GetComponent<Animator>();

        hasTriggered = true;
        StartCoroutine(PlayDialogue());
    }

    private IEnumerator PlayDialogue()
    {
        FreezePlayer();

        yield return new WaitForSeconds(delayBeforeDialogue);

        if (storyNpc != null)
            storyNpc.SetActive(true);

        if (storyMessage != null)
            storyMessage.SetActive(true);

        if (waitForSpace)
        {
            // blokada skipa
            yield return new WaitForSeconds(secondsBeforeSpaceAllowed);

            // dopiero teraz można zamknąć SPACE
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

        UnfreezePlayer();
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
}