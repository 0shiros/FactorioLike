using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingListIcon : MonoBehaviour
{
    private Vector2 panelSize;
    private Vector2 offSet;
    [SerializeField] private GameObject buildingIconPrefab;
    [SerializeField] private List<BuildingData> buildingListIconData;
    private Sprite iconSprite;
    private TextMeshProUGUI text;
    [SerializeField] private int iconSize;
    [SerializeField] private int iconsPerRow;

    private void Start()
    {
        panelSize = GetComponent<RectTransform>().sizeDelta;
        offSet = panelSize * 0.1f;
        panelSize -= offSet;
        CreateBuildingIcons();
    }
    
    private void CreateBuildingIcons()
    {
        float firstIconX = offSet.x + buildingIconPrefab.GetComponent<RectTransform>().sizeDelta.x * iconSize / 2;
        float firstIconY = -offSet.y - buildingIconPrefab.GetComponent<RectTransform>().sizeDelta.x * iconSize / 2;
        float iconSpacing = 5f; // Espacement entre les icônes

        for (int i = 0; i < buildingListIconData.Count; i++)
        {
            GameObject iconObject = Instantiate(buildingIconPrefab, transform);
            RectTransform iconRect = iconObject.GetComponent<RectTransform>();
            iconRect.sizeDelta *= iconSize;

            float iconX = firstIconX + (i % iconsPerRow - 1) * (iconSize + iconSpacing);
            float iconY = firstIconY - ((float)i / 2) * (iconSize + iconSpacing);
            iconRect.anchoredPosition = new Vector2(iconX, iconY);

            Image iconImage = iconObject.GetComponentInChildren<Image>(true);
            iconImage.sprite = buildingListIconData[i].buildingSprite;

            text = iconObject.GetComponentInChildren<TextMeshProUGUI>(true);
            text.text = buildingListIconData[i].buildingName;
        }
    }
}
