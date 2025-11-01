using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Quaternion = UnityEngine.Quaternion;

public class HarvestBuilding : Building
{
    [Header("Harvesting State")]
    public TileMapToResource tileToResourceMap;
    private ExtractOrTranformResource resourceExtracted;
    
    private void Update()
    {
        if(resourceExtracted == ExtractOrTranformResource.none) DetectTile();
        else HarvestResources();
    }

    public override void InitializeBuilding(BuildingData data, int order, Tilemap tileMapResources)
    {
        base.InitializeBuilding(data, order, tileMapResources);
        timeElapsed = 0f;
    }

    private void DetectTile()
    {
        if (tilemapResources == null) return;
        
        Vector3Int cellPosition = tilemapResources.WorldToCell(trans.position);
        currentTile = tilemapResources.GetTile(cellPosition);
        
        if (currentTile != null) IdentifyResource();
    }
    
    private void IdentifyResource()
    {
        TileToResource tileToResource = tileToResourceMap.tileToResourceMap.Find(t => t.tile == currentTile);

        if (tileToResource == null) return;

        if (Array.Exists(resourcesCanBeExtractedOrTransformed, r => r == tileToResource.resource))
        {
            resourceExtracted = tileToResource.resource;
            resourcesStored.Add(new ResourceAndAmount (resourceExtracted, 0 ));
        }
    }
    
    private void HarvestResources()
    {
        timeElapsed += Time.deltaTime;
        float timeToGenerate = cycleTime / quantityResourcesPerCycle;

        if (!(timeElapsed >= timeToGenerate)) return;
        resourcesStored[0].quantity++;
        timeElapsed -= timeToGenerate;
    }
}
