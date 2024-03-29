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

    // Layers
    OnNewLayer,
    OnRemoveLayer,
    OnLayerSwitchBackgroundToGray,
    OnLayerSwitchBackgroundToWhite,

    // UI
    // Canvas
    OnNewCanvasClicked,
    OnSwitchCanvasClicked,
    OnRemoveCanvasClicked,
    // Color Picker
    OnCurrentColorChanged,
    // Layers
    OnAddLayerClicked,
    OnRemoveLayerClicked,
    OnPromoteLayerClicked,
    OnDemoteLayerClicked,
    OnLayerAlphaChanged,
    OnLayerVisiblityClicked,
    OnLayerLockClicked,
    OnLayerSelectClicked,
}
