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
    public TileBase cristalertTileOnMap;
    public TileBase boisNoireTileOnMap;
    public TileBase rocheNoireTileOnMap;
    public TileBase craneRoncierTileOnMap;
    

    private void Start() => InitializeGrid();

    private void InitializeGrid()
    {
        float startX = -(width / 2f) * cellsSize + originPosition.x + cellsSize * 0.5f;
        float startY = -(height / 2f) * cellsSize + originPosition.y + cellsSize * 0.5f;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Vector2 cellPosition = new Vector2(startX + x * cellsSize, startY + y * cellsSize);
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
            CreateBuilding(currentCellIndex);
        }
    }

    private bool CanConstructOnCell(int cellIndex)
    {
        if (cells[currentCellIndex].building == null && buildingData != null)
        {
            return true;
        }
        
        return false;
    }

    private void DetectCellContains()
    {
        for (int i = 0; i < cells.Count; i++)
        {
            if (cells[i].rect.Contains(PositionScreenToWorld()))
            {
                currentCellIndex = i;
                break;
            }
        }
    }

    private GameObject CreateBuilding(int cellIndex)
    {
        GameObject prefab = buildingsPrefabs[(int)buildingData.buildingType];
        GameObject buildingInstance = Instantiate(prefab, cells[currentCellIndex].position, Quaternion.identity, buildings.transform);
        Building buildingComponent = buildingInstance.GetComponent<Building>();
        buildingComponent.buildingType = buildingData.buildingType;
        if(buildingComponent.buildingType == BuildingType.Harvest)
        {
            HarvestBuilding harvestBuilding = buildingInstance.GetComponent<HarvestBuilding>();
            harvestBuilding.tilemapResources = tileMapResources;
            harvestBuilding.InitializeHarvestBuilding(buildingData, cristalertTileOnMap, boisNoireTileOnMap, rocheNoireTileOnMap, craneRoncierTileOnMap);
        }
        cells[currentCellIndex].building = buildingInstance;
        return buildingInstance;
    }

    private void DestroyBuildings(int cellIndex)
    {
        Destroy(cells[cellIndex].building);
    }

    private Vector2 PositionScreenToWorld()
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        return Camera.main.ScreenToWorldPoint(mousePosition);
    }

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
