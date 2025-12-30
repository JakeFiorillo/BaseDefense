using UnityEngine;

public class PlayerHarvest : MonoBehaviour
{
    public ToolData currentTool;
    public float harvestRange = 1.2f;
    public LayerMask harvestLayer;
    
    [SerializeField] private PlayerSwingVisual swingVisual;

    private float nextHarvestTime;

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if (Time.time >= nextHarvestTime)
            {
                Debug.Log($"Cooldown value: {currentTool.swingCooldown}");
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
    
        // Play swing visual with tool's cooldown
        if (swingVisual != null)
        {
            swingVisual.PlaySwing(currentTool.swingCooldown);
        }
    }
}