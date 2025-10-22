using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Building")]
public class BuildingData : ScriptableObject
{
    public string buildingName => name;
    public Sprite buildingSprite;
    public Vector2Int buildingSize;
    public bool requiresPower;
    public ResourceAndAmount[] resourcesRequiredToBuild;
    public ResourceAndAmount[] resourcesStored;
    public BuildingType buildingType;
    public ResourceAndAmount[] resourcesPerCycle;
    public float cycleTime;
    public string description;
}

public enum BuildingType
{
    Special,
    Transform,
    Harvest
}
