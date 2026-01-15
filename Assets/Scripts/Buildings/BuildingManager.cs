using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    public static BuildingManager Instance;
    
    [Header("Building Prefabs")]
    public GameObject corePrefab;
    public GameObject goldGeneratorPrefab;
    public GameObject wallPrefab;
    public GameObject bombTowerPrefab;
    public GameObject laserTowerPrefab;
    public GameObject boltTowerPrefab;
    public GameObject shockwaveTowerPrefab;
    public GameObject cannonTowerPrefab;
    
    private bool coreHasBeenPlaced = false;
    private List<Building> allBuildings = new List<Building>();
    
    // Track how many gold generators have been built (for pricing)
    private int goldGeneratorsBuiltCount = 0;
    private int[] goldGeneratorCosts = { 0, 100, 500, 1200 };
    
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
        if (goldGeneratorsBuiltCount < goldGeneratorCosts.Length)
        {
            return goldGeneratorCosts[goldGeneratorsBuiltCount];
        }
        else
        {
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
        
        if (building.buildingType == BuildingType.GoldGenerator)
        {
            goldGeneratorsBuiltCount--;
            Debug.Log($"Gold generator destroyed. Can rebuy for {GetGoldGeneratorCost()} gold");
        }
        
        if (building.buildingType == BuildingType.Core)
        {
            coreHasBeenPlaced = false;
        }
    }
    
    public void RegisterBuilding(Building building)
    {
        if (!allBuildings.Contains(building))
        {
            allBuildings.Add(building);
        }
    }
    
    // Helper method to get building costs
    public int GetBuildingCost(BuildingType type)
    {
        switch (type)
        {
            case BuildingType.Core:
                return 0;
            case BuildingType.GoldGenerator:
                return GetGoldGeneratorCost();
            case BuildingType.Wall:
                return 10;
            case BuildingType.BombTower:
                return 200;
            case BuildingType.LaserTower:
                return 250;
            case BuildingType.BoltTower:
                return 300;
            case BuildingType.ShockwaveTower:
                return 600;
            case BuildingType.CannonTower:
                return 400;
            default:
                return 0;
        }
    }
}