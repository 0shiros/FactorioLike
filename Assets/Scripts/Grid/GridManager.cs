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
    private int currentCellIndex;

    [Header("Buildings Settings")]
    public GameObject buildings;
    public GameObject[] buildingsPrefabs;

    [Header("Tilemap Settings")]
    public Tilemap tileMapResources;
    private TileBase tileToConstruct;
    public TileBase cristalertTileOnMap, boisNoireTileOnMap, rocheNoireTileOnMap, craneRoncierTileOnMap;

    private void Start() => InitializeGrid();

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
        DetectCellContains();
        DestroyBuildings(currentCellIndex);
    }

    public void AddBuildingToMap()
    {
        DetectCellContains();
        if (CanConstructOnCell(currentCellIndex))
        {
            CreateBuilding(buildingsPrefabs[(int)buildingData.buildingType], currentCellIndex);
        }
    }
    
    private void DetectCellContains()
    {
        Vector2 mousePosition = PositionScreenToWorld();
        for (int i = 0; i < cells.Count; i++)
        {
            if (cells[i].rect.Contains(mousePosition))
            {
                currentCellIndex = i;
                break;
            }
        }
    }

    private bool CanConstructOnCell(int cellIndex)
    {
        if (cells[cellIndex].building != null || buildingData == null) return false;

        TileBase detectedTile = DetectTile(PositionScreenToWorld());
        return (detectedTile == null && (buildingData.buildingType == BuildingType.Special || buildingData.buildingType == BuildingType.Transform)) ||
               (detectedTile != null && buildingData.buildingType == BuildingType.Harvest);
    }

    private GameObject CreateBuilding(GameObject prefab, int cellIndex)
    {
        GameObject buildingInstance = Instantiate(prefab, cells[currentCellIndex].position, Quaternion.identity, buildings.transform);
        Building buildingComponent = buildingInstance.GetComponent<Building>();
        buildingComponent.InitializeBuilding(buildingData,ReturnOrderInLayer(cells[currentCellIndex].position), tileMapResources);
        cells[currentCellIndex].building = buildingInstance;
        return buildingInstance;
    }
    private void DestroyBuildings(int cellIndex)
    {
        Destroy(cells[cellIndex].building);
    }
    
    private TileBase DetectTile(Vector2 mousePosition)
    {
        if (tileMapResources == null) return null;
        return tileToConstruct = tileMapResources.GetTile(tileMapResources.WorldToCell(mousePosition));
    }
    
    private int ReturnOrderInLayer(Vector2 position) => height / 2 - (int)position.y;


    private Vector2 PositionScreenToWorld() => Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
    

    private void OnDrawGizmos()
    {
        if (cells == null) return;
        Gizmos.color = Color.white;
        foreach (Cell cell in cells)
        {
            Gizmos.DrawWireCube(cell.position, Vector2.one * cellsSize);
        }
    }
}
