using UnityEngine;
using UnityEngine.Tilemaps;

public class SpecialBuilding : TransformBuilding, ISpecialBuilding
{

    public override void InitializeBuilding(BuildingData data,int order, Tilemap tileMapResources)
    {
        base.InitializeBuilding(data, order, tileMapResources);
    }

    public void ActivateSpecialAbility()
    {
        
    }
}
