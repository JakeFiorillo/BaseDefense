using UnityEngine;

public class WorldSpawner : MonoBehaviour
{
    [Header("Map Size")]
    public float grassHalfSize = 16f;

    [Header("Prefabs")]
    public GameObject[] treePrefabs;
    public GameObject[] rockPrefabs;

    [Header("Spawn Counts")]
    public int treeCount = 40;
    public int rockCount = 20;

    void Start()
    {
        SpawnObjects(treePrefabs, treeCount);
        SpawnObjects(rockPrefabs, rockCount);
    }

    void SpawnObjects(GameObject[] prefabs, int count)
    {
        for (int i = 0; i < count; i++)
        {
            Vector2 pos = GetRandomPointInGrass();
            Instantiate(prefabs[Random.Range(0, prefabs.Length)], pos, Quaternion.identity);
        }
    }

    Vector2 GetRandomPointInGrass()
    {
        float x = Random.Range(-grassHalfSize, grassHalfSize);
        float y = Random.Range(-grassHalfSize, grassHalfSize);
        return new Vector2(x, y);
    }
}
