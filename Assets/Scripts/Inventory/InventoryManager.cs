using UnityEngine;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    private Dictionary<ResourceType, int> resources = new();

    void Awake()
    {
        Instance = this;
    }

    public void AddResource(ResourceType type, int amount)
    {
        if (!resources.ContainsKey(type))
            resources[type] = 0;

        resources[type] += amount;
        Debug.Log($"{type} +{amount} (Total: {resources[type]})");

        InventoryUI.Instance.UpdateUI(type, resources[type]);
    }
}
