using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AxeUpgradeUI : MonoBehaviour
{
    public static AxeUpgradeUI Instance;

    [Header("UI References")]
    public BuildingButton axeButton;

    [Header("Axe Icons")]
    public Sprite woodenAxeIcon;
    public Sprite stoneAxeIcon;
    public Sprite goldAxeIcon;
    public Sprite emeraldAxeIcon;
    public Sprite diamondAxeIcon;
    public Sprite rubyAxeIcon;

    private int currentTier = 0;
    private string[] tierNames = { "Wooden", "Stone", "Gold", "Emerald", "Diamond", "Ruby" };
    
    // Upgrade costs [gold, wood, stone]
    private int[,] upgradeCosts = {
        // Gold, Wood, Stone
        { 300, 20, 25 },    // Wooden -> Stone
        { 600, 45, 60 },    // Stone -> Gold
        { 1100, 75, 90 },   // Gold -> Emerald
        { 1750, 115, 130 }, // Emerald -> Diamond
        { 2500, 175, 175 }  // Diamond -> Ruby
    };

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void UpdateUI()
    {
        if (axeButton == null) return;

        BuildingInfo axeInfo = new BuildingInfo();
        
        if (currentTier >= tierNames.Length - 1)
        {
            // Max tier reached (Ruby)
            axeInfo.buildingType = BuildingType.Core;
            axeInfo.buildingName = tierNames[currentTier] + " Axe";
            axeInfo.hotkey = "U";
            axeInfo.cost = 0;
            axeInfo.icon = GetAxeIcon(currentTier);
            
            axeButton.Setup(axeInfo);
            
            if (axeButton.costText != null)
            {
                axeButton.costText.text = "MAX";
                axeButton.costText.color = Color.yellow;
            }
        }
        else
        {
            // Show next tier info
            int nextTier = currentTier + 1;
            axeInfo.buildingType = BuildingType.Core;
            axeInfo.buildingName = tierNames[nextTier];
            axeInfo.hotkey = "U";
            axeInfo.cost = 0; // We'll override the cost display
            axeInfo.icon = GetAxeIcon(nextTier);
            
            axeButton.Setup(axeInfo);
            
            // Custom cost display with all three resources
            if (axeButton.costText != null)
            {
                int gold = upgradeCosts[currentTier, 0];
                int wood = upgradeCosts[currentTier, 1];
                int stone = upgradeCosts[currentTier, 2];
                
                axeButton.costText.text = $"{gold}g {wood}w {stone}s";
                
                // Check affordability for all resources
                bool canAfford = CanAffordUpgrade();
                axeButton.costText.color = canAfford ? axeButton.canAffordColor : axeButton.cannotAffordColor;
            }
        }
    }

    bool CanAffordUpgrade()
    {
        if (currentTier >= tierNames.Length - 1)
            return false;

        int goldCost = upgradeCosts[currentTier, 0];
        int woodCost = upgradeCosts[currentTier, 1];
        int stoneCost = upgradeCosts[currentTier, 2];

        int currentGold = Inventory.Instance.GetResource(ResourceType.Gold);
        int currentWood = Inventory.Instance.GetResource(ResourceType.Wood);
        int currentStone = Inventory.Instance.GetResource(ResourceType.Stone);

        return currentGold >= goldCost && 
               currentWood >= woodCost && 
               currentStone >= stoneCost;
    }

    public void OnAxeClicked()
    {
        if (currentTier >= tierNames.Length - 1)
        {
            Debug.Log("Axe already at max tier (Ruby)!");
            return;
        }

        // Declare costs at the top of the method
        int goldCost = upgradeCosts[currentTier, 0];
        int woodCost = upgradeCosts[currentTier, 1];
        int stoneCost = upgradeCosts[currentTier, 2];

        if (!CanAffordUpgrade())
        {
            Debug.Log($"Not enough resources! Need {goldCost}g, {woodCost}w, {stoneCost}s");
            return;
        }

        // Deduct all costs
        Inventory.Instance.AddResource(ResourceType.Gold, -goldCost);
        Inventory.Instance.AddResource(ResourceType.Wood, -woodCost);
        Inventory.Instance.AddResource(ResourceType.Stone, -stoneCost);

        // Upgrade tier
        currentTier++;

        Debug.Log($"Upgraded axe to {tierNames[currentTier]}!");

        UpdateUI();
    }

    Sprite GetAxeIcon(int tier)
    {
        switch (tier)
        {
            case 0: return woodenAxeIcon;
            case 1: return stoneAxeIcon;
            case 2: return goldAxeIcon;
            case 3: return emeraldAxeIcon;
            case 4: return diamondAxeIcon;
            case 5: return rubyAxeIcon;
            default: return woodenAxeIcon;
        }
    }

    public int GetCurrentTier()
    {
        return currentTier;
    }

    public string GetCurrentTierName()
    {
        return tierNames[currentTier];
    }
}