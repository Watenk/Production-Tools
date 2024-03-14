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

    // References
    private InputHandler inputHandler;

    //----------------------------------------------

    public CanvasManager(){

        inputHandler = GameManager.GetService<InputHandler>();

        inputHandler.OnLeftMouseDown += OnLeftMouse;
        inputHandler.OnRightMouseDown += OnRightMouseDown;
        inputHandler.OnSpaceDown += OnSpaceDown;
    }

    public void OnUpdate(){
        foreach (Canvas current in canvases){
            current.OnUpdate();
        }
    }


    public void NewCanvas(Vector2Int size){
        Canvas newCanvas = new Canvas(size);
        canvases.Add(newCanvas);
        SetCurrentCanvas(newCanvas);
    }

    public void SaveCanvas(Canvas canvas){
        SaveManager.Save(canvas);
    }

    public Canvas LoadCanvas(){
        Canvas loadedCanvas = SaveManager.Load();
        canvases.Add(loadedCanvas);
        SetCurrentCanvas(loadedCanvas);
        return loadedCanvas;
    }

    public void SetCurrentCanvas(Canvas canvas){
        currentCanvas = canvas;
    }

    public void SetPixel(Vector2Int pos, Color color){
        currentCanvas.SetPixel(pos, color);
    }

    //----------------------------------------------

    private void OnLeftMouse(){
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
