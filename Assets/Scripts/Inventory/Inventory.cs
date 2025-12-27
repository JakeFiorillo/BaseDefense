using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;

    private Dictionary<ResourceType, int> resources =
        new Dictionary<ResourceType, int>();

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        // Initialize all resource types at 0
        foreach (ResourceType type in System.Enum.GetValues(typeof(ResourceType)))
        {
            resources[type] = 0;
        }
    }

    public void AddResource(ResourceType type, int amount)
    {
        resources[type] += amount;
        InventoryUI.Instance.UpdateUI(type, resources[type]);
    }

    public int GetResource(ResourceType type)
    {
        return resources[type];
    }
}
