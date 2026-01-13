using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Projectile Settings")]
    public float speed = 5f;
    public int damage = 10;
    public GameObject impactEffect;  // Explosion prefab
    
    protected Vector3 targetPosition;
    protected Transform targetTransform;
    protected bool hasTarget = false;

    public virtual void Initialize(Vector3 target, int damage)
    {
        this.targetPosition = target;
        this.damage = damage;
        hasTarget = true;
    }

    public virtual void Initialize(Transform target, int damage)
    {
        this.targetTransform = target;
        this.damage = damage;
        hasTarget = true;
    }

    protected virtual void Update()
    {
        if (!hasTarget) return;

        MoveTowardsTarget();
    }

    protected virtual void MoveTowardsTarget()
    {
        Vector3 target = targetTransform != null ? targetTransform.position : targetPosition;
        
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        // Rotate to face direction
        Vector3 direction = (target - transform.position).normalized;
        if (direction != Vector3.zero)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }

        // Check if reached target
        if (Vector3.Distance(transform.position, target) < 0.1f)
        {
            OnReachTarget();
        }
    }

    protected virtual void OnReachTarget()
    {
        // Spawn impact effect if exists
        if (impactEffect != null)
        {
            GameObject effect = Instantiate(impactEffect, transform.position, Quaternion.identity);
            Destroy(effect, 1f);  // Destroy effect after 1 second
        }

        DealDamage();
        Destroy(gameObject);
    }

    protected virtual void DealDamage()
    {
        // Override in child classes for specific damage behavior
    }
}