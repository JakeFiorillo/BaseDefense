using UnityEngine;

public class CannonTower : DefenseTower
{
    // Damage (Splash) by upgrade level
    private int[] damageByLevel = { 75, 80, 85, 90, 100, 110 };
    
    // HP by upgrade level
    private int[] healthByLevel = { 1200, 1300, 1400, 1500, 1600, 1700 };

    protected override void Start()
    {
        buildingType = BuildingType.CannonTower;
        buildingName = "Cannon Tower";
        
        baseDamage = damageByLevel[upgradeLevel];
        maxHealth = healthByLevel[upgradeLevel];
        attackRange = 6f;
        attackCooldown = 3f;  // Slow but powerful
        
        base.Start();
    }

    protected override void Attack()
    {
        if (projectilePrefab != null && currentTarget != null)
        {
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
            CannonballProjectile cannonball = projectile.GetComponent<CannonballProjectile>();
            
            if (cannonball != null)
            {
                cannonball.Initialize(currentTarget.position, baseDamage);
            }
        }
    }
}