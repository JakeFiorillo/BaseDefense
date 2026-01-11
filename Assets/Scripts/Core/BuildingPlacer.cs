using UnityEngine;
using System.Collections.Generic;

public class BuildingPlacer : MonoBehaviour
{
    [Header("Building Prefabs")]
    public GameObject corePrefab;
    public GameObject goldGeneratorPrefab;
    
    [Header("Building Icons")]
    public Sprite coreIcon;
    public Sprite goldGeneratorIcon;
    
    [Header("Settings")]
    public KeyCode toggleGridKey = KeyCode.B;
    public KeyCode coreKey = KeyCode.C;
    public KeyCode goldGeneratorKey = KeyCode.Alpha1;
    
    [Header("Preview Colors")]
    public Color validColor = new Color(0f, 1f, 0f, 0.5f);
    public Color invalidColor = new Color(1f, 0f, 0f, 0.5f);
    public Color cannotAffordColor = new Color(1f, 0.5f, 0f, 0.5f);
    
    private bool placementMode = false;
    private GameObject previewBuilding;
    private SpriteRenderer previewRenderer;
    private bool currentPlacementValid = false;
    private BuildingType selectedBuildingType;
    private GameObject currentBuildingPrefab;

    void Start()
    {
        StartCoroutine(InitializePlacement());
    }

    System.Collections.IEnumerator InitializePlacement()
    {
        // Wait one frame to ensure all managers are initialized
        yield return null;
    
        Debug.Log("BuildingPlacer Start - Checking if core is placed...");
    
        // Automatically enter placement mode at start to place core
        if (!BuildingManager.Instance.HasCorePlaced())
        {
            Debug.Log("Core not placed - entering placement mode");
            placementMode = true;
            GridManager.Instance.SetGridVisibility(true);
        
            // Setup menu with just the core
            SetupCoreOnlyMenu();
            SelectBuilding(BuildingType.Core, corePrefab);
        
            if (BuildingMenuUI.Instance != null)
            {
                BuildingMenuUI.Instance.SetInstructionText("Place your Core Tower to begin!");
            }
        }
        else
        {
            Debug.Log("Core already placed");
        }
    }

    void SetupCoreOnlyMenu()
    {
        if (BuildingMenuUI.Instance == null)
        {
            Debug.LogError("BuildingMenuUI.Instance is null!");
            return;
        }

        List<BuildingInfo> buildings = new List<BuildingInfo>
        {
            new BuildingInfo
            {
                buildingType = BuildingType.Core,
                buildingName = "Core",
                hotkey = "C",
                cost = 0,
                icon = coreIcon
            }
        };

        BuildingMenuUI.Instance.SetupBuildingButtons(buildings);
        BuildingMenuUI.Instance.ShowMenu(true);
    }

    void SetupFullBuildingMenu()
    {
        if (BuildingMenuUI.Instance == null)
        {
            Debug.LogError("BuildingMenuUI.Instance is null!");
            return;
        }

        List<BuildingInfo> buildings = new List<BuildingInfo>
        {
            new BuildingInfo
            {
                buildingType = BuildingType.GoldGenerator,
                buildingName = "Gold Gen",
                hotkey = "1",
                cost = BuildingManager.Instance.GetGoldGeneratorCost(),
                icon = goldGeneratorIcon
            }
            // Core is intentionally NOT in this list - it can only be placed at the start
            // Add more buildings here as you create them
        };

    BuildingMenuUI.Instance.SetupBuildingButtons(buildings);
    }

