using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canvas : IUpdateable
{
    public Vector2Int Size { get; private set; }

    private string saveLocation;
    private string name;
    private Dictionary<int, Layer> layers = new Dictionary<int, Layer>();
    private Layer currentLayer;

    //------------------------------------------------

    public Canvas(Vector2Int size){
        this.Size = size;
    }

    public Canvas(CanvasSaveFile saveFile){
        name = saveFile.Name;
        Size = saveFile.Size;
        layers = saveFile.Layers;
        saveLocation = saveFile.SaveLocation;
        currentLayer = saveFile.Layers[0];
    }

    public void OnUpdate(){
        foreach (var current in layers){
            current.Value.OnUpdate();
        }
    }

    public void SaveTo(CanvasSaveFile saveFile){
        saveFile.Name = name;
        saveFile.Size = Size;
        saveFile.Layers = layers;
        saveFile.LayerCount = layers.Count;
        saveFile.SaveLocation = saveLocation;
    }

    public void AddLayer(){
        Layer newLayer = new Layer("New Layer", Size, layers.Count);
        layers.Add(layers.Count, newLayer);
        currentLayer = newLayer;
    }

    public void SetPixel(Vector2Int pos, Color color){
        currentLayer.SetPixel(pos, color);
    }
}
