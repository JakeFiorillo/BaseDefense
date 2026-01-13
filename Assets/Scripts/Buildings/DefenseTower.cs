using UnityEngine;

public class DefenseTower : Building
{
    [Header("Tower Stats")]
    public float attackRange = 5f;
    public float attackCooldown = 1f;
    public int baseDamage = 10;
    
    [Header("Projectile/Effect")]
    public GameObject projectilePrefab;
    public Transform firePoint;  // Where projectiles spawn from
    
    protected float nextAttackTime = 0f;
    protected Transform currentTarget = null;

    protected override void Start()
    {
        base.Start();
        
        // Set fire point to tower position if not set
        if (firePoint == null)
            firePoint = transform;
    }

    protected virtual void Update()
    {
        if (Time.time >= nextAttackTime)
        {
            FindTarget();
            
            if (currentTarget != null)
            {
                Attack();
                nextAttackTime = Time.time + attackCooldown;
            }
        }
    }

    protected virtual void FindTarget()
    {
        // Find closest enemy in range
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, attackRange);
        
        float closestDistance = Mathf.Infinity;
        Transform closestEnemy = null;

        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                float distance = Vector3.Distance(transform.position, hit.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestEnemy = hit.transform;
                }
            }
        }

        currentTarget = closestEnemy;
    }

    protected virtual void Attack()
    {
        // Override in child classes
    }

    // Draw attack range in editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}