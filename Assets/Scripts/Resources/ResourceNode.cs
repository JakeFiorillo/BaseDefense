using UnityEngine;

public class ResourceNode : MonoBehaviour
{
    public ResourceType resourceType;

    [Header("Harvest Settings")]
    public int requiredToolTier = 0; // 0 = fist
    public int minYield = 0;
    public int maxYield = 1;

    public int Harvest(int toolTier)
    {
        if (toolTier < requiredToolTier)
            return 0;

        return Random.Range(minYield, maxYield + 1);
    }
}
