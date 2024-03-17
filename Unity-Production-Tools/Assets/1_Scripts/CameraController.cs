using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController
{
    private bool moveCam;
    private Vector2 previousMousePos;

    //-------------------------------------------------------------------

    public CameraController(){
        EventManager.AddListener("OnMousePosChange", MousePosChange);
        EventManager.AddListener("OnMiddleMouseDown", MiddleMouseDown);
        EventManager.AddListener("OnMiddleMouseUp", MiddleMouseUp);
        EventManager.AddListener<float>("OnMouseScroll", MouseScroll);
    }

    //--------------------------------------------------------------------

    private void MousePosChange(){
        if (moveCam == true){
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            float xDifference = (mousePos.x - previousMousePos.x); // * ToolSettings.Instance.CameraSpeed;
            float yDifference = (mousePos.y - previousMousePos.y); // * ToolSettings.Instance.CameraSpeed;

            Vector3 newCamPos = new Vector3(Camera.main.transform.position.x - xDifference, Camera.main.transform.position.y - yDifference, -10);
            Camera.main.transform.position = newCamPos;
        }
    }

    private void MiddleMouseDown(){
        previousMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        moveCam = true;
    }

    private void MiddleMouseUp(){
        moveCam = false;
    }

    private void MouseScroll(float scrollDelta){
        if (scrollDelta > 0f && Camera.main.orthographicSize > ToolSettings.Instance.MinCamSize)
        {
            Camera.main.orthographicSize -= Camera.main.orthographicSize * ToolSettings.Instance.ScrollSpeed * 0.01f;
        }

        if (scrollDelta < 0f && Camera.main.orthographicSize < ToolSettings.Instance.MaxCamSize)
        {
            Camera.main.orthographicSize += Camera.main.orthographicSize * ToolSettings.Instance.ScrollSpeed * 0.01f;
        }
    }
}
