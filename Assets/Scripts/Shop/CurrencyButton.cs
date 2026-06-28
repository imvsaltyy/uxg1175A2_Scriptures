using UnityEngine;

public class CurrencyButton : MonoBehaviour
{
    public enum TransactionType
    {
        Buy,
        Sell
    }

    [Header("Transaction")]
    public TransactionType transactionType;

    [Header("Buy")]
    public int buyPrice = 100;
    public GameObject powerUpToUnlock;

    [Header("Sell")]
    public int sellPrice = 0;
    public GameObject lootToRemove;

    // Assign this to the Button's OnClick()
    public void ProcessTransaction()
    {
        if (ShopInventory.Instance == null)
        {
            Debug.LogError("ShopInventory not found!");
            return;
        }

        switch (transactionType)
        {
            case TransactionType.Buy:

                if (ShopInventory.Instance.SpendCurrency(buyPrice))
                {
                    if (powerUpToUnlock != null)
                        powerUpToUnlock.SetActive(true);

                    Debug.Log("Bought power-up for $" + buyPrice);
                }

                break;

            case TransactionType.Sell:

                ShopInventory.Instance.AddCurrency(sellPrice);

                if (lootToRemove != null)
                    lootToRemove.SetActive(false);

                Debug.Log("Sold loot for $" + sellPrice);

                break;
        }
    }
}