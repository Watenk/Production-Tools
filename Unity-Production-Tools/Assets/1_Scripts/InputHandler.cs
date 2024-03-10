using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : IUpdateable
{
    public event Action OnLeftMouse;
    public event Action OnLeftMouseDown;
    public event Action OnLeftMouseUp;
    public event Action OnRightMouseDown;
    public event Action OnRightMouseUp;
    public event Action OnMiddleMouseDown;
    public event Action OnMiddleMouseUp;
    public event Action OnMousePosChange;
    public event Action<float> OnMouseScroll;

    private Vector2 previousMousePos;

    //--------------------------------------------------------

    public void OnUpdate(){
        MouseInputs();
    }

    //---------------------------------------------------------

    private void MouseInputs(){
        if (Input.GetKey(KeyCode.Mouse0) && OnLeftMouse != null) { OnLeftMouse(); }
        if (Input.GetKeyDown(KeyCode.Mouse0) && OnLeftMouseDown != null) { OnLeftMouseDown(); }
        if (Input.GetKeyUp(KeyCode.Mouse0) && OnLeftMouseUp != null) { OnLeftMouseUp(); }
        if (Input.GetKeyDown(KeyCode.Mouse1) && OnRightMouseDown != null) { OnRightMouseDown(); }
        if (Input.GetKeyUp(KeyCode.Mouse1) && OnRightMouseUp != null) { OnRightMouseUp(); }
        if (Input.GetKeyDown(KeyCode.Mouse2) && OnMiddleMouseDown != null) { OnMiddleMouseDown(); }
        if (Input.GetKeyUp(KeyCode.Mouse2) && OnMiddleMouseUp != null) { OnMiddleMouseUp(); }

        if (Input.mouseScrollDelta.y != 0) { OnMouseScroll(Input.mouseScrollDelta.y); }
        
        Vector2 mousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        if (mousePos != previousMousePos){
            previousMousePos = mousePos;
            OnMousePosChange();
        }
    }
}