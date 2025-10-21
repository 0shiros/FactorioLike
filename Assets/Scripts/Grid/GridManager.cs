using System;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class GridManager : MonoBehaviour
{
    [Header("Grid Settings")]
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private int cellsSize;
    [SerializeField] private List<Cell> cells;
    [SerializeField] private Vector2 originPosition;

    private void Start()
    {
        InitializeGrid();
    }

    private void InitializeGrid()
    {
        if (cells == null)
            cells = new List<Cell>();

        float startX = -(width / 2f) * cellsSize + originPosition.x + cellsSize * 0.5f;
        float startY = -(height / 2f) * cellsSize + originPosition.y + cellsSize * 0.5f;

        for (int y = 0; y < width; y++)
        {
            for (int x = 0; x < height; x++)
            {
                float cellX = startX + x * cellsSize;
                float cellY = startY + y * cellsSize;
                Vector2 cellPosition = new Vector2(cellX, cellY);
                
                Cell cell = new Cell();
                cell.InitializeCell(cellPosition, 0);
                cells.Add(cell);
            }
        }
    }
    
    public void SetValue(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        ChangeCellValueOnClick(PositionScreenToWorld(),5);
    }
    
    private void ChangeCellValueOnClick(Vector2 mousePosition, int newValue)
    {
        Debug.Log(mousePosition);
        
        foreach (Cell cell in cells)
        {
            Vector2 cellPos = cell.GetPosition();
            Rect cellRect = new Rect(
                cellPos.x - cellsSize / 2f,
                cellPos.y - cellsSize / 2f,
                cellsSize,
                cellsSize
            );
            
            Debug.Log(cellRect.ToString());

            if (cellRect.Contains(mousePosition))
            {
                cell.SetValue(newValue);
                break;
            }
        }
    }
    
    private Vector2 PositionScreenToWorld()
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Camera mainCamera = Camera.main;
        return mainCamera.ScreenToWorldPoint(mousePosition);
    }

    private void OnDrawGizmos()
    {
        if (cells == null) return;
        Gizmos.color = Color.white;
        foreach (Cell cell in cells)
        {
            Vector2 cellPos = cell.GetPosition();
            
            Gizmos.DrawWireCube(
                new Vector3(cellPos.x, cellPos.y, 0),
                new Vector3(cellsSize, cellsSize, 0)
            );
        }
    }
}


