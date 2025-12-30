using UnityEngine;

[CreateAssetMenu(menuName = "Tools/Tool Data")]
public class ToolData : ScriptableObject
{
    public string toolName;
    public float swingCooldown;

    public int[] treeYieldAmounts;
    public int[] treeYieldChances;

    public int[] rockYieldAmounts;
    public int[] rockYieldChances;

    public int RollTreeYield()
    {
        return Roll(treeYieldAmounts, treeYieldChances);
    }

    public int RollRockYield()
    {
        return Roll(rockYieldAmounts, rockYieldChances);
    }

    private int Roll(int[] values, int[] chances)
    {
        // Make sure arrays are valid
        if (values == null || chances == null || values.Length != chances.Length)
        {
            Debug.LogError("Invalid yield arrays!");
            return 0;
        }

        int roll = Random.Range(0, 100);
        int cumulative = 0;

        for (int i = 0; i < chances.Length; i++)
        {
            cumulative += chances[i];
            if (roll < cumulative)
            {
                Debug.Log($"Rolled {roll}, returning {values[i]}");
                return values[i];
            }
        }

        Debug.LogWarning($"Roll {roll} fell through! Cumulative was {cumulative}");
        return 0;
    }
}
