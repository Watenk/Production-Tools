using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : IUpdateable
{
    // public event Action OnUndo;
    // public event Action OnRedo;

    private Vector2 previousMousePos;

    //--------------------------------------------------------

    public void OnUpdate(){
        MouseInputs();
    }

    //---------------------------------------------------------

    private void MouseInputs(){
        if (Input.GetKey(KeyCode.Mouse0)) { EventManager.Invoke("OnLeftMouse"); }
        if (Input.GetKeyDown(KeyCode.Mouse0)) { EventManager.Invoke("OnLeftMouseDown"); }
        if (Input.GetKeyUp(KeyCode.Mouse0)) { EventManager.Invoke("OnLeftMouseUp"); }
        if (Input.GetKeyDown(KeyCode.Mouse1)) { EventManager.Invoke("OnRightMouseDown"); }
        if (Input.GetKeyUp(KeyCode.Mouse1)) { EventManager.Invoke("OnRightMouseUp"); }
        if (Input.GetKeyDown(KeyCode.Mouse2)) { EventManager.Invoke("OnMiddleMouseDown"); }
        if (Input.GetKeyUp(KeyCode.Mouse2)) { EventManager.Invoke("OnMiddleMouseUp"); }
        if (Input.GetKeyDown(KeyCode.Space)) { EventManager.Invoke("OnSpaceDown"); }
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.S)) { EventManager.Invoke("OnSave"); }
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.L)) { EventManager.Invoke("OnLoad"); }

        if (Input.mouseScrollDelta.y != 0) { EventManager.Invoke("OnMouseScroll", Input.mouseScrollDelta.y); }
        
        Vector2 mousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        if (mousePos != previousMousePos){
            previousMousePos = mousePos;
            EventManager.Invoke("OnMousePosChange");
        }
    }
}