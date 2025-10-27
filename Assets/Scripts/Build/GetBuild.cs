using System;
using UnityEngine;

public class GetBuild : MonoBehaviour
{
    private BuildingData buildingData;
    public int index;
    public static Action<BuildingData> action;
   
    public void GetBuilding()
    {
       buildingData = GetComponentInParent<BuildingListIcon>().buildingListIconData[index];
       action?.Invoke(buildingData);
    }
    
    
}
