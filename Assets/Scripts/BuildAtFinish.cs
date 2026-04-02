using UnityEngine;
using UnityEngine.SceneManagement;

public class BuildAtFinish : MonoBehaviour
{
    [SerializeField] private string playerTag = "PlayerTag";
    [SerializeField] private int requiredCoins = 3;

    [Header("Objects to show")]
    [SerializeField] private GameObject buildingPlaceholder;
    [SerializeField] private GameObject builderNpc;
    [SerializeField] private GameObject buildMessage;

    [Header("Next Scene")]
    [SerializeField] private string nextSceneName = "2LevelScene";

    private bool alreadyBuilt = false;
    private bool waitingForContinue = false;

    private void Update()
    {
        if (waitingForContinue && Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(nextSceneName);
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
            if (buildingPlaceholder != null)
                buildingPlaceholder.SetActive(true);

            if (builderNpc != null)
                builderNpc.SetActive(true);

            if (buildMessage != null)
                buildMessage.SetActive(true);

            alreadyBuilt = true;
            waitingForContinue = true;

            Debug.Log("Budynek postawiony. Czekam na spację.");
        }
        else
        {
            Debug.Log("Za mało monet, żeby postawić budynek.");
        }
    }

}
