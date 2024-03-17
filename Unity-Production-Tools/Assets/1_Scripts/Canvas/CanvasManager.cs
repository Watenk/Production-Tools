using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SFB;

public class CanvasManager : IUpdateable
{
    private List<Canvas> canvases = new List<Canvas>();
    private Canvas currentCanvas;

    // Sizes
    private Vector2Int a4 = new Vector2Int(2480, 3508);

    //----------------------------------------------

    public CanvasManager(){

        EventManager.AddListener("OnLeftMouse", OnLeftMouse);
        EventManager.AddListener("OnSave", SaveCanvas);
        EventManager.AddListener("OnLoad", LoadCanvas);
        EventManager.AddListener<Vector2Int>("OnNewCanvasClicked", NewCanvas);
        EventManager.AddListener<Canvas>("OnSwitchCanvasClicked", SwitchCanvas);
        EventManager.AddListener<Canvas>("OnRemoveCanvasClicked", RemoveCanvas);
    }

    public void OnUpdate(){
        if (currentCanvas != null) currentCanvas.OnUpdate();
    }

    //----------------------------------------------

    private void OnLeftMouse(){

        if (currentCanvas == null) return;

        Vector3 camPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2Int pos = new Vector2Int((int)camPos.x, currentCanvas.Size.y - (int)camPos.y - 1);
        SetPixel(pos, Color.red);
    }

    private void NewCanvas(Vector2Int size){
        Canvas newCanvas = new Canvas(size);
        canvases.Add(newCanvas);
        EventManager.Invoke("OnNewCanvas", newCanvas);
        SwitchCanvas(newCanvas);
    }

    private void LoadCanvas(){
        Canvas loadedCanvas = SaveManager.Load();
        if (loadedCanvas == null) return;
        canvases.Add(loadedCanvas);
        EventManager.Invoke("OnLoadCanvas", loadedCanvas);
        SwitchCanvas(loadedCanvas);
    }

    private void SaveCanvas(){
        SaveManager.Save(currentCanvas);
    }

    private void RemoveCanvas(Canvas canvas){
        canvases.Remove(canvas);
        if (canvases.Count != 0) { SwitchCanvas(canvases[canvases.Count - 1]); }
        else currentCanvas = null;
        canvas.Remove();
    }

    private void SwitchCanvas(Canvas canvas){
        if (currentCanvas != null) currentCanvas.SetLayersActive(false);
        currentCanvas = canvas;
        currentCanvas.SetLayersActive(true);
        
        EventManager.Invoke("OnSwitchTab", currentCanvas);
    }

    private void SetPixel(Vector2Int pos, Color color){
        currentCanvas.SetPixel(pos, color);
    }
}