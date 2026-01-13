using UnityEngine;

public class BoltTower : DefenseTower
{
    // Damage by upgrade level
    private int[] damageByLevel = { 50, 55, 60, 65, 70, 75 };
    
    // HP by upgrade level
    private int[] healthByLevel = { 1300, 1450, 1600, 1700, 1800, 1900 };

    protected override void Start()
    {
        buildingType = BuildingType.BoltTower;
        buildingName = "Bolt Tower";
        
        baseDamage = damageByLevel[upgradeLevel];
        maxHealth = healthByLevel[upgradeLevel];
        attackRange = 6f;
        attackCooldown = 1.5f;
        
        base.Start();
    }

    protected override void Attack()
    {
        if (projectilePrefab != null && currentTarget != null)
        {
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
            BoltProjectile bolt = projectile.GetComponent<BoltProjectile>();
            
            if (bolt != null)
            {
                bolt.Initialize(currentTarget, baseDamage);
            }
        }
    }
}