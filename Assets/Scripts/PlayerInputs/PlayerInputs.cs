using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputs : MonoBehaviour
{
    [SerializeField] private CameraDrag cameraDrag;
    [SerializeField] private GridManager gridManager;

    private void Start()
    {
        if (cameraDrag == null)
        {
            Debug.LogError("No CameraDrag assigned");
        }
        
        if (gridManager == null)
        {
            Debug.LogError("No GridManager assigned");
        }
    }

    public void DragCamera(InputAction.CallbackContext context)
    {
        cameraDrag.DragCamera(context);
    }
    
    public void ChangeCellValueOnClick(InputAction.CallbackContext context)
    {
        gridManager.SetValue(context);
    }
}
