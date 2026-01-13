using UnityEngine;

public class CannonballProjectile : BombProjectile
{
    [Header("Cannon Settings")]
    public float splashRadius = 1.5f;  // Larger splash than bomb

    protected override void DealDamage()
    {
        // Cannonball deals larger splash damage
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, splashRadius);
        
        foreach (Collider2D hit in hits)
        {
            // Deal damage to enemies here when you implement them
            Debug.Log($"Cannonball hit: {hit.gameObject.name}");
        }
    }
}