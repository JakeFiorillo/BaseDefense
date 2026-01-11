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
    public TextMeshProUGUI instructionText;

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
        // Hide menu by default
        if (menuPanel != null)
            menuPanel.SetActive(false);
    }

    public void ShowMenu(bool show)
    {
        if (menuPanel != null)
            menuPanel.SetActive(show);
    }

    public void SetupBuildingButtons(List<BuildingInfo> buildings)
    {
        // Clear existing buttons
        foreach (Transform child in buttonContainer)
        {
            Destroy(child.gameObject);
        }
        buildingButtons.Clear();

        // Create buttons for each building
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
    }

    public void SetSelectedBuilding(BuildingType type)
    {
        currentlySelected = type;

        // Update all button visuals
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
        if (instructionText != null)
        {
            instructionText.text = text;
        }
    }
}

[System.Serializable]
public class BuildingInfo
{
    public BuildingType buildingType;
    public string buildingName;
    public string hotkey;
    public int cost;
    public Sprite icon;
}