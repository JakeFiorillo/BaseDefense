using UnityEngine;

public class PlayerHarvest : MonoBehaviour
{
    public ToolData currentTool;
    public float harvestRange = 1.2f;
    public LayerMask harvestLayer;

    private float nextHarvestTime;

    void Update()
{
    if (Input.GetMouseButton(0))
    {
        Debug.Log($"Time: {Time.time}, NextHarvest: {nextHarvestTime}, CanHarvest: {Time.time >= nextHarvestTime}");
        
        if (Time.time >= nextHarvestTime)
        {
            TryHarvest();
            nextHarvestTime = Time.time + currentTool.swingCooldown;
        }
    }
}
    void TryHarvest()
    {
        Collider2D hit = Physics2D.OverlapCircle(
            transform.position,
            harvestRange,
            harvestLayer
        );

        if (hit == null)
        {
            Debug.Log("No harvestable in range");
            return;
        }

        Harvestable harvestable = hit.GetComponent<Harvestable>();

        if (harvestable == null)
        {
            Debug.Log("Hit object but no Harvestable component");
            return;
        }

        harvestable.Harvest(currentTool);
    }

    // Debug helper
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, harvestRange);
    }
}
