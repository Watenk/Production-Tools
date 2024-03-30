using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// I know it can be better but it wasn't really a priority

public class InputHandler : IUpdateable
{
    private Vector2 previousMousePos;
    private bool updateInputs = true;

    //--------------------------------------------------------

    public InputHandler(){
        EventManager.AddListener(Events.OnStopInputs, () => updateInputs = false);
        EventManager.AddListener(Events.OnResumeInputs, () => updateInputs = true);
    }

    public void OnUpdate(){
        if (updateInputs) MouseInputs();
    }

    //---------------------------------------------------------

    private void MouseInputs(){
        if (Input.GetKey(KeyCode.Mouse0))     { EventManager.Invoke(Events.OnLeftMouse); }
        if (Input.GetKeyDown(KeyCode.Mouse0)) { EventManager.Invoke(Events.OnLeftMouseDown); }
        if (Input.GetKeyUp(KeyCode.Mouse0))   { EventManager.Invoke(Events.OnLeftMouseUp); }
        if (Input.GetKeyDown(KeyCode.Mouse1)) { EventManager.Invoke(Events.OnRightMouseDown); }
        if (Input.GetKeyUp(KeyCode.Mouse1))   { EventManager.Invoke(Events.OnRightMouseUp); }
        if (Input.GetKeyDown(KeyCode.Mouse2)) { EventManager.Invoke(Events.OnMiddleMouseDown); }
        if (Input.GetKeyUp(KeyCode.Mouse2))   { EventManager.Invoke(Events.OnMiddleMouseUp); }
        if (Input.GetKeyDown(KeyCode.Space))  { EventManager.Invoke(Events.OnSpaceDown); }
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.S)) { EventManager.Invoke(Events.OnSave); }
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.E)) { EventManager.Invoke(Events.OnExport); }
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.L)) { EventManager.Invoke(Events.OnLoad); }
        if (Input.GetKey(KeyCode.LeftControl) && !Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Z)) { EventManager.Invoke(Events.OnUndo); }
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Z))  { EventManager.Invoke(Events.OnRedo); }

        if (Input.mouseScrollDelta.y != 0) { EventManager.Invoke(Events.OnMouseScroll, Input.mouseScrollDelta.y); }
        
        Vector2 mousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        if (mousePos != previousMousePos){
            previousMousePos = mousePos;
            EventManager.Invoke(Events.OnMousePosChange);
        }
    }
}