using UnityEngine;

public class ShockwaveEffect : MonoBehaviour
{
    [Header("Shockwave Settings")]
    public float maxRadius = 3f;
    public float expandSpeed = 10f;
    public int damage = 10;
    public SpriteRenderer spriteRenderer;
    
    private float currentRadius = 0f;
    private bool hasDealtDamage = false;

    public void Initialize(int damage, float radius)
    {
        this.damage = damage;
        this.maxRadius = radius;
        
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        currentRadius += expandSpeed * Time.deltaTime;
        float scale = currentRadius / maxRadius;
        transform.localScale = Vector3.one * scale * maxRadius * 2;

        // Make it fade out as it expands
        if (spriteRenderer != null)
        {
            Color c = spriteRenderer.color;
            c.a = 1f - (currentRadius / maxRadius);
            spriteRenderer.color = c;
        }

        // Deal damage when reaching full size
        if (!hasDealtDamage && currentRadius >= maxRadius * 0.8f)
        {
            DealDamage();
            hasDealtDamage = true;
        }

        if (currentRadius >= maxRadius)
        {
            Destroy(gameObject);
        }
    }

    void DealDamage()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, maxRadius);
        
        foreach (Collider2D hit in hits)
        {
            // Deal damage to enemies here when you implement them
            Debug.Log($"Shockwave hit: {hit.gameObject.name} for {damage} damage");
        }
    }
}