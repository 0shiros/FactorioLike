using System;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
  [Header("Settings")] 
  [SerializeField] private float speed;
  public Vector2 input;
  public Rect limitPosition;
  
  [Header("References")]
  private Transform cameraTransform;

  private void Awake()
  {
      cameraTransform = transform;
  }
  
  public void Move()
  {
      Vector3 move = new Vector3(input.x, input.y, 0) * (speed * Time.deltaTime);
      Vector3 newPos = cameraTransform.position + move;

      float clampedX = Mathf.Clamp(newPos.x, limitPosition.xMin, limitPosition.xMax);
      float clampedY = Mathf.Clamp(newPos.y, limitPosition.yMin, limitPosition.yMax);

      cameraTransform.position = new Vector3(clampedX, clampedY, cameraTransform.position.z);
  }

  // private void OnDrawGizmos()
  // {
  //       Gizmos.color = Color.red;
  //       Vector3 bottomLeft = new Vector3(limitPosition.xMin, limitPosition.yMin, 0);
  //       Vector3 bottomRight = new Vector3(limitPosition.xMax, limitPosition.yMin, 0);
  //       Vector3 topRight = new Vector3(limitPosition.xMax, limitPosition.yMax, 0);
  //       Vector3 topLeft = new Vector3(limitPosition.xMin, limitPosition.yMax, 0);
  //   
  //       Gizmos.DrawLine(bottomLeft, bottomRight);
  //       Gizmos.DrawLine(bottomRight, topRight);
  //       Gizmos.DrawLine(topRight, topLeft);
  //       Gizmos.DrawLine(topLeft, bottomLeft);
  // }
}
