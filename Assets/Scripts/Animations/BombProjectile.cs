using UnityEngine;

public class BombProjectile : Projectile
{
    [Header("Bomb Settings")]
    public float arcHeight = 2f;
    private Vector3 startPosition;
    private float journeyLength;
    private float journeyTime = 0f;

    public override void Initialize(Vector3 target, int damage)
    {
        base.Initialize(target, damage);
        startPosition = transform.position;
        journeyLength = Vector3.Distance(startPosition, targetPosition);
    }

    protected override void MoveTowardsTarget()
    {
        journeyTime += Time.deltaTime * speed;
        float percentComplete = journeyTime / journeyLength;

        if (percentComplete >= 1f)
        {
            OnReachTarget();
            return;
        }

        // Linear movement
        Vector3 currentPos = Vector3.Lerp(startPosition, targetPosition, percentComplete);
        
        // Add arc (parabola)
        float arc = arcHeight * Mathf.Sin(percentComplete * Mathf.PI);
        currentPos.y += arc;

        transform.position = currentPos;

        // Rotate based on arc direction
        Vector3 direction = transform.position - startPosition;
        if (direction != Vector3.zero)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle - 90);
        }
    }

    protected override void DealDamage()
    {
        // Bomb deals splash damage
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 1f);  // 1 unit splash radius
        
        foreach (Collider2D hit in hits)
        {
            // Deal damage to enemies here when you implement them
            Debug.Log($"Bomb hit: {hit.gameObject.name}");
        }
    }
}