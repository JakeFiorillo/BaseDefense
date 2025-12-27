using UnityEngine;
using System.Collections;

public class PlayerHarvest : MonoBehaviour
{
    public ToolData currentTool;
    public float harvestRange = 1.5f;
    public LayerMask harvestLayer;

    private bool isSwinging = false;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isSwinging)
        {
            StartCoroutine(Swing());
        }
    }

    IEnumerator Swing()
    {
        isSwinging = true;

        // Small delay = swing time
        yield return new WaitForSeconds(currentTool.swingTime);

        RaycastHit2D hit = Physics2D.Raycast(
            transform.position,
            GetMouseDirection(),
            harvestRange,
            harvestLayer
        );

        if (hit.collider != null)
        {
            Harvestable harvestable = hit.collider.GetComponent<Harvestable>();
            if (harvestable != null)
            {
                HandleHarvest(harvestable);
            }
        }

        isSwinging = false;
    }

    void HandleHarvest(Harvestable harvestable)
    {
        int amount = 0;

        if (harvestable.harvestType == HarvestType.Tree)
        {
            amount = currentTool.GetTreeYield();
            if (amount > 0)
                Debug.Log($"Collected {amount} wood");
        }
        else if (harvestable.harvestType == HarvestType.Rock)
        {
            amount = currentTool.GetRockYield();
            if (amount > 0)
                Debug.Log($"Collected {amount} stone");
        }

        // Later: add to inventory here
    }

    Vector2 GetMouseDirection()
    {
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return (mouseWorld - transform.position).normalized;
    }
}