    void Update()
    {
        // Debug B key press
        if (Input.GetKeyDown(toggleGridKey))
        {
            Debug.Log($"B key pressed. Core placed: {BuildingManager.Instance.HasCorePlaced()}, Current placement mode: {placementMode}");
        }

        // Can only toggle grid if core has been placed
        if (BuildingManager.Instance.HasCorePlaced())
        {
            if (Input.GetKeyDown(toggleGridKey))
            {
                TogglePlacementMode();
            }
        }
        
        // Select building type with keys (only in placement mode)
        if (placementMode)
        {
            // Can only select core if it hasn't been placed yet
            if (!BuildingManager.Instance.HasCorePlaced() && Input.GetKeyDown(coreKey))
            {
                SelectBuilding(BuildingType.Core, corePrefab);
            }
            // Can only select other buildings if core HAS been placed
            else if (BuildingManager.Instance.HasCorePlaced())
            {
                if (Input.GetKeyDown(goldGeneratorKey))
                {
                    SelectBuilding(BuildingType.GoldGenerator, goldGeneratorPrefab);
                }
            }
        }

        if (placementMode && currentBuildingPrefab != null)
        {
            UpdateBuildingPreview();
            
            // Place building on click only if valid
            if (Input.GetMouseButtonDown(0) && currentPlacementValid)
            {
                PlaceBuilding();
            }
            
            // Cancel with right click or Escape
            if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Escape))
            {
                if (BuildingManager.Instance.HasCorePlaced())
                {
                    TogglePlacementMode();
                }
            }
        }
    }

    void TogglePlacementMode()
    {
        placementMode = !placementMode;
        Debug.Log($"Toggled placement mode to: {placementMode}");
        
        GridManager.Instance.SetGridVisibility(placementMode);

        if (!placementMode)
        {
            DestroyPreview();
            if (BuildingMenuUI.Instance != null)
            {
                BuildingMenuUI.Instance.ShowMenu(false);
                BuildingMenuUI.Instance.SetInstructionText("");
            }
        }
        else
        {
            SetupFullBuildingMenu();
            if (BuildingMenuUI.Instance != null)
            {
                BuildingMenuUI.Instance.ShowMenu(true);
                BuildingMenuUI.Instance.SetInstructionText("Select a building to place (Right-click to cancel)");
            }
        }
    }
    
    void SelectBuilding(BuildingType type, GameObject prefab)
    {
        selectedBuildingType = type;
        currentBuildingPrefab = prefab;
        
        // Destroy old preview and create new one
        DestroyPreview();
        CreatePreview();
        
        // Update UI
        if (BuildingMenuUI.Instance != null)
        {
            BuildingMenuUI.Instance.SetSelectedBuilding(type);
        }
        
        Debug.Log($"Selected: {type}");
    }

    void CreatePreview()
    {
        if (currentBuildingPrefab != null)
        {
            previewBuilding = Instantiate(currentBuildingPrefab);
            previewRenderer = previewBuilding.GetComponent<SpriteRenderer>();
            
            // Disable any colliders and scripts on the preview
            Collider2D[] colliders = previewBuilding.GetComponents<Collider2D>();
            foreach (Collider2D col in colliders)
            {
                col.enabled = false;
            }
            
            // Disable building scripts on preview
            Building buildingScript = previewBuilding.GetComponent<Building>();
            if (buildingScript != null)
            {
                buildingScript.enabled = false;
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
            
            // Check if placement is valid
            currentPlacementValid = GridManager.Instance.IsValidPlacement(snappedPos, 1f);
            
            // Check if can afford (core is always free/affordable)
            bool canAfford = true;
            if (selectedBuildingType == BuildingType.GoldGenerator)
            {
                canAfford = BuildingManager.Instance.CanAffordGoldGenerator();
                if (BuildingMenuUI.Instance != null)
                {
                    BuildingMenuUI.Instance.UpdateBuildingAffordability(BuildingType.GoldGenerator, canAfford);
                }
            }
            
            // Change preview color based on validity and affordability
            if (previewRenderer != null)
            {
                if (!canAfford)
                {
                    previewRenderer.color = cannotAffordColor;
                    currentPlacementValid = false;
                }
                else if (currentPlacementValid)
                {
                    previewRenderer.color = validColor;
                }
                else
                {
                    previewRenderer.color = invalidColor;
                }
            }
        }
    }

    void PlaceBuilding()
    {
        if (currentBuildingPrefab != null && currentPlacementValid)
        {
            Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorld.z = 0;
        
            Vector3 snappedPos = GridManager.Instance.SnapToGrid(mouseWorld);
        
            // Place Core
            if (selectedBuildingType == BuildingType.Core)
            {
                // IMPORTANT: Check if core has already been placed
                if (BuildingManager.Instance.HasCorePlaced())
                {
                    Debug.Log("Core already placed! Cannot place another.");
                    return;
                }
            
                Debug.Log("Placing core...");
                GameObject building = Instantiate(currentBuildingPrefab, snappedPos, Quaternion.identity);
            
                // Enable colliders
                Collider2D[] colliders = building.GetComponents<Collider2D>();
                foreach (Collider2D col in colliders)
                {
                    col.enabled = true;
                }
            
                building.tag = "Building";
            
                // Register with BuildingManager
                Building buildingScript = building.GetComponent<Building>();
                if (buildingScript != null)
                {
                    BuildingManager.Instance.RegisterBuilding(buildingScript);
                }
            
                BuildingManager.Instance.OnCorePlaced();
            
                // Exit placement mode after placing core
                DestroyPreview();
                placementMode = false;
                GridManager.Instance.SetGridVisibility(false);

                if (BuildingMenuUI.Instance != null)
                {
                    BuildingMenuUI.Instance.ShowMenu(false);
                    BuildingMenuUI.Instance.SetInstructionText("Core placed! Press B to build structures.");
                }
            
                Debug.Log("Core placed! You can now build other structures.");
            }
            // Place Gold Generator
            else if (selectedBuildingType == BuildingType.GoldGenerator)
            {
                int cost = BuildingManager.Instance.GetGoldGeneratorCost();
            
                // Deduct cost
                if (cost > 0)
                {
                    Inventory.Instance.AddResource(ResourceType.Gold, -cost);
                }
            
                // Place the building
                GameObject building = Instantiate(currentBuildingPrefab, snappedPos, Quaternion.identity);
            
                // Enable colliders
                Collider2D[] colliders = building.GetComponents<Collider2D>();
                foreach (Collider2D col in colliders)
                {
                    col.enabled = true;
                }
            
                building.tag = "Building";
            
                // Register with BuildingManager
                Building buildingScript = building.GetComponent<Building>();
                if (buildingScript != null)
                {
                    BuildingManager.Instance.RegisterBuilding(buildingScript);
                }
            
                BuildingManager.Instance.OnGoldGeneratorPlaced();
            
                // Update the menu to show new cost
                SetupFullBuildingMenu();
                if (BuildingMenuUI.Instance != null)
                {
                    BuildingMenuUI.Instance.SetSelectedBuilding(selectedBuildingType);
                }
            
                Debug.Log($"Gold Generator placed! Cost: {cost}. Next one costs: {BuildingManager.Instance.GetGoldGeneratorCost()}");
            }
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