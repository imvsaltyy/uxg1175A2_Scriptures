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
}