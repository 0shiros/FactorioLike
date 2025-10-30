using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "ScriptableObjects/TileMapToResource")]
public class TileMapToResource : ScriptableObject
{
    public List<TileToResource> tileToResourceMap;
}

[System.Serializable]
public class TileToResource
{
    public TileBase tile;
    public ExtractOrTranformResource resource;
}