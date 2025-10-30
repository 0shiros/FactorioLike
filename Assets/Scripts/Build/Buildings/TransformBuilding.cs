using UnityEngine;
using UnityEngine.Tilemaps;

public class TransformBuilding : Building
{
    public RecipeData recipe;
    public float timeElapsed;
    
    public override void InitializeBuilding(BuildingData data, int order, Tilemap tileMapResources)
    {
        base.InitializeBuilding(data, order, tileMapResources);
        timeElapsed = 0f;
    }
    
    
}
