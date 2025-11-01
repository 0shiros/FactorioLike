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
    [SerializeField] private List<Cell> cells;
    private BuildingData buildingData;

    [Header("Buildings Settings")]
    public GameObject buildings;
    public GameObject buildingPreviewPrefab;
    public Color32 previewColorToBuild;
    public Color32 previewColorCantBuild;
    private GameObject buildingPreview;
    private SpriteRenderer buildingPreviewSpriteRenderer;
    private int currentCell;
    

    [Header("Tilemap Settings")]
    public Tilemap tileMapResources;
    private TileBase tileToConstruct;
    public TileBase cristalertTileOnMap, boisNoireTileOnMap, rocheNoireTileOnMap, craneRoncierTileOnMap;

    private void Start()
    {
        InitializeGrid();
        buildingPreview = Instantiate(buildingPreviewPrefab, PositionScreenToWorld(), Quaternion.identity);
        buildingPreviewSpriteRenderer = buildingPreview.GetComponent<SpriteRenderer>();
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
    
    public void RemoveBuildingFromMap()
    {
        DestroyBuildings(currentCell);
    }

    public void AddBuildingToMap()
    {
        if (CanConstructOnCell(currentCell))
        {
            CreateBuilding(currentCell);
        }
    }
    
    private void DetectCellContains()
    {
        currentCell = cells.FindIndex(cell => cell.rect.Contains(PositionScreenToWorld()));
    }

    private bool CanConstructOnCell(int cellIndex)
    {
        if (cellIndex < 0 || cellIndex > cells.Count || cells[cellIndex].building != null || buildingData == null) return false;

        TileBase detectedTile = DetectTile(PositionScreenToWorld());
        return (detectedTile == null && buildingData.buildingType is BuildingType.Transport  or BuildingType.Stock or BuildingType.Transform) ||
               (detectedTile != null && buildingData.buildingType == BuildingType.Harvest);
    }

    private GameObject CreateBuilding(int cellIndex)
    {
        GameObject buildingInstance = Instantiate(buildingData.buildingPrefab, cells[currentCell].position, Quaternion.identity, buildings.transform);
        Building buildingComponent = buildingInstance.GetComponent<Building>();
        buildingComponent.InitializeBuilding(buildingData,ReturnOrderInLayer(cells[currentCell].position), tileMapResources);
        cells[currentCell].building = buildingInstance;
        return buildingInstance;
    }
    private void DestroyBuildings(int cellIndex)
    {
        if (cellIndex < 0 || cellIndex > cells.Count) return;
        Destroy(cells[cellIndex].building);
    }
    
    private TileBase DetectTile(Vector2 mousePosition)
    {
        if (tileMapResources == null) return null;
        return tileToConstruct = tileMapResources.GetTile(tileMapResources.WorldToCell(mousePosition));
    }

    private void PreviewBuilding()
    {
        if (currentCell > 0 && currentCell < cells.Count)
        {
            buildingPreview.transform.position = cells[currentCell].position;
        }
        else
        {
            buildingPreview.transform.position = buildingPreview.transform.position;
            return;
        }
        
        buildingPreviewSpriteRenderer.color = CanConstructOnCell(currentCell) ? previewColorToBuild : previewColorCantBuild;
        buildingPreviewSpriteRenderer.sprite = buildingData != null ? buildingPreviewSpriteRenderer.sprite = buildingData.buildingSprite : null;
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
