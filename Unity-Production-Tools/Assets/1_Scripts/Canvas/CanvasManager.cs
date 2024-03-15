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

    // Dependencies
    private InputHandler inputHandler;
    private UIManager uiManager;

    //----------------------------------------------

    public CanvasManager(){

        inputHandler = GameManager.GetService<InputHandler>();
        uiManager = GameManager.GetService<UIManager>();
        uiManager.Init(this);

        inputHandler.OnLeftMouse += OnLeftMouse;
        inputHandler.OnSave += OnSave;
        inputHandler.OnLoad += OnLoad;
    }

    public void OnUpdate(){
        if (currentCanvas != null) currentCanvas.OnUpdate();
    }

    public Canvas NewCanvas(Vector2Int size){
        Canvas newCanvas = new Canvas(size);
        canvases.Add(newCanvas);
        return newCanvas;
    }

    public Canvas LoadCanvas(){
        Canvas loadedCanvas = SaveManager.Load();
        if (loadedCanvas == null) return default;
        canvases.Add(loadedCanvas);
        uiManager.AddTab(loadedCanvas);
        SwitchCanvas(loadedCanvas);
        return loadedCanvas;
    }

    public void SaveCanvas(Canvas savedCanvas){
        SaveManager.Save(savedCanvas);
    }

    public void RemoveCanvas(Canvas canvas){
        canvases.Remove(canvas);
        if (canvases.Count != 0) { SwitchCanvas(canvases[canvases.Count - 1]); }
        else currentCanvas = null;
        canvas.Remove();
    }

    public void SwitchCanvas(Canvas canvas){
        if (currentCanvas == null) {
            currentCanvas = canvas;
            currentCanvas.SetLayersActive(true);
        }
        else{
            currentCanvas.SetLayersActive(false);
            currentCanvas = canvas;
            currentCanvas.SetLayersActive(true);
        }
        uiManager.SwitchTab(canvas);
    }

    public void SetPixel(Vector2Int pos, Color color){
        currentCanvas.SetPixel(pos, color);
    }

    //----------------------------------------------

    private void OnLeftMouse(){

        if (currentCanvas == null) return;

        Vector3 camPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2Int pos = new Vector2Int((int)camPos.x, currentCanvas.Size.y - (int)camPos.y - 1);
        SetPixel(pos, Color.red);
    }

    private void OnLoad(){
        LoadCanvas();
    }

    private void OnSave(){
        SaveCanvas(currentCanvas);
    }
}
