using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public abstract class Building : MonoBehaviour
{
    [Header("Building Data")]
    public BuildingData buildingData;
    public GameObject buildingPrefab;
    public BuildingType buildingType;
    public ResourceAndAmount[] resourcesRequiredToBuild;
    public ExtractOrTranformResource[] resourcesCanBeExtracted;
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
    private float transferProgress = 0f;
    private Vector2 dirRayCastDetection = Vector2.down;
    
    protected void Start()
    { 
        trans = transform;
        DetectBuilding();
        RefreshBuildingAround();
    }

    protected virtual void Update()
    {
        if (buildingDetected == null || buildingType == BuildingType.Stock) return;
        MakeTheTransfert();
    }

    public virtual void InitializeBuilding(BuildingData data, int order, Tilemap tilemapResource)
    {
        buildingData = data;
        gameObject.name = data.name;
        buildingPrefab = data.buildingPrefab;
        resourcesRequiredToBuild = data.resourcesRequiredToBuild;
        gameObject.GetComponent<SpriteRenderer>().sprite = data.buildingSprite;
        description = data.description;
        buildingType = data.buildingType;
        quantityResourcesPerCycle = data.quantityResourcesPerCycle;
        resourcesCanBeExtracted = data.resourcesCanBeExtracted;
        cycleTime = data.cycleTime;
        tilemapResources = tilemapResource;
        SetOrderInLayer(order);
    }
    
    private void SetOrderInLayer(int order)
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.sortingOrder = order;
        }
    }

    public void DetectBuilding()
    {
        Vector2 worldDirectionRayCast = trans.TransformDirection(dirRayCastDetection);
        foreach (RaycastHit2D hit in Physics2D.RaycastAll(transform.position, worldDirectionRayCast, 1f))
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

    public void RefreshBuildingAround()
    {
        foreach (Collider2D collider in Physics2D.OverlapCircleAll(trans.position, 1f))
        {
            if (collider != null && collider.gameObject != gameObject)
            {
                Building building = collider.GetComponent<Building>();

                if (building.buildingDetected == null || building.buildingDetected != buildingDetected)
                {
                    building.DetectBuilding();
                }
            }
        }
    }
    
    private void MakeTheTransfert()
    {
        if (resourcesStored.Count == 0) return;

        switch (buildingDetected.buildingType)
        {
            case BuildingType.Stock:
                TransferIfPossible();
                break;

            case BuildingType.Transform:
                foreach (var resource in buildingDetected.resourcesStored)
                {
                    if (resource.resource == resourcesStored[0].resource)
                    {
                        TransferIfPossible();
                        break;
                    }
                }
                break;

            default:
                if (buildingDetected.resourcesStored.Count == 0 || 
                    buildingDetected.resourcesStored[0].resource == resourcesStored[0].resource)
                {
                    TransferIfPossible();
                }
                break;
        }
    }

    private void TransferIfPossible()
    {
        int transferAmount = CalculateTransferAmount();
        if (transferAmount > 0)
        {
            TransferResourceToBuilding(buildingDetected, resourcesStored[0], transferAmount);
        }
    }

    private void TransferResourceToBuilding(Building targetBuilding, ResourceAndAmount resource, int amount)
    {
        if (targetBuilding.buildingType == BuildingType.Stock)
        {
            StockBuilding stockBuilding = targetBuilding.gameObject.GetComponent<StockBuilding>();
            ResourceAndAmount targetResource = stockBuilding.playerResources.resourcesStored.Find(r => r.resource == resource.resource);
  
            if (targetResource != null)
            {
                targetResource.quantity += amount;
            }
            else
            {
                stockBuilding.playerResources.resourcesStored.Add(new ResourceAndAmount(resource.resource, amount));
            }
            resource.quantity -= amount;
        }
        else
        {
            ResourceAndAmount targetResource = targetBuilding.resourcesStored.Find(r => r.resource == resource.resource);
            if (targetResource != null)
            {
                targetResource.quantity += amount;
            }
            else
            {
                targetBuilding.resourcesStored.Add(new ResourceAndAmount(resource.resource, amount));
            }
            resource.quantity -= amount;
        }
    }

    private int CalculateTransferAmount()
    {
        if (resourcesStored.Count == 0 || resourcesStored[0].quantity <= 0)
        {
            transferProgress = 0f;
            return 0;
        }

        float maxTransferPerSecond = quantityResourcesPerCycle / cycleTime;
        transferProgress += maxTransferPerSecond * Time.deltaTime;

        int amountToTransfer = Mathf.Min(Mathf.FloorToInt(transferProgress), resourcesStored[0].quantity);
        if (amountToTransfer > 0)
        {
            transferProgress -= amountToTransfer;
        }
        return amountToTransfer;
    }

    private void OnDrawGizmos()
    {
        if (buildingType != BuildingType.Stock)
        {
            Vector2 worldDirectionRayCast = trans.TransformDirection(dirRayCastDetection);
            
            Gizmos.color = Color.red;
            Gizmos.DrawLine(trans.position, trans.position + (Vector3)worldDirectionRayCast);
            
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(trans.position, 1f);
        }
    }
    
}
