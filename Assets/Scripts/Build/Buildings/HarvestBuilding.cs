using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class HarvestBuilding : Building
{
    private Transform trans;
    public Tilemap tilemapResources;
    public ResourceAndAmount[] resourcesRequiredToBuild;
    public ResourceAndAmount[] resourcesStored;
    public ResourceAndAmount resourcesPerCycle;
    public float cycleTime;
    private TileBase resourceTile;
    
    private void Start()
    {
        trans = transform;
    }

    private void Update()
    {
        DetectTile();
    }

    public void InitializeHarvestBuilding(BuildingData data)
    {
        InitializeBuilding(data);
        resourcesRequiredToBuild = data.resourcesRequiredToBuild;
        resourcesStored = data.resourcesStored;
        resourcesPerCycle = data.resourcesPerCycle;
        cycleTime = data.cycleTime;
    }
    
    private void DetectTile()
    {
        Vector3Int cellPosition = tilemapResources.WorldToCell(trans.position);
        resourceTile = tilemapResources.GetTile(cellPosition);
        if (resourceTile != null)
        {
            HarvestResources();
        }
    }
    
    private void HarvestResources()
    {
        if (resourceTile)
        {
            
        }
    }
    
}
