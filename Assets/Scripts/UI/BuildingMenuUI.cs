using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class BuildingMenuUI : MonoBehaviour
{
    public static BuildingMenuUI Instance;

    [Header("UI References")]
    public GameObject menuPanel;
    public Transform buttonContainer;

    [Header("Button Prefab")]
    public GameObject buildingButtonPrefab;

    private Dictionary<BuildingType, BuildingButton> buildingButtons = new Dictionary<BuildingType, BuildingButton>();
    private BuildingType currentlySelected = BuildingType.Core;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        if (menuPanel != null)
            menuPanel.SetActive(false);
    }

    public void ShowMenu(bool show)
    {
        if (menuPanel != null)
            menuPanel.SetActive(show);

        // Update axe UI when menu is shown
        if (show && AxeUpgradeUI.Instance != null)
        {
            AxeUpgradeUI.Instance.UpdateUI();
        }
    }

    public void SetupBuildingButtons(List<BuildingInfo> buildings)
    {
        foreach (Transform child in buttonContainer)
        {
            Destroy(child.gameObject);
        }
        buildingButtons.Clear();

        foreach (BuildingInfo info in buildings)
        {
            GameObject buttonObj = Instantiate(buildingButtonPrefab, buttonContainer);
            BuildingButton btn = buttonObj.GetComponent<BuildingButton>();

            if (btn != null)
            {
                btn.Setup(info);
                buildingButtons[info.buildingType] = btn;
            }
        }

        // Add axe upgrade button at the end
        if (AxeUpgradeUI.Instance != null)
        {
            GameObject axeButtonObj = Instantiate(buildingButtonPrefab, buttonContainer);
            BuildingButton axeBtn = axeButtonObj.GetComponent<BuildingButton>();

            if (axeBtn != null)
            {
                AxeUpgradeUI.Instance.axeButton = axeBtn;
                AxeUpgradeUI.Instance.UpdateUI();

                // Make it clickable
                Button buttonComponent = axeButtonObj.GetComponent<Button>();
                if (buttonComponent != null)
                {
                    buttonComponent.onClick.AddListener(AxeUpgradeUI.Instance.OnAxeClicked);
                }
            }
        }
    }

    public void SetSelectedBuilding(BuildingType type)
    {
        currentlySelected = type;

        foreach (var kvp in buildingButtons)
        {
            kvp.Value.SetSelected(kvp.Key == type);
        }
    }

    public void UpdateBuildingAffordability(BuildingType type, bool canAfford)
    {
        if (buildingButtons.ContainsKey(type))
        {
            buildingButtons[type].SetAffordable(canAfford);
        }
    }

    public void SetInstructionText(string text)
    {
        // No longer used, but keeping for compatibility
    }
}
