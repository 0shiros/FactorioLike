using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMovement : MonoBehaviour
{
  [Header("Settings")] 
  [SerializeField] private float speed;
  public Vector2 input;
  
  
  [Header("References")]
  private Transform cameraTransform;

  private void Awake()
  {
      cameraTransform = transform;
  }
  
  public void Move()
  {
      Vector3 move = new Vector3(input.x, input.y, 0) * (speed * Time.deltaTime);
      cameraTransform.position += move;
  }
}
