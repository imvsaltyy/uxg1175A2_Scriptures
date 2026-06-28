using UnityEngine;
using UnityEngine.EventSystems;

public class WeaponHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("This Weapon")]
    public GameObject verticalImage;
    public GameObject horizontalImage;

    [Header("Other Weapon")]
    public GameObject otherVerticalImage;
    public GameObject otherHorizontalImage;

    private void Start()
    {
        verticalImage.SetActive(true);
        horizontalImage.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Show this weapon horizontally
        verticalImage.SetActive(false);
        horizontalImage.SetActive(true);

        // Show the other weapon vertically
        otherVerticalImage.SetActive(true);
        otherHorizontalImage.SetActive(false);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Return BOTH weapons to their default vertical state
        verticalImage.SetActive(true);
        horizontalImage.SetActive(false);

        otherVerticalImage.SetActive(true);
        otherHorizontalImage.SetActive(false);
    }
}