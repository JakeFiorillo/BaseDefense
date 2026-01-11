using UnityEngine;

public class CoreBuilding : Building
{
    protected override void Start()
    {
        buildingType = BuildingType.Core;
        buildingName = "Core";
        maxHealth = 10000;
        base.Start();
    }
    
    protected override void OnDestroyed()
    {
        Debug.Log("GAME OVER - Core Destroyed!");
        // You'll add game over logic later
        base.OnDestroyed();
    }
}