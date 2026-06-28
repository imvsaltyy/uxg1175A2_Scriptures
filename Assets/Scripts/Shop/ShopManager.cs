using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject buyPanel;
    public GameObject sellPanel;

    [Header("Buttons")]
    public Image buyButtonImage;
    public Image sellButtonImage;

    [Range(0f, 1f)]
    public float inactiveOpacity = 0.5f;

    [Header("Loot Description")]
    public GameObject descriptionPanel;
    public TMP_Text lootNameText;
    public TMP_Text lootDescriptionText;
    public Image lootImage;

    private void Start()
    {
        ShowBuyTab();

        // Hide description when opening the shop
        if (descriptionPanel != null)
            descriptionPanel.SetActive(false);
    }

    public void ShowBuyTab()
    {
        buyPanel.SetActive(true);
        sellPanel.SetActive(false);

        SetButtonOpacity(buyButtonImage, 1f);
        SetButtonOpacity(sellButtonImage, inactiveOpacity);
    }

    public void ShowSellTab()
    {
        buyPanel.SetActive(false);
        sellPanel.SetActive(true);

        SetButtonOpacity(buyButtonImage, inactiveOpacity);
        SetButtonOpacity(sellButtonImage, 1f);
    }

    private void SetButtonOpacity(Image buttonImage, float alpha)
    {
        Color color = buttonImage.color;
        color.a = alpha;
        buttonImage.color = color;
    }

    // Called when a loot item is clicked
    public void ShowLootDescription(string lootName, string description, Sprite lootSprite)
    {
        if (descriptionPanel != null)
            descriptionPanel.SetActive(true);

        if (lootNameText != null)
            lootNameText.text = lootName;

        if (lootDescriptionText != null)
            lootDescriptionText.text = description;

        if (lootImage != null)
            lootImage.sprite = lootSprite;
    }
}