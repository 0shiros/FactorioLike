using System;
using UnityEngine;

public class MapResource : MonoBehaviour
{
    [SerializeField] private MapResourceData mapResourceData;

    private void Start()
    {
        SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = mapResourceData.resourceSprite;
        gameObject.name = mapResourceData.name;
        Debug.Log(mapResourceData.extractResource);
    }
}
