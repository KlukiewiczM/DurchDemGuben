using System.Collections;
using UnityEngine;

public class BuildAtFinish : MonoBehaviour
{
    [SerializeField] private string playerTag = "PlayerTag";
    [SerializeField] private int requiredCoins = 3;

    [Header("Objects to show")]
    [SerializeField] private GameObject buildingPlaceholder;
    [SerializeField] private GameObject builderNpc;
    [SerializeField] private GameObject buildMessage;

    [Header("Player control")]
    [SerializeField] private MonoBehaviour playerMovementScript;

    [Header("Timing")]
    [SerializeField] private float delayBeforeNpc = 2f;
    [SerializeField] private float delayBeforeMessage = 1f;

    private bool alreadyBuilt = false;
    private bool waitingForContinue = false;

    private void Update()
    {
        if (waitingForContinue && Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(HideSequence());
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (alreadyBuilt) return;
        if (!other.CompareTag(playerTag)) return;
        if (CoinManager.Instance == null) return;

        bool success = CoinManager.Instance.SpendCoins(requiredCoins);

        if (success)
        {
            alreadyBuilt = true;
            StartCoroutine(PlayBuildSequence());
        }
        else
        {
            Debug.Log("Za mało monet, żeby postawić budynek.");
        }
    }

    private IEnumerator PlayBuildSequence()
    {
        if (playerMovementScript != null)
            playerMovementScript.enabled = false;

        yield return new WaitForSeconds(delayBeforeNpc);

        if (builderNpc != null)
            builderNpc.SetActive(true);

        yield return new WaitForSeconds(delayBeforeMessage);

        if (buildMessage != null)
            buildMessage.SetActive(true);

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

        if (playerMovementScript != null)
            playerMovementScript.enabled = true;

        Debug.Log("Sekwencja zakończona. Gracz może iść dalej.");
    }
}