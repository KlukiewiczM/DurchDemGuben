using System.Collections;
using UnityEngine;

public class BuildAtFinish : MonoBehaviour
{
    [SerializeField] private string playerTag = "PlayerTag";
    [SerializeField] private int requiredCoins = 3;

    [Header("Objects to show")]
    [SerializeField] private GameObject buildingPlaceholder;
    [SerializeField] private GameObject buildAnimationObject;
    [SerializeField] private GameObject builderNpc;
    [SerializeField] private GameObject buildMessage;

    [Header("Timing")]
    [SerializeField] private float delayBeforeNpc = 2f;
    [SerializeField] private float delayBeforeMessage = 1f;
    [SerializeField] private float buildAnimationDuration = 1.2f;

    private bool alreadyBuilt = false;
    private bool waitingForContinue = false;

    private PlayerMovement2D playerMovement;
    private Rigidbody2D playerRb;
    private Animator playerAnimator;

    private void Start()
    {
        if (buildingPlaceholder != null)
            buildingPlaceholder.SetActive(false);

        if (buildAnimationObject != null)
            buildAnimationObject.SetActive(false);

        if (builderNpc != null)
            builderNpc.SetActive(false);

        if (buildMessage != null)
            buildMessage.SetActive(false);
    }

    private void Update()
    {
        if (waitingForContinue && Input.GetKeyDown(KeyCode.Space))
            StartCoroutine(HideSequence());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (alreadyBuilt) return;

        PlayerMovement2D movement = other.GetComponentInParent<PlayerMovement2D>();
        if (movement == null) return;
        if (!movement.CompareTag(playerTag)) return;
        if (CoinManager.Instance == null) return;

        if (!CoinManager.Instance.SpendCoins(requiredCoins))
        {
            Debug.Log("Za mało monet, żeby postawić budynek.");
            return;
        }

        playerMovement = movement;
        playerRb = movement.GetComponent<Rigidbody2D>();
        playerAnimator = movement.GetComponent<Animator>();

        alreadyBuilt = true;
        StartCoroutine(PlayBuildSequence());
    }

    private IEnumerator PlayBuildSequence()
    {
        FreezePlayer();

        yield return new WaitForSeconds(delayBeforeNpc);

        if (builderNpc != null)
            builderNpc.SetActive(true);

        yield return new WaitForSeconds(delayBeforeMessage);

        if (buildMessage != null)
            buildMessage.SetActive(true);

        if (buildAnimationObject != null)
            buildAnimationObject.SetActive(true);

        yield return new WaitForSeconds(buildAnimationDuration);

        if (buildAnimationObject != null)
            buildAnimationObject.SetActive(false);

        if (buildingPlaceholder != null)
            buildingPlaceholder.SetActive(true);

        waitingForContinue = true;

        Debug.Log("Budynek postawiony. Czekam na spację.");
    }

    private IEnumerator HideSequence()
    {
        waitingForContinue = false;

        if (builderNpc != null)
            builderNpc.SetActive(false);

        if (buildMessage != null)
            buildMessage.SetActive(false);

        yield return null;

        UnfreezePlayer();

        Debug.Log("Sekwencja zakończona. Gracz może iść dalej.");
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