using UnityEngine;
using UnityEngine.Tilemaps;

public class StockBuilding : Building
{
    public PlayerResources playerResources;

    public override void InitializeBuilding(BuildingData data, int order, Tilemap tileMapResources)
    {
        base.InitializeBuilding(data, order, tileMapResources);
    }
}
