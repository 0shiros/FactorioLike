using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Building")]
public class BuildingData : ScriptableObject
{
    public string buildingName => name;
    public GameObject buildingPrefab;
    public Sprite buildingSprite;
    public ResourceAndAmount[] resourcesRequiredToBuild;
    public BuildingType buildingType;
    public ExtractOrTranformResource[] resourcesCanBeExtractedOrTransformed;
    public int quantityResourcesPerCycle;
    public float cycleTime = 1;
    public string description;
}

public enum BuildingType
{
    Transport,
    Transform,
    Harvest,
    Stock
}
