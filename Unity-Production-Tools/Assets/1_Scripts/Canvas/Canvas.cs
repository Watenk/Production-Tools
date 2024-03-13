using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canvas : ISaveable, IUpdateable
{
    public Vector2Int Size { get; private set; }

    private string name;
    private Dictionary<int, Layer> layers = new Dictionary<int, Layer>();
    private Layer currentLayer;

    //------------------------------------------------

    public Canvas(string name, Vector2Int size){
        this.name = name;
        this.Size = size;
        
        AddLayer();
    }

    public void OnUpdate(){
        foreach (var current in layers){
            current.Value.OnUpdate();
        }
    }

    public void Save(CanvasSaveFile saveFile){
        saveFile.Name = name;
        saveFile.Size = Size;
        saveFile.Layers = layers;
        saveFile.LayerCount = layers.Count;
    }

    public void Load(CanvasSaveFile saveFile){

        Clear();

        name = saveFile.Name;
        Size = saveFile.Size;
        layers = saveFile.Layers;
    }

    public void AddLayer(){
        Layer newLayer = new Layer("New Layer", Size, layers.Count);
        layers.Add(layers.Count, newLayer);
        currentLayer = newLayer;
    }

    public void SetPixel(Vector2Int pos, Color color){
        currentLayer.SetPixel(pos, color);
    }

    public void Clear(){
        foreach (var current in layers){
            current.Value.Clear();
        }
    }
}
