using UnityEngine;

public class Harvestable : MonoBehaviour
{
    public ResourceType resourceType;

    public void Harvest(ToolData tool)
    {
        int amount = tool.GetHarvestAmount();

        if (amount <= 0)
            return;

        InventoryManager.Instance.AddResource(resourceType, amount);
    }
}
