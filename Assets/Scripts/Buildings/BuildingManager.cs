using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    public static BuildingManager Instance;
    
    [Header("Building Prefabs")]
    public GameObject corePrefab;
    public GameObject goldGeneratorPrefab;
    
    private bool coreHasBeenPlaced = false;
    private List<Building> allBuildings = new List<Building>();
    
    // Track how many gold generators have been built (for pricing)
    private int goldGeneratorsBuiltCount = 0;
    private int[] goldGeneratorCosts = { 0, 100, 500, 1200 };  // Free, 100, 500, 1200
    
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    
    public bool HasCorePlaced()
    {
        return coreHasBeenPlaced;
    }
    
    public void OnCorePlaced()
    {
        coreHasBeenPlaced = true;
        Debug.Log("Core has been placed! You can now build other structures.");
    }
    
    public int GetGoldGeneratorCost()
    {
        // Return cost based on how many have been built
        if (goldGeneratorsBuiltCount < goldGeneratorCosts.Length)
        {
            return goldGeneratorCosts[goldGeneratorsBuiltCount];
        }
        else
        {
            // After 4th, it costs 1200
            return 1200;
        }
    }
    
    public bool CanAffordGoldGenerator()
    {
        int cost = GetGoldGeneratorCost();
        return Inventory.Instance.GetResource(ResourceType.Gold) >= cost;
    }
    
    public void OnGoldGeneratorPlaced()
    {
        goldGeneratorsBuiltCount++;
    }
    
    public void OnBuildingDestroyed(Building building)
    {
        allBuildings.Remove(building);
        
        // If a gold generator was destroyed, decrement count so it can be rebought at same price
        if (building.buildingType == BuildingType.GoldGenerator)
        {
            goldGeneratorsBuiltCount--;
            Debug.Log($"Gold generator destroyed. Can rebuy for {GetGoldGeneratorCost()} gold");
        }
        
        // If core was destroyed - game over
        if (building.buildingType == BuildingType.Core)
        {
            coreHasBeenPlaced = false;  // Technically, but game should be over
        }
    }
    
    public void RegisterBuilding(Building building)
    {
        if (!allBuildings.Contains(building))
        {
            allBuildings.Add(building);
        }
    }
}