using UnityEngine;

public class Building : MonoBehaviour
{
    public BuildingData buildingData;  
    public BuildingType buildingType;
    public int buildingSize;
    public bool requiresPower;
    public string description;

    protected void InitializeBuilding(BuildingData data)
    {
        buildingData = data;
        gameObject.name = buildingData.name;
        gameObject.GetComponent<SpriteRenderer>().sprite = buildingData.buildingSprite;
        buildingType = buildingData.buildingType;
        buildingSize = buildingData.buildingSize;
        requiresPower = buildingData.requiresPower;
        description = buildingData.description;
    }
}
