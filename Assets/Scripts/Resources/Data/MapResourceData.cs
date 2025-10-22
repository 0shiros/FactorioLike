using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/MapResource")]
public class MapResourceData : ScriptableObject
{
    public string resourceName => name;
    public Sprite resourceSprite;
    public ExtractOrTranformResource extractResource;
}

