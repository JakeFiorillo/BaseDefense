using UnityEngine;

public class LaserTower : DefenseTower
{
    // Damage by upgrade level
    private int[] damageByLevel = { 2, 2, 3, 3, 4, 4 };  // Low damage but fast attack
    
    // HP by upgrade level
    private int[] healthByLevel = { 950, 1000, 1050, 1100, 1200, 1300 };

    protected override void Start()
    {
        buildingType = BuildingType.LaserTower;
        buildingName = "Laser Tower";
        
        baseDamage = damageByLevel[upgradeLevel];
        maxHealth = healthByLevel[upgradeLevel];
        attackRange = 6f;
        attackCooldown = 0.5f;  // Fast attack speed
        
        base.Start();
    }

    protected override void Attack()
    {
        if (projectilePrefab != null && currentTarget != null)
        {
            GameObject laser = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
            LaserBeam beam = laser.GetComponent<LaserBeam>();
            
            if (beam != null)
            {
                beam.Initialize(firePoint.position, currentTarget.position, baseDamage);
            }
        }
    }
}