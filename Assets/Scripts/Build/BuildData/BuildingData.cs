using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Building", menuName = "Buildings/BuildingData", order = 1)]
public class BuildingData : ScriptableObject
{
    public string buildingName;
    public Vector2Int buildingSize;
    public Sprite buildingSprite;
    public string description;
    public bool needsAlimentation; 
    public List<Resources> resourceCosts;

    [System.Serializable]
    public class Resources
    {
        public string resourceName;
        public int amount;
    }
}
