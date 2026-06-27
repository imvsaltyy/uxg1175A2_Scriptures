using UnityEngine;
using UnityEngine.UI;

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

    private void Start()
    {
        ShowBuyTab();
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
}