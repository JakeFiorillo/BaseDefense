using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BuildingButton : MonoBehaviour
{
    [Header("UI References")]
    public Image backgroundImage;
    public Image iconImage;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI hotkeyText;
    public TextMeshProUGUI costText;
    public GameObject selectedIndicator;

    [Header("Colors")]
    public Color normalColor = new Color(0.2f, 0.2f, 0.2f, 0.8f);
    public Color selectedColor = new Color(0.3f, 0.5f, 0.3f, 0.9f);
    public Color canAffordColor = Color.white;
    public Color cannotAffordColor = Color.red;

    private BuildingInfo info;
    private Button button;

    void Awake()
    {
        button = GetComponent<Button>();
    }

    public void Setup(BuildingInfo buildingInfo)
    {
        info = buildingInfo;

        if (nameText != null)
            nameText.text = buildingInfo.buildingName;

        if (hotkeyText != null)
            hotkeyText.text = buildingInfo.hotkey;

        if (costText != null)
        {
            if (buildingInfo.cost == 0)
                costText.text = "FREE";
            else
                costText.text = $"{buildingInfo.cost}g";
        }

        if (iconImage != null && buildingInfo.icon != null)
            iconImage.sprite = buildingInfo.icon;

        SetSelected(false);
        SetAffordable(true);
    }

    public void SetSelected(bool selected)
    {
        if (selectedIndicator != null)
            selectedIndicator.SetActive(selected);

        if (backgroundImage != null)
            backgroundImage.color = selected ? selectedColor : normalColor;
    }

    public void SetAffordable(bool canAfford)
    {
        if (costText != null)
            costText.color = canAfford ? canAffordColor : cannotAffordColor;
    }
}