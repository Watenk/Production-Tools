using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager
{
    private List<Canvas> canvases = new List<Canvas>();
    private Vector2Int a4 = new Vector2Int(2480, 3508);

    //----------------------------------------------

    public CanvasManager(){
        AddCanvas(new Vector2Int(32, 32));
    }

    public void AddCanvas(Vector2Int size){
        canvases.Add(new Canvas(size));
    }

    public void SaveCanvas(string name){

    }

    public void LoadCanvas(string name){

    }
}
