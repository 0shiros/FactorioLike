using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{
    [Header("Grid Settings")]
    [SerializeField] private int width, height, cellsSize;
    [SerializeField] private Vector2 originPosition;
    [SerializeField] private List<Cell> cells = new();
    private BuildingData buildingData;

    [Header("Buildings Settings")]
    public GameObject buildings;
    public GameObject buildingPreviewPrefab;
    public Color32 previewColorToBuild;
    public Color32 previewColorCantBuild;
    private GameObject buildingPreview;
    private SpriteRenderer buildingPreviewSpriteRenderer;
    private int currentCell;
    private Quaternion rotation;

    [Header("Tilemap Settings")]
    public Tilemap tileMapResources;
    public float currentRotation = 0;

    private void Start()
    {
        InitializeGrid();
        SetupBuildingPreview();
    }

    private void Update()
    {
        DetectCellContains();
        PreviewBuilding();
    }
    
    private void InitializeGrid()
    {
        Vector2 start = originPosition - new Vector2(width, height) * cellsSize * 0.5f + Vector2.one * cellsSize * 0.5f;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Vector2 cellPosition = start + new Vector2(x, y) * cellsSize;
                cells.Add(new Cell { 
                    position = cellPosition, 
                    building = null, 
                    rect = new Rect(
                    cellPosition.x - cellsSize / 2f,
                    cellPosition.y - cellsSize / 2f,
                    cellsSize,
                    cellsSize),
                });
            }
        }
    }
    private void OnEnable() => GetBuild.action += SetBuildingCellData;
    private void OnDisable() => GetBuild.action -= SetBuildingCellData;
    
    private void SetBuildingCellData(BuildingData building) => buildingData = building;
    
    public void RemoveBuildingFromMap() => DestroyBuildings(currentCell);
    
    public void AddBuildingToMap()
    {
        if (CanConstructOnCell(currentCell)) CreateBuilding(currentCell);
    }
    
    private void DetectCellContains()
    {
        currentCell = cells.FindIndex(cell => cell.rect.Contains(PositionScreenToWorld()));
    }

    private bool CanConstructOnCell(int cellIndex)
    {
        if (!IsInGrid(cellIndex)|| cells[cellIndex].building != null || buildingData == null) return false;

        TileBase detectedTile = DetectTile(PositionScreenToWorld());
        return (detectedTile == null && buildingData.buildingType != BuildingType.Harvest) ||
               (detectedTile != null && buildingData.buildingType == BuildingType.Harvest);
    }

    private bool IsInGrid(int cellIndex) => cellIndex >= 0 && cellIndex < cells.Count;
    
    private GameObject CreateBuilding(int cellIndex)
    {
        rotation = buildingData.buildingType == BuildingType.Transport ? Quaternion.AngleAxis(currentRotation, Vector3.forward) : Quaternion.identity;
        
        GameObject buildingInstance = Instantiate(buildingData.buildingPrefab, cells[currentCell].position, rotation, buildings.transform);
        buildingInstance.GetComponent<Building>().InitializeBuilding(buildingData,ReturnOrderInLayer(cells[currentCell].position), tileMapResources);
        cells[currentCell].building = buildingInstance;
        return buildingInstance;
    }
    private void DestroyBuildings(int cellIndex)
    {
        if (IsInGrid(cellIndex)) Destroy(cells[cellIndex].building);
    }
    
    private TileBase DetectTile(Vector2 mousePosition)=> tileMapResources?.GetTile(tileMapResources.WorldToCell(mousePosition));
    
    private void SetupBuildingPreview()
    {
        buildingPreview = Instantiate(buildingPreviewPrefab, PositionScreenToWorld(), Quaternion.identity);
        buildingPreviewSpriteRenderer = buildingPreview.GetComponent<SpriteRenderer>();
    }

    private void PreviewBuilding()
    {
        if (IsInGrid(currentCell))
        {
            buildingPreview.transform.position = cells[currentCell].position;
            buildingPreviewSpriteRenderer.sprite = buildingData?.buildingSprite;
            buildingPreviewSpriteRenderer.color = CanConstructOnCell(currentCell) ? previewColorToBuild : previewColorCantBuild;
            buildingPreview.transform.rotation = buildingData?.buildingType == BuildingType.Transport ? Quaternion.AngleAxis(currentRotation, Vector3.forward) : Quaternion.identity;
        }
    }

    public void ChangeBuildingDirection(float rotationValue)
    {
        if(buildingData.buildingType != BuildingType.Transport)
        {
            currentRotation = 0;
            return;
        }
        
        currentRotation += rotationValue > 0 ? -90 : 90;
    }
    
    private int ReturnOrderInLayer(Vector2 position) => height / 2 - (int)position.y;


    private Vector2 PositionScreenToWorld() => Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
    

    // private void OnDrawGizmos()
    // {
    //     if (cells == null) return;
    //     Gizmos.color = Color.white;
    //     foreach (Cell cell in cells)
    //     {
    //         Gizmos.DrawWireCube(cell.position, Vector2.one * cellsSize);
    //     }
    // }
}
