using UnityEngine;

public class BuildAtFinish : MonoBehaviour
{
    [SerializeField] private string playerTag = "PlayerTag";
    [SerializeField] private int requiredCoins = 3;
    [SerializeField] private GameObject buildingPlaceholder;

    private bool alreadyBuilt = false;

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

            alreadyBuilt = true;
            Debug.Log("Budynek postawiony!");
        }
        else
        {
            Debug.Log("Za mało monet, żeby postawić budynek.");
        }
    }

}
