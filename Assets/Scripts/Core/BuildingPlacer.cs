using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuildingPlacer : MonoBehaviour
{
    [Header("Building Prefabs")]
    public GameObject corePrefab;
    public GameObject goldGeneratorPrefab;
    public GameObject wallPrefab;
    public GameObject bombTowerPrefab;
    public GameObject laserTowerPrefab;
    public GameObject boltTowerPrefab;
    public GameObject shockwaveTowerPrefab;
    public GameObject cannonTowerPrefab;
    
    [Header("Building Icons")]
    public Sprite coreIcon;
    public Sprite goldGeneratorIcon;
    public Sprite wallIcon;
    public Sprite bombTowerIcon;
    public Sprite laserTowerIcon;
    public Sprite boltTowerIcon;
    public Sprite shockwaveTowerIcon;
    public Sprite cannonTowerIcon;
    
    [Header("Settings")]
    public KeyCode toggleGridKey = KeyCode.B;
    public KeyCode coreKey = KeyCode.C;
    public KeyCode goldGeneratorKey = KeyCode.Alpha1;
    public KeyCode wallKey = KeyCode.Alpha2;
    public KeyCode bombTowerKey = KeyCode.Alpha3;
    public KeyCode laserTowerKey = KeyCode.Alpha4;
    public KeyCode boltTowerKey = KeyCode.Alpha5;
    public KeyCode shockwaveTowerKey = KeyCode.Alpha6;
    public KeyCode cannonTowerKey = KeyCode.Alpha7;
    
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

    IEnumerator InitializePlacement()
    {
        yield return null;
        
        Debug.Log("BuildingPlacer Start - Checking if core is placed...");
        
        if (!BuildingManager.Instance.HasCorePlaced())
        {
            Debug.Log("Core not placed - entering placement mode");
            placementMode = true;
            GridManager.Instance.SetGridVisibility(true);
            
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
            },
            new BuildingInfo
            {
                buildingType = BuildingType.Wall,
                buildingName = "Wall",
                hotkey = "2",
                cost = 10,
                icon = wallIcon
            },
            new BuildingInfo
            {
                buildingType = BuildingType.BombTower,
                buildingName = "Bomb Tower",
                hotkey = "3",
                cost = 200,
                icon = bombTowerIcon
            },
            new BuildingInfo
            {
                buildingType = BuildingType.LaserTower,
                buildingName = "Laser Tower",
                hotkey = "4",
                cost = 250,
                icon = laserTowerIcon
            },
            new BuildingInfo
            {
                buildingType = BuildingType.BoltTower,
                buildingName = "Bolt Tower",
                hotkey = "5",
                cost = 300,
                icon = boltTowerIcon
            },
            new BuildingInfo
            {
                buildingType = BuildingType.ShockwaveTower,
                buildingName = "Shockwave",
                hotkey = "6",
                cost = 600,
                icon = shockwaveTowerIcon
            },
            new BuildingInfo
            {
                buildingType = BuildingType.CannonTower,
                buildingName = "Cannon Tower",
                hotkey = "7",
                cost = 400,
                icon = cannonTowerIcon
            }
        };

        BuildingMenuUI.Instance.SetupBuildingButtons(buildings);
    }

    void Update()
    {
        if (Input.GetKeyDown(toggleGridKey))
        {
            Debug.Log($"B key pressed. Core placed: {BuildingManager.Instance.HasCorePlaced()}, Current placement mode: {placementMode}");
        }

        if (BuildingManager.Instance.HasCorePlaced())
        {
            if (Input.GetKeyDown(toggleGridKey))
            {
                TogglePlacementMode();
            }
        }
        
        if (placementMode)
        {
            if (!BuildingManager.Instance.HasCorePlaced() && Input.GetKeyDown(coreKey))
            {
                SelectBuilding(BuildingType.Core, corePrefab);
            }
            else if (BuildingManager.Instance.HasCorePlaced())
            {
                if (Input.GetKeyDown(goldGeneratorKey))
                    SelectBuilding(BuildingType.GoldGenerator, goldGeneratorPrefab);
                
                if (Input.GetKeyDown(wallKey))
                    SelectBuilding(BuildingType.Wall, wallPrefab);
                
                if (Input.GetKeyDown(bombTowerKey))
                    SelectBuilding(BuildingType.BombTower, bombTowerPrefab);
                
                if (Input.GetKeyDown(laserTowerKey))
                    SelectBuilding(BuildingType.LaserTower, laserTowerPrefab);
                
                if (Input.GetKeyDown(boltTowerKey))
                    SelectBuilding(BuildingType.BoltTower, boltTowerPrefab);
                
                if (Input.GetKeyDown(shockwaveTowerKey))
                    SelectBuilding(BuildingType.ShockwaveTower, shockwaveTowerPrefab);
                
                if (Input.GetKeyDown(cannonTowerKey))
                    SelectBuilding(BuildingType.CannonTower, cannonTowerPrefab);
            }
        }

        if (placementMode && currentBuildingPrefab != null)
        {
            UpdateBuildingPreview();
            
            if (Input.GetMouseButtonDown(0) && currentPlacementValid)
            {
                PlaceBuilding();
            }
            
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
                
                // Update affordability immediately when menu opens
                UpdateAllBuildingAffordability();
            }
        }
    }
    
    void SelectBuilding(BuildingType type, GameObject prefab)
    {
        selectedBuildingType = type;
        currentBuildingPrefab = prefab;
        
        DestroyPreview();
        CreatePreview();
        
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
            
            Collider2D[] colliders = previewBuilding.GetComponents<Collider2D>();
            foreach (Collider2D col in colliders)
            {
                col.enabled = false;
            }
            
            Building buildingScript = previewBuilding.GetComponent<Building>();
            if (buildingScript != null)
            {
                buildingScript.enabled = false;
            }
            
            // Disable DefenseTower script if it exists
            DefenseTower towerScript = previewBuilding.GetComponent<DefenseTower>();
            if (towerScript != null)
            {
                towerScript.enabled = false;
            }
        }
    }

    void UpdateBuildingPreview()
    {
        if (previewBuilding != null)
        {
            Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorld.z = 0;
            
            // Determine grid size based on building type (walls are 1x1, everything else is 2x2)
            int gridSize = (selectedBuildingType == BuildingType.Wall) ? 1 : 2;
            float buildingSize = (selectedBuildingType == BuildingType.Wall) ? 0.5f : 1f;
            
            Vector3 snappedPos = GridManager.Instance.SnapToGrid(mouseWorld, gridSize);
            previewBuilding.transform.position = snappedPos;
            
            currentPlacementValid = GridManager.Instance.IsValidPlacement(snappedPos, buildingSize);
            
            // Update affordability for ALL buildings in the menu
            UpdateAllBuildingAffordability();
            
            // Check if can afford the currently selected building
            int cost = BuildingManager.Instance.GetBuildingCost(selectedBuildingType);
            bool canAfford = Inventory.Instance.GetResource(ResourceType.Gold) >= cost;
            
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

    void UpdateAllBuildingAffordability()
    {
        if (BuildingMenuUI.Instance == null) return;
    
        int currentGold = Inventory.Instance.GetResource(ResourceType.Gold);
    
        // Update each building type's affordability
        BuildingMenuUI.Instance.UpdateBuildingAffordability(
            BuildingType.GoldGenerator, 
            currentGold >= BuildingManager.Instance.GetGoldGeneratorCost()
        );
    
        BuildingMenuUI.Instance.UpdateBuildingAffordability(
            BuildingType.Wall, 
            currentGold >= 10
        );
    
        BuildingMenuUI.Instance.UpdateBuildingAffordability(
            BuildingType.BombTower, 
            currentGold >= 200
        );
    
        BuildingMenuUI.Instance.UpdateBuildingAffordability(
            BuildingType.LaserTower, 
            currentGold >= 250
        );
    
        BuildingMenuUI.Instance.UpdateBuildingAffordability(
            BuildingType.BoltTower, 
            currentGold >= 300
        );
    
        BuildingMenuUI.Instance.UpdateBuildingAffordability(
            BuildingType.ShockwaveTower, 
            currentGold >= 600
        );
    
        BuildingMenuUI.Instance.UpdateBuildingAffordability(
            BuildingType.CannonTower, 
            currentGold >= 400
        );

        // Update axe upgrade affordability
        if (AxeUpgradeUI.Instance != null)
        {
            AxeUpgradeUI.Instance.UpdateUI();
        }
    }

    void PlaceBuilding()
    {
        if (currentBuildingPrefab != null && currentPlacementValid)
        {
            Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorld.z = 0;
            
            int gridSize = (selectedBuildingType == BuildingType.Wall) ? 1 : 2;
            Vector3 snappedPos = GridManager.Instance.SnapToGrid(mouseWorld, gridSize);
            
            // Place Core
            if (selectedBuildingType == BuildingType.Core)
            {
                if (BuildingManager.Instance.HasCorePlaced())
                {
                    Debug.Log("Core already placed! Cannot place another.");
                    return;
                }
                
                PlaceBuildingAtPosition(snappedPos, 0);
                BuildingManager.Instance.OnCorePlaced();
                
                DestroyPreview();
                placementMode = false;
                GridManager.Instance.SetGridVisibility(false);
                
                if (BuildingMenuUI.Instance != null)
                {
                    BuildingMenuUI.Instance.ShowMenu(false);
                    BuildingMenuUI.Instance.SetInstructionText("Core placed! Press B to build structures.");
                }
                
                Debug.Log("Core placed!");
            }
            // Place Gold Generator
            else if (selectedBuildingType == BuildingType.GoldGenerator)
            {
                int cost = BuildingManager.Instance.GetGoldGeneratorCost();
                PlaceBuildingAtPosition(snappedPos, cost);
                BuildingManager.Instance.OnGoldGeneratorPlaced();
                
                SetupFullBuildingMenu();
                if (BuildingMenuUI.Instance != null)
                {
                    BuildingMenuUI.Instance.SetSelectedBuilding(selectedBuildingType);
                }
                
                Debug.Log($"Gold Generator placed! Next one costs: {BuildingManager.Instance.GetGoldGeneratorCost()}");
            }
            // Place any other building
            else
            {
                int cost = BuildingManager.Instance.GetBuildingCost(selectedBuildingType);
                PlaceBuildingAtPosition(snappedPos, cost);
                Debug.Log($"{selectedBuildingType} placed! Cost: {cost}");
            }
        }
    }

    void PlaceBuildingAtPosition(Vector3 position, int cost)
    {
        // Deduct cost
        if (cost > 0)
        {
            Inventory.Instance.AddResource(ResourceType.Gold, -cost);
        }
        
        // Place the building
        GameObject building = Instantiate(currentBuildingPrefab, position, Quaternion.identity);
        
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
    }

    void DestroyPreview()
    {
        if (previewBuilding != null)
        {
            Destroy(previewBuilding);
        }
    }
}