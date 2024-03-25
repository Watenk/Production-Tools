using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorHistory : IHistory
{
    public int Count { get; private set; }

    private Dictionary<Vector2Int, Color> newColors = new Dictionary<Vector2Int, Color>();
    private Dictionary<Vector2Int, Color> oldColors = new Dictionary<Vector2Int, Color>();

    // Dependencies
    private Canvas canvas;

    //-----------------------------------

    public ColorHistory(Canvas canvas){
        this.canvas = canvas;
    }

    public void SetPixel(Vector2Int pos, Color oldColor, Color newColor){
        newColors.TryAdd(pos, newColor);
        oldColors.TryAdd(pos, oldColor);
        Count++;
    }

    public void Redo(){
        foreach (var current in newColors){
            canvas.SetPixel(current.Key, current.Value);
        }
    }

    public void Undo(){
        foreach (var current in oldColors){
            canvas.SetPixel(current.Key, current.Value);
        }
    }
}
