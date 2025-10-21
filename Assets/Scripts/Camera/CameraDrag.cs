using UnityEngine;
using UnityEngine.InputSystem;

public class CameraDrag : MonoBehaviour
{
  [Header("Settings")]
  private Vector3 origin;
  private Vector3 difference;
  private bool isDragging = false;
  [SerializeField] private float dragSpeed = 1;
  
  [Header("References")]
  private Camera mainCamera;

  private Vector3 mousePosition => mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());

  private void Awake()
  {
    mainCamera = Camera.main;
  }

  public void OnDrag(InputAction.CallbackContext context)
  {
    if (context.started)
    {
      origin = mousePosition;
    }
    isDragging = context.started || context.performed;
  }

  private void LateUpdate()
  {
    if(!isDragging) return;
    difference = mousePosition - transform.position;
    transform.position = origin - difference * dragSpeed;
  }
}
