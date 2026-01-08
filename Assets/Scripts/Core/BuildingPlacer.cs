using UnityEngine;

public class BuildingPlacer : MonoBehaviour
{
    public GameObject buildingPrefab;
    public KeyCode toggleGridKey = KeyCode.B;
    
    [Header("Preview Colors")]
    public Color validColor = new Color(0f, 1f, 0f, 0.5f);  // Green
    public Color invalidColor = new Color(1f, 0f, 0f, 0.5f);  // Red
    
    private bool placementMode = false;
    private GameObject previewBuilding;
    private SpriteRenderer previewRenderer;
    private bool currentPlacementValid = false;

    void Update()
    {
        if (Input.GetKeyDown(toggleGridKey))
        {
            TogglePlacementMode();
        }

        if (placementMode)
        {
            UpdateBuildingPreview();
            
            // Place building on click only if valid
            if (Input.GetMouseButtonDown(0) && currentPlacementValid)
            {
                PlaceBuilding();
            }
        }
    }

    void TogglePlacementMode()
    {
        placementMode = !placementMode;
        GridManager.Instance.SetGridVisibility(placementMode);

        if (placementMode)
        {
            CreatePreview();
        }
        else
        {
            DestroyPreview();
        }
    }

    void CreatePreview()
    {
        if (buildingPrefab != null)
        {
            previewBuilding = Instantiate(buildingPrefab);
            previewRenderer = previewBuilding.GetComponent<SpriteRenderer>();
            
            // Disable any colliders on the preview (we'll add them on placement)
            Collider2D[] colliders = previewBuilding.GetComponents<Collider2D>();
            foreach (Collider2D col in colliders)
            {
                col.enabled = false;
            }
        }
    }

    void UpdateBuildingPreview()
    {
        if (previewBuilding != null)
        {
            Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorld.z = 0;
            
            Vector3 snappedPos = GridManager.Instance.SnapToGrid(mouseWorld);
            previewBuilding.transform.position = snappedPos;
            
            // Check if placement is valid (building size is 1f now with 0.5 cell size)
            currentPlacementValid = GridManager.Instance.IsValidPlacement(snappedPos, 1f);
            
            // Change preview color based on validity
            if (previewRenderer != null)
            {
                previewRenderer.color = currentPlacementValid ? validColor : invalidColor;
            }
        }
    }

    void PlaceBuilding()
    {
        if (buildingPrefab != null && currentPlacementValid)
        {
            Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorld.z = 0;
            
            Vector3 snappedPos = GridManager.Instance.SnapToGrid(mouseWorld);
            
            GameObject building = Instantiate(buildingPrefab, snappedPos, Quaternion.identity);
            
            // Make sure the placed building has proper colliders enabled
            Collider2D[] colliders = building.GetComponents<Collider2D>();
            foreach (Collider2D col in colliders)
            {
                col.enabled = true;
            }
            
            building.tag = "Building";
            
            Debug.Log($"Building placed at: {snappedPos}");
        }
    }

    void DestroyPreview()
    {
        if (previewBuilding != null)
        {
            Destroy(previewBuilding);
        }
    }
}