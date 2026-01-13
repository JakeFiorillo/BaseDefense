using UnityEngine;

public class BoltProjectile : Projectile
{
    protected override void DealDamage()
    {
        // Bolt deals single-target damage
        if (targetTransform != null)
        {
            // Deal damage to the target enemy when you implement them
            Debug.Log($"Bolt hit: {targetTransform.gameObject.name} for {damage} damage");
        }
    }
}