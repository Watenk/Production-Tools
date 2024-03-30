using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Canvas
{
    public Vector2Int Size { get; private set; }
    public HistoryManager HistoryManager { get; private set; }
    public Layer CurrentLayer { get; private set; }
    public string Name { get; private set; }

    // CanvasData
    private string saveLocation;
    private Dictionary<int, Layer> layers = new Dictionary<int, Layer>();
    private Layer background;
    
    //------------------------------------------------

    public Canvas(Vector2Int size){
        Size = size;
        HistoryManager = new HistoryManager();
        background = new Layer("White Background", 0, Size, Color.white);
        background.GameObject.transform.position = new Vector3(background.GameObject.transform.position.x, background.GameObject.transform.position.y, 1.0f);

        AddLayer(new Color(0.8f, 0.8f, 0.8f, 1.0f), "Background");
    }

    // Load Canvas from Save
    public Canvas(CanvasSaveFile saveFile){
        Name = saveFile.Name;
        Size = saveFile.Size;
        layers = saveFile.Layers;
        saveLocation = saveFile.SaveLocation;
        HistoryManager = new HistoryManager();
        SwitchLayer(saveFile.Layers[0]);
    }

    public void Remove(){
        foreach (var current in layers){
            current.Value.Delete();
        }
        if (background != null) GameObject.Destroy(background.GameObject);
    }

    public void SaveTo(ref CanvasSaveFile saveFile){
        saveFile.Name = Name;
        saveFile.Size = Size;
        saveFile.Layers = layers;
        saveFile.LayerCount = layers.Count;
        saveFile.SaveLocation = saveLocation;
        List<string> layerNames = new List<string>();
        foreach (Layer current in layers.Values){
            layerNames.Add(current.Name);
        }
        saveFile.LayerNames = layerNames;
    }

    public Layer AddLayer(Color color){
        Layer newLayer = new Layer("New Layer", layers.Count, Size, color);
        layers.Add(layers.Count, newLayer);
        SwitchLayer(newLayer);
        return newLayer;
    }

    public Layer AddLayer(Color color, string name){
        Layer newLayer = new Layer(name, layers.Count, Size, color);
        layers.Add(layers.Count, newLayer);
        SwitchLayer(newLayer);
        return newLayer;
    }

    public Dictionary<int, Layer> GetLayers(){
        return layers;
    }

    public void RemoveLayer(){
        if (CurrentLayer == null) return;

        EventManager.Invoke(Events.OnRemoveLayer, CurrentLayer);
        layers.Remove(CurrentLayer.Index);
        CurrentLayer.Delete();
        CurrentLayer = null;

        ReCalcLayerIndexes();
        
        if (layers.Count > 0){
            layers.TryGetValue(layers.Count - 1, out Layer newCurrentLayer);
            SwitchLayer(newCurrentLayer);
        } 
        else{
            CurrentLayer = null;
        }
    }

    public void SwitchLayer(Layer layer){
        if (CurrentLayer != null) EventManager.Invoke(Events.OnLayerChangeColor, CurrentLayer, Color.white);
        EventManager.Invoke(Events.OnLayerChangeColor, layer, new Color(140f / 255f, 154f / 255f, 176f / 255f));
        CurrentLayer = layer;
    }

    public void PromoteLayer(Layer layer){
        if (layers.Count == 1) return;
        if (layer.Index == layers.Count - 1) return;

    }

    public void DemoteLayer(Layer layer){
        if (layers.Count == 1) return;
        if (layer.Index == 0) return;

    }

    public Color GetPixel(Vector2Int pos){
        if (CurrentLayer == null) return Color.magenta;
        return CurrentLayer.GetPixel(pos);
    }

    public void SetPixel(Vector2Int pos, Color color){
        if (CurrentLayer == null) return;
        CurrentLayer.SetPixel(pos, color, true);
    }

    public void SetLayersActive(bool value){
        foreach (var current in layers){
            current.Value.SetActive(value);
        }
    }

    public void SetName(string newName){
        Name = newName;
    }

    //----------------------------------

    private void ReCalcLayerIndexes(){

        var sortedKeys = layers.Keys.OrderBy(key => key).ToList();
        Dictionary<int, Layer> orderedLayers = new Dictionary<int, Layer>();
        int newIndex = 0;
        foreach (var key in sortedKeys){
            orderedLayers[newIndex] = layers[key];
            orderedLayers[newIndex].SetIndex(newIndex);
            newIndex++;
        }

        layers = orderedLayers;
    }
}
