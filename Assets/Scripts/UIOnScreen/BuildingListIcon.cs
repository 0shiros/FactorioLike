using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingListIcon : MonoBehaviour
{
    [SerializeField] private GameObject buildingIconPrefab;
    [SerializeField] private List<BuildingData> buildingListIconData;

    private void Start()
    {
        CreateBuildingIcons();
    }
    
    private void CreateBuildingIcons()
    {
        for (int i = 0; i < buildingListIconData.Count; i++)
        {
            GameObject icon = Instantiate(buildingIconPrefab, Vector3.zero, Quaternion.identity, transform);
            icon.GetComponentInChildren<TextMeshProUGUI>().text = buildingListIconData[i].buildingName;
            icon.GetComponentInChildren<Image>().sprite = buildingListIconData[i].buildingSprite;
        }
    }
    
}
