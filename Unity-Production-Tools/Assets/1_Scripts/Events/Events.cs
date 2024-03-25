using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Events
{
    // Input
    OnLeftMouse,
    OnLeftMouseDown,
    OnLeftMouseUp,
    OnRightMouseDown,
    OnRightMouseUp,
    OnMiddleMouseDown,
    OnMiddleMouseUp,
    OnSpaceDown,
    OnMouseScroll,
    OnMousePosChange,
    OnSave,
    OnLoad,
    OnUndo,
    OnRedo,

    // Canvas
    OnNewCanvas,
    OnLoadCanvas,
    OnSwitchTab,

    // UI
    OnNewCanvasClicked,
    OnSwitchCanvasClicked,
    OnRemoveCanvasClicked,
    OnCurrentColorChanged,
}
