using UnityEngine;

public class LaserBeam : MonoBehaviour
{
    [Header("Laser Settings")]
    public LineRenderer lineRenderer;
    public float duration = 0.2f;
    public int damage = 10;
    
    private float timer = 0f;

    public void Initialize(Vector3 start, Vector3 end, int damage)
    {
        this.damage = damage;
        
        if (lineRenderer == null)
            lineRenderer = GetComponent<LineRenderer>();

        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
        
        // Deal instant damage
        DealDamage(end);
    }

    void Update()
    {
        timer += Time.deltaTime;
        
        if (timer >= duration)
        {
            Destroy(gameObject);
        }
    }

    void DealDamage(Vector3 targetPos)
    {
        // Check if laser hit an enemy at target position
        Collider2D hit = Physics2D.OverlapPoint(targetPos);
        if (hit != null)
        {
            Debug.Log($"Laser hit: {hit.gameObject.name} for {damage} damage");
        }
    }
}