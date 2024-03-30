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
    OnExport,
    OnLoad,
    OnUndo,
    OnRedo,
    OnStopInputs,
    OnResumeInputs,

    // Canvas
    OnNewCanvas,
    OnLoadCanvas,
    OnSwitchTab,

    // Layers
    OnNewLayer,
    OnRemoveLayer,
    OnLayerChangeColor,

    // UI
    // Canvas
    OnNewCanvasClicked,
    OnSwitchCanvasClicked,
    OnRemoveCanvasClicked,
    // Color Picker
    OnCurrentColorChanged,
    OnBrushAlphaChanged,
    // Layers
    OnAddLayerClicked,
    OnRemoveLayerClicked,
    OnPromoteLayerClicked,
    OnDemoteLayerClicked,
    OnLayerAlphaChanged,
    OnLayerVisiblityClicked,
    OnLayerSelectClicked,
    OnCanvasNull,
}
