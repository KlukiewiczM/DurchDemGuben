using System.Collections;
using UnityEngine;

public class BuildAtFinish : MonoBehaviour
{
    [SerializeField] private string playerTag = "PlayerTag";
    [SerializeField] private int requiredCoins = 3;

    [Header("Objects to show")]
    [SerializeField] private GameObject constructionObject;
    [SerializeField] private GameObject buildingPlaceholder;
    [SerializeField] private GameObject buildAnimationObject;
    [SerializeField] private GameObject flashObject;
    [SerializeField] private GameObject builderNpc;
    [SerializeField] private GameObject buildMessage;

    [Header("Audio")]
    [SerializeField] private AudioClip buildSound;
    [SerializeField] private float buildSoundVolume = 1f;

    [Header("Timing")]
    [SerializeField] private float delayBeforeNpc = 2f;
    [SerializeField] private float delayBeforeMessage = 1f;
    [SerializeField] private float buildAnimationDuration = 1.2f;
    [SerializeField] private float flashDuration = 0.15f;
    [SerializeField] private float buildingFadeDuration = 0.6f;

    private bool alreadyBuilt = false;
    private bool waitingForContinue = false;

    private PlayerMovement2D playerMovement;
    private Rigidbody2D playerRb;
    private Animator playerAnimator;

    private SpriteRenderer buildingRenderer;

    private void Start()
    {
        if (buildingPlaceholder != null)
        {
            buildingRenderer = buildingPlaceholder.GetComponent<SpriteRenderer>();
            SetBuildingAlpha(0f);
            buildingPlaceholder.SetActive(false);
        }

        if (constructionObject != null)
            constructionObject.SetActive(true);

        if (buildAnimationObject != null)
            buildAnimationObject.SetActive(false);

        if (flashObject != null)
            flashObject.SetActive(false);

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

        if (buildingPlaceholder != null)
        {
            SetBuildingAlpha(0f);
            buildingPlaceholder.SetActive(false);
        }

        if (buildAnimationObject != null)
            buildAnimationObject.SetActive(true);

        PlayBuildSound();

        yield return new WaitForSeconds(buildAnimationDuration);

        if (buildAnimationObject != null)
            buildAnimationObject.SetActive(false);

        if (flashObject != null)
        {
            flashObject.SetActive(true);
            yield return new WaitForSeconds(flashDuration);
            flashObject.SetActive(false);
        }

        if (constructionObject != null)
            constructionObject.SetActive(false);

        if (buildingPlaceholder != null)
        {
            buildingPlaceholder.SetActive(true);
            yield return StartCoroutine(FadeInBuilding());
        }

        waitingForContinue = true;
    }

    private IEnumerator FadeInBuilding()
    {
        if (buildingRenderer == null)
            yield break;

        float timer = 0f;

        while (timer < buildingFadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Clamp01(timer / buildingFadeDuration);
            SetBuildingAlpha(alpha);
            yield return null;
        }

        SetBuildingAlpha(1f);
    }

    private void SetBuildingAlpha(float alpha)
    {
        if (buildingRenderer == null) return;

        Color color = buildingRenderer.color;
        color.a = alpha;
        buildingRenderer.color = color;
    }

    private void PlayBuildSound()
    {
        if (buildSound == null || Camera.main == null) return;

        AudioSource.PlayClipAtPoint(
            buildSound,
            Camera.main.transform.position,
            buildSoundVolume
        );
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