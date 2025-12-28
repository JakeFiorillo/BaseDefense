using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    public int wood;
    public int stone;
    public int gold;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void AddResource(ResourceType type, int amount)
    {
        switch (type)
        {
            case ResourceType.Wood:
                wood += amount;
                break;
            case ResourceType.Stone:
                stone += amount;
                break;
            case ResourceType.Gold:
                gold += amount;
                break;
        }
    }
}
