using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GridManager : MonoBehaviour
{
    [Header("Grid Settings")]
    [SerializeField] private int width, height, cellsSize;
    [SerializeField] private Vector2 originPosition;
    [SerializeField] private List<Cell> cells;
    private BuildingData buildingData;

    public GameObject buildings;
    public GameObject[] buildingsPrefabs;

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
                cells.Add(new Cell { position = cellPosition, building = null } );;
            }
        }
    }

    public void SetValue() => ChangeCellValueOnClick(PositionScreenToWorld(), buildingData);

    private void OnEnable() => GetBuild.action += SetBuildingData;
    private void OnDisable() => GetBuild.action -= SetBuildingData;

    private void SetBuildingData(BuildingData building) => buildingData = building;

    private void ChangeCellValueOnClick(Vector2 mousePosition, BuildingData building)
    {
        foreach (Cell cell in cells)
        {
            Rect cellRect = new Rect(
                cell.GetPosition().x - cellsSize / 2f,
                cell.GetPosition().y - cellsSize / 2f,
                cellsSize,
                cellsSize
            );

            if (cellRect.Contains(mousePosition) && cell.GetValue() == null && buildingData != null)    
            {
                cell.SetValue(CreateBuilding(cell));
                break;
            }
        }
    }

    private GameObject CreateBuilding(Cell cell)
    {
        GameObject prefab = buildingsPrefabs[(int)buildingData.buildingType];
        GameObject buildingInstance = Instantiate(prefab, cell.GetPosition(), Quaternion.identity, buildings.transform);
        buildingInstance.GetComponent<Building>().InitializeBuilding(buildingData);
        return buildingInstance;
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
            Gizmos.DrawWireCube(cell.GetPosition(), Vector2.one * cellsSize);
        }
    }
}
