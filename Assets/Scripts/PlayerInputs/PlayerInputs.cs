using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerInputs : MonoBehaviour
{
    [SerializeField] private CameraDrag cameraDrag;
    [SerializeField] private GridManager gridManager;
    private bool isLeftClickPress;
    
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
        if(isLeftClickPress && !EventSystem.current.IsPointerOverGameObject()) gridManager.SetValue();
    }

    public void DragCamera(InputAction.CallbackContext context)
    {
        cameraDrag.DragCamera(context);
    }
    
    public void ChangeCellValueOnClick(InputAction.CallbackContext context)
    {
        isLeftClickPress = context.action.IsPressed();
    }
}
