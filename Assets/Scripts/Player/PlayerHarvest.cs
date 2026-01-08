using UnityEngine;

public class PlayerHarvest : MonoBehaviour
{
    public ToolData currentTool;
    public float harvestRange = 0.4f;
    public LayerMask harvestLayer;
    
    [SerializeField] private PlayerSwingVisual swingVisual;

    private float nextHarvestTime;
    private bool canHarvestDuringSwing = true;  // Allow harvesting during current swing

    void Update()
    {
        // Swing happens regardless of targets
        if (Input.GetMouseButton(0))
        {
            if (Time.time >= nextHarvestTime)
            {
                Debug.Log($"Cooldown value: {currentTool.swingCooldown}");
                PerformSwing();
                nextHarvestTime = Time.time + currentTool.swingCooldown;
            }
        }
    }

    void PerformSwing()
    {
        // Reset harvest flag for this swing
        canHarvestDuringSwing = true;
        
        // Play swing visual with tool's cooldown
        if (swingVisual != null)
        {
            swingVisual.PlaySwing(currentTool.swingCooldown);
        }
    }

    // Called by SwingWeapon script when weapon hits something
    public void OnWeaponHit(Collider2D hit)
    {
        // Only harvest once per swing
        if (!canHarvestDuringSwing)
            return;

        Harvestable harvestable = hit.GetComponent<Harvestable>();

        if (harvestable != null)
        {
            harvestable.Harvest(currentTool);
            canHarvestDuringSwing = false;  // Prevent multiple harvests in one swing
        }
    }

    // Debug helper
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, harvestRange);
    }
}