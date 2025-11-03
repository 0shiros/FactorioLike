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
    private float transferProgress = 0f;
    
    protected void Start()
    { 
        trans = transform;
        DetectBuilding();
        RefreshBuildingAround();
    }

    protected virtual void Update()
    {
        if (buildingDetected == null) return;
        MakeTheTransfert();
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

    public void DetectBuilding()
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

    public void RefreshBuildingAround()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(trans.position, 1f);
        foreach (Collider2D collider in colliders)
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
    
    protected void MakeTheTransfert()
    {
        if (resourcesStored.Count > 0) 
        {
            int transferAmount = CalculateTransferAmount();
            if (transferAmount > 0)
            {
                ResourceAndAmount resourceToTransfer = resourcesStored[0];
                TransferResourceToBuilding(buildingDetected, resourceToTransfer, transferAmount);
            }
        }
    }

    private void TransferResourceToBuilding(Building targetBuilding, ResourceAndAmount resource, int amount)
    {
        bool resourceFound = false;
        foreach (ResourceAndAmount storedResource in targetBuilding.resourcesStored)
        {
            if (storedResource.resource == resource.resource)
            {
                storedResource.quantity += amount;
                resourceFound = true;
                break;
            }
        }

        if (!resourceFound)
        {
            targetBuilding.resourcesStored.Add(new ResourceAndAmount(resource.resource, amount));
        }

        resource.quantity -= amount;
    }

    private int CalculateTransferAmount()
    {
        if (resourcesStored.Count == 0 || resourcesStored[0].quantity <= 0)
        {
            transferProgress = 0f; 
            return 0;
        }

        float maxTransferPerSecond = (float)quantityResourcesPerCycle / cycleTime;

        transferProgress += maxTransferPerSecond * Time.deltaTime;

        int availableAmount = resourcesStored[0].quantity;
        int amountToTransfer = Mathf.Min(Mathf.FloorToInt(transferProgress), availableAmount);

        if (amountToTransfer > 0)
            transferProgress -= amountToTransfer;
        
        if (resourcesStored[0].quantity - amountToTransfer <= 0)
            transferProgress = 0f;

        return amountToTransfer;
    }

    private void OnDrawGizmos()
    {
        if (buildingType != BuildingType.Stock)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(trans.position, trans.position + Vector3.down);
            
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(trans.position, 1f);
        }
    }
    
}
