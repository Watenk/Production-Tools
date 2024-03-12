using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : IUpdateable
{
    private List<Canvas> canvases = new List<Canvas>();
    private Canvas selectedCanvas;

    private Vector2Int a4 = new Vector2Int(2480, 3508);

    //----------------------------------------------

    public CanvasManager(){
        AddCanvas(a4);
        GameManager.GetService<SaveManager>().Save("test");

        SetPixel(new Vector2Int(2, 2), Color.red);
    }

    public void OnUpdate(){
        foreach (Canvas current in canvases){
            current.OnUpdate();
        }
    }

    public void AddCanvas(Vector2Int size){
        Canvas newCanvas = new Canvas("New Canvas", size);
        selectedCanvas = newCanvas;
        canvases.Add(newCanvas);
    }
    
    public void SetPixel(Vector2Int pos, Color color){
        selectedCanvas.SetPixel(pos, color);
    }
}
