using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canvas
{
    public Vector2Int Size { get; private set; }
    public HistoryManager HistoryManager { get; private set; }
    public Layer CurrentLayer { get; private set; }

    // CanvasData
    private string saveLocation;
    private string name;
    private Dictionary<int, Layer> layers = new Dictionary<int, Layer>();
    
    //------------------------------------------------

    public Canvas(Vector2Int size){
        this.Size = size;
        HistoryManager = new HistoryManager();

        AddLayer();
    }

    // Load Canvas from Save
    public Canvas(CanvasSaveFile saveFile){
        name = saveFile.Name;
        Size = saveFile.Size;
        layers = saveFile.Layers;
        saveLocation = saveFile.SaveLocation;
        CurrentLayer = saveFile.Layers[0];
    }

    public void Remove(){
        foreach (var current in layers){
            current.Value.Delete();
        }
    }

    public void SaveTo(ref CanvasSaveFile saveFile){
        saveFile.Name = name;
        saveFile.Size = Size;
        saveFile.Layers = layers;
        saveFile.LayerCount = layers.Count;
        saveFile.SaveLocation = saveLocation;
    }

    public void AddLayer(){
        Layer newLayer = new Layer("New Layer", layers.Count, this);
        layers.Add(layers.Count, newLayer);
        CurrentLayer = newLayer;
    }

    public Color GetPixel(Vector2Int pos){
        if (CurrentLayer == null) return Color.magenta;
        return CurrentLayer.GetPixel(pos);
    }

    public void SetPixel(Vector2Int pos, Color color){
        if (CurrentLayer == null) return;
        CurrentLayer.SetPixel(pos, color);
    }

    public void SetLayersActive(bool value){
        foreach (var current in layers){
            current.Value.SetActive(value);
        }
    }
}
