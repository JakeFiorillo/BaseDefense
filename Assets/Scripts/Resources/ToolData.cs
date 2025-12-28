using UnityEngine;

[CreateAssetMenu(menuName = "Tools/Tool Data")]
public class ToolData : ScriptableObject
{
    [Header("Tool Info")]
    public string toolName;
    public float swingCooldown = 1f;

    [Header("Harvest Probabilities")]
    [Range(0f, 1f)] public float chanceForZero = 0.25f;
    [Range(0f, 1f)] public float chanceForOne = 0.75f;
    [Range(0f, 1f)] public float chanceForTwo = 0f;

    public int GetHarvestAmount()
    {
        float roll = Random.value;

        if (roll < chanceForZero)
            return 0;

        roll -= chanceForZero;

        if (roll < chanceForOne)
            return 1;

        return 2;
    }
}
