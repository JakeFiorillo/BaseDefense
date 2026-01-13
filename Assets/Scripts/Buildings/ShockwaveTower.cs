using UnityEngine;

public class ShockwaveTower : DefenseTower
{
    [Header("Shockwave Settings")]
    public float shockwaveRadius = 3f;
    
    // Damage by upgrade level
    private int[] damageByLevel = { 1, 2, 2, 3, 3, 4 };  // Low damage but AOE
    
    // HP by upgrade level
    private int[] healthByLevel = { 1000, 1100, 1200, 1300, 1350, 1400 };

    protected override void Start()
    {
        buildingType = BuildingType.ShockwaveTower;
        buildingName = "Shockwave Tower";
        
        baseDamage = damageByLevel[upgradeLevel];
        maxHealth = healthByLevel[upgradeLevel];
        attackRange = 3f;
        attackCooldown = 2f;
        
        base.Start();
    }

    protected override void Attack()
    {
        if (projectilePrefab != null)
        {
            GameObject effect = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            ShockwaveEffect shockwave = effect.GetComponent<ShockwaveEffect>();
            
            if (shockwave != null)
            {
                shockwave.Initialize(baseDamage, shockwaveRadius);
            }
        }
    }
}