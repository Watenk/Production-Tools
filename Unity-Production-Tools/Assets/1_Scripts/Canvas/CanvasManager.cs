using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : IUpdateable
{
    public Canvas CurrentCanvas { get; private set; }

    private List<Canvas> canvases = new List<Canvas>();

    // Sizes
    private Vector2Int a4 = new Vector2Int(2480, 3508);

    // References
    private SaveManager saveManager;
    private InputHandler inputHandler;

    //----------------------------------------------

    public CanvasManager(){

        saveManager = GameManager.GetService<SaveManager>();
        inputHandler = GameManager.GetService<InputHandler>();

        inputHandler.OnLeftMouseDown += OnLeftMouseDown;
        inputHandler.OnRightMouseDown += OnRightMouseDown;
        inputHandler.OnSpaceDown += OnSpaceDown;

        AddCanvas(a4);
    }

    public void OnUpdate(){
        foreach (Canvas current in canvases){
            current.OnUpdate();
        }
    }

    public void AddCanvas(Vector2Int size){
        Canvas newCanvas = new Canvas("New Canvas", size);
        CurrentCanvas = newCanvas;
        canvases.Add(newCanvas);
    }
    
    public void SetPixel(Vector2Int pos, Color color){
        CurrentCanvas.SetPixel(pos, color);
    }

    //----------------------------------------------

    private void OnLeftMouseDown(){
        Vector3 camPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2Int pos = new Vector2Int((int)camPos.x, CurrentCanvas.Size.y - (int)camPos.y - 1);
        SetPixel(pos, Color.red);
    }

    private void OnRightMouseDown(){
        saveManager.Save();
    }

    private void OnSpaceDown(){
        saveManager.Load();
    }
}
