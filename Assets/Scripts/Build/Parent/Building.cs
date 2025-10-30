using UnityEngine;
using UnityEngine.Tilemaps;

public class Building : MonoBehaviour
{
    [Header("Building Data")]
    public BuildingData buildingData;  
    public BuildingType buildingType;
    public bool requiresPower;
    public ExtractOrTranformResource[] resourcesCanBeExtractedOrTransformed;
    public int quantityResourcesPerCycle;
    public float cycleTime;
    public string description;
    
    [Header("References")]
    public Tilemap tilemapResources;


    public virtual void InitializeBuilding(BuildingData data, int order, Tilemap tilemapResource)
    {
        buildingData = data;
        gameObject.name = buildingData.name;
        gameObject.GetComponent<SpriteRenderer>().sprite = buildingData.buildingSprite;
        requiresPower = buildingData.requiresPower;
        description = buildingData.description;
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
}
