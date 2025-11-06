using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResourceListIcone : MonoBehaviour
{
    [SerializeField] private GameObject resourceIconPrefab;
    public List<Sprite> resourceListIcon;
    public List<GameObject> resourceIcons;
    private void Start()
    {
        CreateResourceIcons();
    }

    private void CreateResourceIcons()
    {
        foreach (var sprite in resourceListIcon)
        {
            GameObject icon = Instantiate(resourceIconPrefab, Vector3.zero, Quaternion.identity, transform);
            resourceIcons.Add(icon);
            icon.GetComponentInChildren<Image>().sprite = sprite;
            icon.GetComponentInChildren<TextMeshProUGUI>().text = 0.ToString();
        }
    }

    private void ChangeQuantityResourceUI(List<ResourceAndAmount> resourcesPlayer)
    {
        if(resourcesPlayer.Count == 0) return;
        
        foreach (var resourceAndAmount in resourcesPlayer)
        {
            foreach (var resource in resourceIcons)
            {
                Image resourceImage = resource.GetComponentInChildren<Image>();
                
                if (resourceAndAmount.resource.ToString() == resourceImage.sprite.name)
                {
                    TextMeshProUGUI resourceTextMeshProUGUI = resource.GetComponentInChildren<TextMeshProUGUI>();
                    resourceTextMeshProUGUI.text = resourceAndAmount.quantity.ToString();
                }
            }
        }
    }

    private void OnEnable()
    {
        PlayerResources.action += ChangeQuantityResourceUI;
    }
    
    private void OnDisable()
    {
        PlayerResources.action -= ChangeQuantityResourceUI;
    }
}
