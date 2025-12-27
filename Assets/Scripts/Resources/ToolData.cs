using UnityEngine;

[CreateAssetMenu(menuName = "Tools/Tool Data")]
public class ToolData : ScriptableObject
{
    public string toolName;
    public float swingTime;

    // Tree harvesting
    public int[] treeYieldAmounts;
    public int[] treeYieldChances; // percentages, must sum to 100

    // Rock harvesting
    public int[] rockYieldAmounts;
    public int[] rockYieldChances; // percentages, must sum to 100

    public int GetTreeYield()
    {
        return GetRandomYield(treeYieldAmounts, treeYieldChances);
    }

    public int GetRockYield()
    {
        return GetRandomYield(rockYieldAmounts, rockYieldChances);
    }

    private int GetRandomYield(int[] amounts, int[] chances)
    {
        int roll = Random.Range(0, 100);
        int cumulative = 0;

        for (int i = 0; i < chances.Length; i++)
        {
            cumulative += chances[i];
            if (roll < cumulative)
            {
                return amounts[i];
            }
        }

        return 0;
    }
}
