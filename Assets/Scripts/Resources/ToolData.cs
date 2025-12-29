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
        int roll = Random.Range(0, 100);
        int cumulative = 0;

        for (int i = 0; i < chances.Length; i++)
        {
            cumulative += chances[i];
            if (roll < cumulative)
                return values[i];
        }

        return 0;
    }
}
