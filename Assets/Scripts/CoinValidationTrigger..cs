using System.Collections;
using UnityEngine;

public class CoinValidationTrigger : MonoBehaviour
{
    [SerializeField] private string playerTag = "PlayerTag";
    [SerializeField] private int requiredCoins = 3;

    [Header("Objects when not enough coins")]
    [SerializeField] private GameObject mayorObject;
    [SerializeField] private GameObject messageObject;

    [Header("Blocking")]
    [SerializeField] private GameObject blockingWall;

    private bool messageActive = false;

    private PlayerMovement2D playerMovement;
    private Rigidbody2D playerRb;
    private Animator playerAnimator;

    private void Start()
    {
        if (mayorObject != null)
            mayorObject.SetActive(false);

        if (messageObject != null)
            messageObject.SetActive(false);

        UpdateWallState();
    }

    private void Update()
    {
        UpdateWallState();

        if (messageActive && Input.GetKeyDown(KeyCode.Space))
            StartCoroutine(HideMessage());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerMovement2D movement = other.GetComponentInParent<PlayerMovement2D>();
        if (movement == null) return;
        if (!movement.CompareTag(playerTag)) return;
        if (CoinManager.Instance == null) return;

        playerMovement = movement;
        playerRb = movement.GetComponent<Rigidbody2D>();
        playerAnimator = movement.GetComponent<Animator>();

        if (CoinManager.Instance.Coins >= requiredCoins)
        {
            DeactivateWallAndTrigger();
            return;
        }

        if (!messageActive)
            StartCoroutine(ShowNotEnoughCoinsMessage());
    }

    private void UpdateWallState()
    {
        if (CoinManager.Instance == null) return;

        if (CoinManager.Instance.Coins >= requiredCoins)
        {
            DeactivateWallAndTrigger();
        }
        else
        {
            if (blockingWall != null)
                blockingWall.SetActive(true);
        }
    }

    private void DeactivateWallAndTrigger()
    {
        if (blockingWall != null)
            blockingWall.SetActive(false);

        gameObject.SetActive(false);
    }

    private IEnumerator ShowNotEnoughCoinsMessage()
    {
        messageActive = true;

        FreezePlayer();

        if (mayorObject != null)
            mayorObject.SetActive(true);

        if (messageObject != null)
            messageObject.SetActive(true);

        yield return null;
    }

    private IEnumerator HideMessage()
    {
        messageActive = false;

        if (mayorObject != null)
            mayorObject.SetActive(false);

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