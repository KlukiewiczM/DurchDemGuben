using System.Collections;
using UnityEngine;

public class NotEnoughCoinsTrigger : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private string playerTag = "PlayerTag";

    [Header("Coins")]
    [SerializeField] private int requiredCoins = 3;

    [Header("Objects To Show")]
    [SerializeField] private GameObject npcObject;
    [SerializeField] private GameObject messageObject;

    [Header("Blocking")]
    [SerializeField] private GameObject blockingWall;

    [Header("Dialogue")]
    [SerializeField] private bool waitForSpace = true;
    [SerializeField] private float autoHideAfterSeconds = 3f;
    [SerializeField] private float delayBeforeClose = 2f;

    private bool waitingForContinue;

    private PlayerMovement2D playerMovement;
    private Rigidbody2D playerRb;
    private Animator playerAnimator;

    private void Start()
    {
        if (npcObject != null)
            npcObject.SetActive(false);

        if (messageObject != null)
            messageObject.SetActive(false);

        CheckWallState();
    }

    private void Update()
    {
        CheckWallState();

        if (waitingForContinue && Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(HideDialogue());
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerMovement2D movement = other.GetComponentInParent<PlayerMovement2D>();

        if (movement == null)
            return;

        if (!movement.CompareTag(playerTag))
            return;

        if (CoinManager.Instance == null)
            return;

        if (CoinManager.Instance.Coins >= requiredCoins)
            return;

        playerMovement = movement;
        playerRb = movement.GetComponent<Rigidbody2D>();
        playerAnimator = movement.GetComponent<Animator>();

        StartCoroutine(ShowDialogue());
    }

    private void CheckWallState()
    {
        if (blockingWall == null || CoinManager.Instance == null)
            return;

        bool hasEnoughCoins = CoinManager.Instance.Coins >= requiredCoins;

        blockingWall.SetActive(!hasEnoughCoins);
    }

    private IEnumerator ShowDialogue()
    {
        FreezePlayer();

        if (npcObject != null)
            npcObject.SetActive(true);

        if (messageObject != null)
            messageObject.SetActive(true);

        yield return new WaitForSeconds(delayBeforeClose);

        if (waitForSpace)
        {
            waitingForContinue = true;
        }
        else
        {
            yield return new WaitForSeconds(autoHideAfterSeconds);
            StartCoroutine(HideDialogue());
        }
    }

    private IEnumerator HideDialogue()
    {
        waitingForContinue = false;

        if (npcObject != null)
            npcObject.SetActive(false);

        if (messageObject != null)
            messageObject.SetActive(false);

        yield return null;

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