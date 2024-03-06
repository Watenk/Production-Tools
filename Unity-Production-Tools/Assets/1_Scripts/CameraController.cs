// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class CameraController
// {
//     private bool moveCam;
//     private Vector2 previousMousePos;

//     //References
//     private InputManager inputManager;

//     //-------------------------------------------------------------------

//     public CameraController(InputManager inputManager){
//         this.inputManager = inputManager;

//         inputManager.OnMousePosChange += OnMousePosChange;
//         inputManager.OnMiddleMouseDown += OnMiddleMouseDown;
//         inputManager.OnMiddleMouseUp += OnMiddleMouseUp;
//         inputManager.OnMouseScroll += OnMouseScroll;
//     }

//     //--------------------------------------------------------------------

//     private void OnMousePosChange(){
//         if (moveCam == true){
//             Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

//             float xDifference = (mousePos.x - previousMousePos.x) * GameSettings.Instance.CameraSpeed;
//             float yDifference = (mousePos.y - previousMousePos.y) * GameSettings.Instance.CameraSpeed;

//             Vector3 newCamPos = new Vector3(Camera.main.transform.position.x - xDifference, Camera.main.transform.position.y - yDifference, -10);
//             Camera.main.transform.position = newCamPos;
//         }
//     }

//     private void OnMiddleMouseDown(){
//         previousMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
//         moveCam = true;
//     }

//     private void OnMiddleMouseUp(){
//         moveCam = false;
//     }

//     private void OnMouseScroll(float scrollDelta){
//         if (scrollDelta > 0f && Camera.main.orthographicSize > GameSettings.Instance.MinCamSize)
//         {
//             Camera.main.orthographicSize -= Camera.main.orthographicSize * GameSettings.Instance.ScrollSpeed * 0.01f;
//         }

//         if (scrollDelta < 0f && Camera.main.orthographicSize < GameSettings.Instance.MaxCamSize)
//         {
//             Camera.main.orthographicSize += Camera.main.orthographicSize * GameSettings.Instance.ScrollSpeed * 0.01f;
//         }
//     }
// }
