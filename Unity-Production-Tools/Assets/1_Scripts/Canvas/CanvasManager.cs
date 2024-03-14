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

    //----------------------------------------------

    public CanvasManager(){

        inputHandler = GameManager.GetService<InputHandler>();

        inputHandler.OnLeftMouse += OnLeftMouse;
        inputHandler.OnRightMouseDown += OnRightMouseDown;
        inputHandler.OnSpaceDown += OnSpaceDown;
    }

    public void OnUpdate(){
        if (currentCanvas != null) currentCanvas.OnUpdate();
    }

    public Canvas NewCanvas(Vector2Int size){
        Canvas newCanvas = new Canvas(size);
        canvases.Add(newCanvas);
        SwitchCanvas(newCanvas);
        return newCanvas;
    }

    public Canvas LoadCanvas(){
        Canvas loadedCanvas = SaveManager.Load();
        canvases.Add(loadedCanvas);
        SwitchCanvas(loadedCanvas);
        return loadedCanvas;
    }

    public void SaveCanvas(Canvas savedCanvas){
        SaveManager.Save(savedCanvas);
    }

    public void SwitchCanvas(Canvas canvas){
        if (currentCanvas != null) currentCanvas.SetLayersActive(false);
        currentCanvas = canvas;
        currentCanvas.SetLayersActive(true);
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

    private void OnRightMouseDown(){
        SaveCanvas(currentCanvas);
    }

    private void OnSpaceDown(){
        LoadCanvas();
    }
}
