using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelNode : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("UI")]
    public GameObject highlightImage;
    public Button button;

    [Header("Map")]
    public GameObject map;

    [Header("Level")]
    public GameObject levelToActivate;

    [Header("All Levels")]
    public GameObject[] allLevels;

    [Header("Path")]
    public bool unlocked = false;
    public bool cleared = false;
    public LevelNode[] nextNodes;

    public static LevelNode currentNode;

    private void Start()
    {
        if (highlightImage != null)
            highlightImage.SetActive(false);

        UpdateButton();
    }

    public void OpenLevel()
    {
        if (!unlocked || cleared)
        {
            Debug.Log("This node is locked or already cleared.");
            return;
        }

        currentNode = this;

        if (map != null)
            map.SetActive(false);

        foreach (GameObject level in allLevels)
        {
            if (level != null)
                level.SetActive(false);
        }

        if (levelToActivate != null)
            levelToActivate.SetActive(true);
    }

    public void ClearNode()
    {
        cleared = true;
        unlocked = false;

        for (int i = 0; i < nextNodes.Length; i++)
        {
            if (nextNodes[i] != null && !nextNodes[i].cleared)
            {
                nextNodes[i].unlocked = true;
                nextNodes[i].UpdateButton();
            }
        }

        UpdateButton();
    }

    public void UpdateButton()
    {
        if (button != null)
        {
            button.interactable = unlocked && !cleared;
        }

        if (highlightImage != null)
        {
            highlightImage.SetActive(false);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!unlocked || cleared) return;

        if (highlightImage != null)
            highlightImage.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (highlightImage != null)
            highlightImage.SetActive(false);
    }
}