using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputs : MonoBehaviour
{
    [SerializeField] private CameraDrag cameraDrag;
    [SerializeField] private GridManager gridManager;
    private bool isHeld;
    
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

    private void Update()
    {
       if(isHeld) gridManager.SetValue();
    }

    public void DragCamera(InputAction.CallbackContext context)
    {
        cameraDrag.DragCamera(context);
    }
    
    public void ChangeCellValueOnClick(InputAction.CallbackContext context)
    {
        isHeld = context.action.IsPressed();
    }
}
