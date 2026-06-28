using TMPro;
using UnityEngine;

public class ShopInventory : MonoBehaviour
{
    public static ShopInventory Instance;

    [Header("Currency")]
    public int currentCurrency = 0;

    [SerializeField] private TMP_Text currencyText;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        UpdateCurrencyUI();
    }

    public void AddCurrency(int amount)
    {
        currentCurrency += amount;
        UpdateCurrencyUI();
    }

    public bool SpendCurrency(int amount)
    {
        if (currentCurrency < amount)
            return false;

        currentCurrency -= amount;
        UpdateCurrencyUI();
        return true;
    }

    public int GetCurrency()
    {
        return currentCurrency;
    }

    public void SetCurrencyText(TMP_Text text)
    {
        currencyText = text;
        UpdateCurrencyUI();
    }

    private void UpdateCurrencyUI()
    {
        if (currencyText != null)
            currencyText.text = "$" + currentCurrency;
    }
}