using UnityEngine;
using System.Collections;

public class PlayerHarvest : MonoBehaviour
{
    public ToolData currentTool;
    public float harvestRange = 1.5f;
    public LayerMask harvestableLayer;

    private bool isHarvesting = false;
    private Coroutine harvestCoroutine;

    void Update()
    {
        // Start harvesting when mouse is held
        if (Input.GetMouseButton(0))
        {
            if (!isHarvesting)
            {
                harvestCoroutine = StartCoroutine(HarvestLoop());
            }
        }
        else
        {
            // Stop harvesting when mouse released
            StopHarvesting();
        }
    }

    IEnumerator HarvestLoop()
    {
        isHarvesting = true;

        while (Input.GetMouseButton(0))
        {
            TryHarvest();
            yield return new WaitForSeconds(currentTool.swingCooldown);
        }

        isHarvesting = false;
    }

    void StopHarvesting()
    {
        if (harvestCoroutine != null)
        {
            StopCoroutine(harvestCoroutine);
            harvestCoroutine = null;
        }
        isHarvesting = false;
    }

    void TryHarvest()
    {
        Collider2D hit = Physics2D.OverlapCircle(
            transform.position,
            harvestRange,
            harvestableLayer
        );

        if (hit == null) return;

        Harvestable harvestable = hit.GetComponent<Harvestable>();
        if (harvestable == null) return;

        harvestable.Harvest(currentTool);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, harvestRange);
    }
}
