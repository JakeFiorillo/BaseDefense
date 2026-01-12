using UnityEngine;

public class Wall : Building
{
    // HP by upgrade level (Wooden through Ruby)
    private int[] healthByLevel = { 150, 175, 200, 250, 300, 350 };
    
    protected override void Start()
    {
        buildingType = BuildingType.Wall;
        buildingName = "Wall";
        
        // Set stats based on upgrade level
        maxHealth = healthByLevel[upgradeLevel];
        
        base.Start();
    }
}