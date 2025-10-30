using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Building")]
public class BuildingData : ScriptableObject
{
    public string buildingName => name;
    public Sprite buildingSprite;
    public bool requiresPower;
    public ResourceAndAmount[] resourcesRequiredToBuild;
    public List<ResourceAndAmount> resourcesStored;
    public BuildingType buildingType;
    public ExtractOrTranformResource[] resourcesCanBeExtractedOrTransformed;
    public int quantityResourcesPerCycle;
    public float cycleTime = 1;
    public string description;
}

public enum BuildingType
{
    Special,
    Transform,
    Harvest
}
