using System.Collections.Generic;
using UnityEngine;

public class TransportBuilding : Building
{
    private float transferProgress = 0f;
    
    private void Update()
    {
        MakeTheTransfert();
    }

    private void MakeTheTransfert()
    {
        if (resourcesStored.Count > 0) 
        {
            int transferAmount = CalculateTransferAmount();
            if (transferAmount > 0)
            {
                ResourceAndAmount resourceToTransfer = resourcesStored[0];
                TransferResourceToBuilding(buildingDetected, resourceToTransfer, transferAmount);
            }
        }
    }

    private void TransferResourceToBuilding(Building targetBuilding, ResourceAndAmount resource, int amount)
    {
        bool resourceFound = false;
        foreach (ResourceAndAmount storedResource in targetBuilding.resourcesStored)
        {
            if (storedResource.resource == resource.resource)
            {
                storedResource.quantity += amount;
                resourceFound = true;
                break;
            }
        }

        if (!resourceFound)
        {
            targetBuilding.resourcesStored.Add(new ResourceAndAmount(resource.resource, amount));
        }

        resource.quantity -= amount;
    }

    private int CalculateTransferAmount()
    {
        if (resourcesStored.Count == 0 || resourcesStored[0].quantity <= 0)
        {
            transferProgress = 0f; 
            return 0;
        }

        float maxTransferPerSecond = (float)quantityResourcesPerCycle / cycleTime;

        transferProgress += maxTransferPerSecond * Time.deltaTime;

        int availableAmount = resourcesStored[0].quantity;
        int amountToTransfer = Mathf.Min(Mathf.FloorToInt(transferProgress), availableAmount);

        if (amountToTransfer > 0)
            transferProgress -= amountToTransfer;
        
        if (resourcesStored[0].quantity - amountToTransfer <= 0)
            transferProgress = 0f;

        return amountToTransfer;
    }

    private void ClearList()
    {
        if (resourcesStored.Count > 0 && resourcesStored[0].quantity <= 0)
        {
            resourcesStored.RemoveAt(0);
        }
    }
}
