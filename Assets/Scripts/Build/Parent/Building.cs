using UnityEngine;

public class Building : MonoBehaviour
{
    public BuildingData buildingData;
    
    public void InitializeBuilding(BuildingData data)
    {
        buildingData = data;
        gameObject.name = buildingData.name;
        gameObject.GetComponent<SpriteRenderer>().sprite = buildingData.buildingSprite;
    }
}
