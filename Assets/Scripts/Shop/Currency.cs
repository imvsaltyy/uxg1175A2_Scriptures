using TMPro;
using UnityEngine;

public class CurrencyUI : MonoBehaviour
{
    [SerializeField] private TMP_Text currencyText;

    private void Start()
    {
        if (ShopInventory.Instance != null)
            ShopInventory.Instance.SetCurrencyText(currencyText);
    }

    private void OnEnable()
    {
        // Refresh whenever this panel becomes visible
        if (ShopInventory.Instance != null)
            ShopInventory.Instance.UpdateCurrencyUI();
    }
}
