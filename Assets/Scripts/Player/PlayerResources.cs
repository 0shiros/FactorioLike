using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerResources : MonoBehaviour
{
    public List<ResourceAndAmount> resourcesStored;
    
    public static Action<List<ResourceAndAmount>> action;

    private void Update()
    {
        GetResourceQuantity();
    }

    private void GetResourceQuantity()
    {
        action?.Invoke(resourcesStored);
    }
}
