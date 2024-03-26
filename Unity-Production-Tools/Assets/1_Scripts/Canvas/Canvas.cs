using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        Size = size;
        HistoryManager = new HistoryManager();

        Layer layer = AddLayer();
        layer.GenerateBackground();
    }

    // Load Canvas from Save
    public Canvas(CanvasSaveFile saveFile){
        name = saveFile.Name;
        Size = saveFile.Size;
        layers = saveFile.Layers;
        saveLocation = saveFile.SaveLocation;
        SwitchLayer(saveFile.Layers[0]);
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

    public Layer AddLayer(){
        Layer newLayer = new Layer("New Layer", layers.Count, this);
        layers.Add(layers.Count, newLayer);
        EventManager.Invoke(Events.OnNewLayer, newLayer);
        SwitchLayer(newLayer);
        return newLayer;
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
        if (CurrentLayer != null) EventManager.Invoke(Events.OnLayerSwitchBackgroundToWhite, CurrentLayer);
        EventManager.Invoke(Events.OnLayerSwitchBackgroundToGray, layer);
        CurrentLayer = layer;
    }

    public void PromoteLayer(Layer layer){
        if (layer.Index == layers.Count - 1) return;

        
    }

    public void DemoteLayer(Layer layer){
        if (layer.Index == 0) return;


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
