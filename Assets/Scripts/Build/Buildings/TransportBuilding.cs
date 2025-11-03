using System.Collections.Generic;
using UnityEngine;

public class TransportBuilding : Building
{
    protected override void Update()
    {
        base.Update();
    }

    private void ClearList()
    {
        if (resourcesStored.Count > 0 && resourcesStored[0].quantity <= 0)
        {
            resourcesStored.RemoveAt(0);
        }
    }
}
