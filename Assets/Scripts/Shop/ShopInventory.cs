using TMPro;
using UnityEngine;

// Single source of truth for currency — reads and writes to PlayerManager.
public class ShopInventory : MonoBehaviour
{
    public static ShopInventory Instance;

    [SerializeField] private TMP_Text currencyText;

    public void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        UpdateCurrencyUI();
    }

    // Called every time currency changes so the UI stays in sync
    public void UpdateCurrencyUI()
    {
        if (currencyText != null && PlayerManager.Instance != null)
            currencyText.text = "$" + PlayerManager.Instance.currency;
    }

    public void SetCurrencyText(TMP_Text text)
    {
        currencyText = text;
        UpdateCurrencyUI();
    }

    public void AddCurrency(int amount)
    {
        if (PlayerManager.Instance == null) return;
        PlayerManager.Instance.AddCurrency(amount);
        UpdateCurrencyUI();
    }

    public bool SpendCurrency(int amount)
    {
        if (PlayerManager.Instance == null) return false;
        if (PlayerManager.Instance.currency < amount) return false;
        PlayerManager.Instance.currency -= amount;
        UpdateCurrencyUI();
        return true;
    }

    public int GetCurrency()
    {
        return PlayerManager.Instance != null ? PlayerManager.Instance.currency : 0;
    }
}
