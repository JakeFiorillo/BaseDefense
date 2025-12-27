using UnityEngine;

public enum HarvestType
{
    Tree,
    Rock
}

public class Harvestable : MonoBehaviour
{
    public HarvestType harvestType;
}
