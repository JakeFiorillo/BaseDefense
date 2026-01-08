using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;

    [Header("Grid Settings")]
    public int gridWidth = 64;
    public int gridHeight = 64;
    public float cellSize = 0.5f;
    
    [Header("Visual Settings")]
    public Color gridColor = new Color(1f, 1f, 1f, 0.3f);
    public float gridLineWidth = 0.05f;

    private bool showGrid = false;
    private LineRenderer[,] horizontalLines;
    private LineRenderer[,] verticalLines;
    private GameObject gridLinesParent;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        CreateGridLines();
        SetGridVisibility(false);
    }

    void CreateGridLines()
    {
        gridLinesParent = new GameObject("GridLines");
        gridLinesParent.transform.parent = transform;

        float offsetX = -(gridWidth * cellSize) / 2f;
        float offsetY = -(gridHeight * cellSize) / 2f;

        // Create horizontal lines
        horizontalLines = new LineRenderer[gridHeight + 1, 1];
        for (int y = 0; y <= gridHeight; y++)
        {
            GameObject lineObj = new GameObject($"HLine_{y}");
            lineObj.transform.parent = gridLinesParent.transform;
            
            LineRenderer lr = lineObj.AddComponent<LineRenderer>();
            lr.material = new Material(Shader.Find("Sprites/Default"));
            lr.startColor = gridColor;
            lr.endColor = gridColor;
            lr.startWidth = gridLineWidth;
            lr.endWidth = gridLineWidth;
            lr.positionCount = 2;
            lr.sortingOrder = 100;
            
            float yPos = offsetY + (y * cellSize);
            lr.SetPosition(0, new Vector3(offsetX, yPos, 0));
            lr.SetPosition(1, new Vector3(offsetX + (gridWidth * cellSize), yPos, 0));
            
            horizontalLines[y, 0] = lr;
        }

        // Create vertical lines
        verticalLines = new LineRenderer[gridWidth + 1, 1];
        for (int x = 0; x <= gridWidth; x++)
        {
            GameObject lineObj = new GameObject($"VLine_{x}");
            lineObj.transform.parent = gridLinesParent.transform;
            
            LineRenderer lr = lineObj.AddComponent<LineRenderer>();
            lr.material = new Material(Shader.Find("Sprites/Default"));
            lr.startColor = gridColor;
            lr.endColor = gridColor;
            lr.startWidth = gridLineWidth;
            lr.endWidth = gridLineWidth;
            lr.positionCount = 2;
            lr.sortingOrder = 100;
            
            float xPos = offsetX + (x * cellSize);
            lr.SetPosition(0, new Vector3(xPos, offsetY, 0));
            lr.SetPosition(1, new Vector3(xPos, offsetY + (gridHeight * cellSize), 0));
            
            verticalLines[x, 0] = lr;
        }
    }

    public void SetGridVisibility(bool visible)
    {
        showGrid = visible;
        if (gridLinesParent != null)
        {
            gridLinesParent.SetActive(visible);
        }
    }

    public void ToggleGrid()
    {
        SetGridVisibility(!showGrid);
    }

    // Snap to center of a 2x2 grid area
    public Vector3 SnapToGrid(Vector3 worldPosition)
    {
        float offsetX = -(gridWidth * cellSize) / 2f;
        float offsetY = -(gridHeight * cellSize) / 2f;

        // Convert to grid coordinates
        int gridX = Mathf.FloorToInt((worldPosition.x - offsetX) / cellSize);
        int gridY = Mathf.FloorToInt((worldPosition.y - offsetY) / cellSize);

        // Clamp to grid bounds (buildings are 2x2 cells)
        gridX = Mathf.Clamp(gridX, 0, gridWidth - 2);
        gridY = Mathf.Clamp(gridY, 0, gridHeight - 2);

        // Snap to center of the 2x2 area
        float snappedX = offsetX + (gridX * cellSize) + cellSize;
        float snappedY = offsetY + (gridY * cellSize) + cellSize;

        return new Vector3(snappedX, snappedY, 0);
    }

    // Check if a 2x2 building can be placed at this position
    public bool IsValidPlacement(Vector3 worldPosition, float buildingSize = 1f)
    {
        // Check for overlaps using a box cast
        // Make the check area slightly smaller (0.9 instead of full 1.0) to allow adjacent buildings
        float checkSize = buildingSize * 0.9f;
        Collider2D[] hits = Physics2D.OverlapBoxAll(worldPosition, new Vector2(checkSize, checkSize), 0f);
        
        foreach (Collider2D hit in hits)
        {
            // Ignore triggers (like the preview building itself)
            if (hit.isTrigger)
                continue;
                
            // Check if hit something that blocks placement
            if (hit.GetComponent<Harvestable>() != null ||  // Trees/Rocks
                hit.GetComponent<PlayerMovement>() != null ||  // Player
                hit.CompareTag("Enemy") ||  // Enemies
                hit.CompareTag("Building"))  // Other buildings
            {
                Debug.Log($"Placement blocked by: {hit.gameObject.name}");
                return false;
            }
        }
        
        return true;
    }
}