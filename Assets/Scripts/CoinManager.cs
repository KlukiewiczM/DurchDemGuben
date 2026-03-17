using UnityEngine;
using TMPro;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance { get; private set; }

    [SerializeField] private TMP_Text coinsText;

    private int coins;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        UpdateUI();
    }

    public void AddCoin(int amount = 1)
    {
        coins += amount;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (coinsText != null)
            coinsText.text = "Coins: " + coins;
    }
}
