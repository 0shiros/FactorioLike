using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class HarvestBuilding : Building
{
    [Header("References")]
    private Transform trans;
    public Tilemap tilemapResources;
    
    [Header("Harvest Building Data")]
    public ExtractOrTranformResource[] resourcesCanBeExtractedOrTransformed;
    public int quantityResourcesPerCycle;
    public float cycleTime;
    
    [Header("Harvesting State")]
    public List<ResourceAndAmount> resourcesStored;
    private ExtractOrTranformResource resourceExtracted;
    private float timeElapsed;
    private TileBase resourceTile;
    private Dictionary<TileBase, ExtractOrTranformResource> tileToResourceMap;
    
    private void Start()
    {
        trans = transform;
    }   

    private void Update()
    {
        if(resourceExtracted == ExtractOrTranformResource.none) DetectTile();
        else HarvestResources();
    }

    public void InitializeHarvestBuilding(BuildingData data,TileBase cristalertTile, TileBase boisNoireTile, TileBase rocheNoireTile, TileBase craneRoncierTile)
    {
        InitializeBuilding(data);
        quantityResourcesPerCycle = data.quantityResourcesPerCycle;
        resourcesCanBeExtractedOrTransformed = data.resourcesCanBeExtractedOrTransformed;
        cycleTime = data.cycleTime;
        timeElapsed = 0f;
        tileToResourceMap = new Dictionary<TileBase, ExtractOrTranformResource>
        {
            {cristalertTile, ExtractOrTranformResource.Extract_FragmentCristalert},
            {boisNoireTile, ExtractOrTranformResource.Extract_BrancheNoire},
            {rocheNoireTile, ExtractOrTranformResource.Extract_PierreNoire},
            {craneRoncierTile, ExtractOrTranformResource.Extract_Crane},
        };
    }
    
    private void DetectTile()
    {
        if (tilemapResources == null) return;
        
        Vector3Int cellPosition = tilemapResources.WorldToCell(trans.position);
        resourceTile = tilemapResources.GetTile(cellPosition);
        if (resourceTile != null)
        {
            IdentifyResource();
        }
    }
    
    private void IdentifyResource()
    {
        foreach (TileBase tile in tileToResourceMap.Keys)
        {
            if(tile == resourceTile)
            {
                foreach (ExtractOrTranformResource resource in resourcesCanBeExtractedOrTransformed)
                {
                    if (tileToResourceMap[tile] == resource)
                    {
                        resourceExtracted = resource;
                        ResourceAndAmount resourceAndAmount = new ResourceAndAmount
                        {
                            resource = resourceExtracted,
                            quantity = 0
                        };
                        resourcesStored.Add(resourceAndAmount);
                        break;
                    }
                }
            }
        }
    }
    
    private void HarvestResources()
    {
        timeElapsed += Time.deltaTime;
        float timeToGenerate = cycleTime / quantityResourcesPerCycle;

        while (timeElapsed >= timeToGenerate)
        {
            resourcesStored[0].quantity++;
            timeElapsed -= timeToGenerate;
        }
    }
}
