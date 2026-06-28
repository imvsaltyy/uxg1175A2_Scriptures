using UnityEngine;
using UnityEngine.EventSystems;

public class LevelNode : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("UI")]
    public GameObject highlightImage;

    [Header("Map")]
    public GameObject map;      // Drag your Map GameObject here

    [Header("Level")]
    public GameObject levelToActivate;

    [Header("All Levels")]
    public GameObject[] allLevels;

    private bool selected = false;

    private void Start()
    {
        if (highlightImage != null)
            highlightImage.SetActive(false);
    }

    // Hover
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (highlightImage != null)
            highlightImage.SetActive(true);
    }

    // Exit Hover
    public void OnPointerExit(PointerEventData eventData)
    {
        if (!selected && highlightImage != null)
            highlightImage.SetActive(false);
    }

    // Called by the Button OnClick()
    public void OpenLevel()
    {
        selected = true;

        if (highlightImage != null)
            highlightImage.SetActive(true);

        // Hide the map
        if (map != null)
            map.SetActive(false);

        // Turn off every level
        foreach (GameObject level in allLevels)
        {
            if (level != null)
                level.SetActive(false);
        }

        // Turn on the selected level
        if (levelToActivate != null)
            levelToActivate.SetActive(true);
    }
}