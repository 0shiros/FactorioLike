using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerInputs : MonoBehaviour
{
    [SerializeField] private CameraMovement cameraMovement;
    [SerializeField] private GridManager gridManager;
    private bool isLeftClickPress;
    private bool isRightClickPress;
    private bool canMove;
    
    private void Start()
    {
        if (cameraMovement == null)
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
        if (isLeftClickPress && !EventSystem.current.IsPointerOverGameObject())
        {
            gridManager.AddBuildingToMap();
        }
        
        if (isRightClickPress && !EventSystem.current.IsPointerOverGameObject())
        {
            gridManager.RemoveBuildingFromMap();
        }

        if (canMove)
        {
            cameraMovement.Move();
        }
    }

    public void CameraMove(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
           canMove = true;
           cameraMovement.input = context.ReadValue<Vector2>();
        }
        else if (context.canceled)
        {
            canMove = false;
        }
    }

    public void DestroyBuilding(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isRightClickPress = true;
        }
        else if (context.canceled)
        {
            isRightClickPress = false;
        }
    }
    
    public void CreateBuilding(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isLeftClickPress = true;
        }
        else if (context.canceled)
        {
            isLeftClickPress = false;
        }
    }

    public void ChangeBuildingDirection(InputAction.CallbackContext context)
    {
        if (context.started) gridManager.ChangeBuildingDirection(context.ReadValue<float>());
    }
}
