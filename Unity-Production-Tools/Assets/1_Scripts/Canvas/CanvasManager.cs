using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Watenk;

public class CanvasManager
{
    private List<Canvas> canvases = new List<Canvas>();
    private Canvas currentCanvas;
    private Color currentColor = Color.black;
    private float currentAlpha = 1.0f;

    // History
    private ColorHistory colorHistory;

    //----------------------------------------------

    public CanvasManager(){

        // Mouse Inputs
        EventManager.AddListener(Events.OnLeftMouse, OnLeftMouse);
        EventManager.AddListener(Events.OnLeftMouseUp, OnLeftMouseUp);

        // Keyboard Inputs
        EventManager.AddListener(Events.OnSave, () => SaveManager.Save(currentCanvas));
        EventManager.AddListener(Events.OnExport, () => SaveManager.Export(currentCanvas));
        EventManager.AddListener(Events.OnLoad, LoadCanvas);
        EventManager.AddListener(Events.OnUndo, OnUndo);
        EventManager.AddListener(Events.OnRedo, OnRedo);

        // UI Inputs
        // Canvas
        EventManager.AddListener<Vector2Int>(Events.OnNewCanvasClicked, NewCanvas);
        EventManager.AddListener<Canvas>(Events.OnSwitchCanvasClicked, SwitchCanvas);
        EventManager.AddListener<Canvas>(Events.OnRemoveCanvasClicked, RemoveCanvas);
        // Color Picker
        EventManager.AddListener<Color>(Events.OnCurrentColorChanged, OnCurrentColorChanged);
        EventManager.AddListener<float>(Events.OnBrushAlphaChanged, (value) => currentAlpha = value);
        // Layers
        EventManager.AddListener(Events.OnAddLayerClicked, () => currentCanvas?.AddLayer(new Color(1.0f, 1.0f, 1.0f, 0.0f)));
        EventManager.AddListener(Events.OnRemoveLayerClicked, () => currentCanvas?.RemoveLayer());
        //EventManager.AddListener<Layer>(Events.OnLayerVisiblityClicked, (layer) => currentCanvas?.SwitchLayer(layer));
        //EventManager.AddListener<Layer>(Events.OnLayerLockClicked, (layer) => currentCanvas?.SwitchLayer(layer));
        EventManager.AddListener<Layer>(Events.OnLayerSelectClicked, (layerUITab) => currentCanvas?.SwitchLayer(layerUITab));
    }

    //----------------------------------------------

    private void OnLeftMouse(){

        if (currentCanvas == null) return;
        if (!GridUtil.IsInBounds(
        new Vector2(Input.mousePosition.x, Screen.height -Input.mousePosition.y), 
        new Vector2(References.Instance.ToolBarBackground.sizeDelta.x, References.Instance.TabsBarBackground.sizeDelta.y), 
        new Vector2(Screen.width - References.Instance.RightBarBackground.sizeDelta.x, Screen.height))) 
        return;

        if (colorHistory == null) colorHistory = new ColorHistory(currentCanvas);

        Vector3 camPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2Int pos = new Vector2Int((int)camPos.x, currentCanvas.Size.y - (int)camPos.y - 1);
        SetPixel(pos, currentColor);
    }

    private void OnLeftMouseUp(){
        if (colorHistory == null || colorHistory.Count == 0) return;
        currentCanvas.HistoryManager.AddHistory(colorHistory);
        colorHistory = null;
    }

    private void OnCurrentColorChanged(Color color){
        currentColor = color;
    }

    private void OnUndo(){
        currentCanvas.HistoryManager.Undo();
    }

    private void OnRedo(){
        currentCanvas.HistoryManager.Redo();
    }

    //---------------------------------------------

    private void NewCanvas(Vector2Int size){
        Canvas newCanvas = new Canvas(size);
        canvases.Add(newCanvas);
        EventManager.Invoke(Events.OnNewCanvas, newCanvas);
        SwitchCanvas(newCanvas);
    }

    private void LoadCanvas(){
        Canvas loadedCanvas = SaveManager.Load();
        if (loadedCanvas == null) return;
        canvases.Add(loadedCanvas);
        EventManager.Invoke(Events.OnLoadCanvas, loadedCanvas);
        SwitchCanvas(loadedCanvas);
    }

    private void RemoveCanvas(Canvas canvas){
        canvases.Remove(canvas);
        if (canvases.Count != 0) { SwitchCanvas(canvases[canvases.Count - 1]); }
        else{
            currentCanvas = null;
            EventManager.Invoke(Events.OnCanvasNull);
        }
        canvas.Remove();
    }

    private void SwitchCanvas(Canvas canvas){
        currentCanvas?.SetLayersActive(false);
        currentCanvas = canvas;
        currentCanvas.SetLayersActive(true);
        
        EventManager.Invoke(Events.OnSwitchTab, currentCanvas);
    }

    private void SetPixel(Vector2Int pos, Color newColor){
        newColor.a = currentAlpha;
        Color currentColor = currentCanvas.GetPixel(pos);
        if (currentColor == newColor) return;
        currentCanvas.SetPixel(pos, newColor);
        colorHistory.SetPixel(pos, currentColor, newColor);
    }
}