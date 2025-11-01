using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public abstract class Building : MonoBehaviour
{
    [Header("Building Data")]
    public BuildingData buildingData;
    public GameObject buildingPrefab;
    public BuildingType buildingType;
    public ExtractOrTranformResource[] resourcesCanBeExtractedOrTransformed;
    public int quantityResourcesPerCycle;
    public float cycleTime;
    public string description;
    
    [Header("References")]
    public Tilemap tilemapResources;
    protected Transform trans;
    
    [Header("Building State")]
    protected TileBase currentTile;
    public List<ResourceAndAmount> resourcesStored;
    public float timeElapsed;
    public Building buildingDetected;
    
    protected void Start()
    { 
        trans = transform;
        DetectBuildingAround();
    }
    

    public virtual void InitializeBuilding(BuildingData data, int order, Tilemap tilemapResource)
    {
        buildingData = data;
        gameObject.name = data.name;
        buildingPrefab = data.buildingPrefab;
        gameObject.GetComponent<SpriteRenderer>().sprite = data.buildingSprite;
        description = data.description;
        quantityResourcesPerCycle = data.quantityResourcesPerCycle;
        resourcesCanBeExtractedOrTransformed = data.resourcesCanBeExtractedOrTransformed;
        cycleTime = data.cycleTime;
        tilemapResources = tilemapResource;
        SetOrderInLayer(order);
    }
    
    protected void SetOrderInLayer(int order)
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.sortingOrder = order;
        }
    }

    protected virtual void DetectBuildingAround()
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Vector2.down, 1f);
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider != null && hit.collider.gameObject != gameObject)
            {
                Building building = hit.collider.GetComponent<Building>();

                if (building != null && building.buildingType != BuildingType.Harvest)
                {
                    buildingDetected = building;
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (buildingType != BuildingType.Stock)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(trans.position, trans.position + Vector3.down);
        }
    }
    
}
