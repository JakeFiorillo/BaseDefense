using UnityEngine;

public class GoldGenerator : Building
{
    [Header("Gold Generation")]
    public float goldPerSecond = 1f;  // Wooden = 1/s
    private float goldAccumulator = 0f;
    
    // Gold per second by upgrade level
    private float[] goldRates = { 1f, 1.5f, 2f, 2.5f, 3f, 4f };  // Wooden through Ruby
    
    // HP by upgrade level
    private int[] healthByLevel = { 600, 650, 700, 750, 800, 850 };
    
    protected override void Start()
    {
        buildingType = BuildingType.GoldGenerator;
        buildingName = "Gold Generator";
        
        // Set stats based on upgrade level
        goldPerSecond = goldRates[upgradeLevel];
        maxHealth = healthByLevel[upgradeLevel];
        
        base.Start();
    }
    
    void Update()
    {
        GenerateGold();
    }
    
    void GenerateGold()
    {
        goldAccumulator += goldPerSecond * Time.deltaTime;
        
        if (goldAccumulator >= 1f)
        {
            int goldToAdd = Mathf.FloorToInt(goldAccumulator);
            goldAccumulator -= goldToAdd;
            
            // Add gold to inventory
            if (Inventory.Instance != null)
            {
                Inventory.Instance.AddResource(ResourceType.Gold, goldToAdd);
            }
        }
    }
}