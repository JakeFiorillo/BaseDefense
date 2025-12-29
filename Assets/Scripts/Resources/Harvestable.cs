using UnityEngine;

public class Harvestable : MonoBehaviour
{
    public HarvestType harvestType;

    public void Harvest(ToolData tool)
    {
        int amount = 0;
        ResourceType resource;

        if (harvestType == HarvestType.Tree)
        {
            amount = tool.RollTreeYield();
            resource = ResourceType.Wood;
        }
        else // Rock
        {
            amount = tool.RollRockYield();
            resource = ResourceType.Stone;
        }

        if (amount > 0)
        {
            Inventory.Instance.AddResource(resource, amount);
        }
    }
}
