using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canvas : Saveable, IUpdateable
{
    private string name;
    private Vector2Int size;
    private List<Layer> layers = new List<Layer>();
    private Layer selectedLayer;

    //------------------------------------------------

    public Canvas(string name, Vector2Int size){
        this.name = name;
        this.size = size;
        
        AddLayer();
    }

    public void OnUpdate(){
        foreach (Layer current in layers){
            current.OnUpdate();
        }
    }

    public override void Save(CanvasSaveFile saveFile){
        saveFile.Name = name;
        saveFile.Size = size;
        saveFile.Layers = layers;
    }

    public override void Load(CanvasSaveFile saveFile){
        name = saveFile.Name;
        size = saveFile.Size;
        layers = saveFile.Layers;
    }

    public void AddLayer(){
        Layer newLayer = new Layer("New Layer", size, layers.Count);
        layers.Add(newLayer);
        selectedLayer = newLayer;
    }

    public void SetPixel(Vector2Int pos, Color color){
        selectedLayer.SetPixel(pos, color);
    }
}
