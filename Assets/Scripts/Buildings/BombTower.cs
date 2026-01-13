using UnityEngine;

public class BombTower : DefenseTower
{
    // Damage by upgrade level
    private int[] damageByLevel = { 35, 40, 45, 50, 55, 60 };
    
    // HP by upgrade level
    private int[] healthByLevel = { 1250, 1300, 1400, 1500, 1600, 1700 };

    protected override void Start()
    {
        buildingType = BuildingType.BombTower;
        buildingName = "Bomb Tower";
        
        baseDamage = damageByLevel[upgradeLevel];
        maxHealth = healthByLevel[upgradeLevel];
        attackRange = 5f;
        attackCooldown = 2f;
        
        base.Start();
    }

    protected override void Attack()
    {
        if (projectilePrefab != null && currentTarget != null)
        {
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
            BombProjectile bomb = projectile.GetComponent<BombProjectile>();
            
            if (bomb != null)
            {
                bomb.Initialize(currentTarget.position, baseDamage);
            }
        }
    }
}