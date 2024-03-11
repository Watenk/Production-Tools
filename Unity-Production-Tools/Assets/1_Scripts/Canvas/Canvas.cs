using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canvas : Saveable
{
    private string name;
    private Vector2Int size;
    private List<Layer> layers = new List<Layer>();

    // Rendering
    private Dictionary<Layer, LayerRenderer> renderers = new Dictionary<Layer, LayerRenderer>();

    //------------------------------------------------

    public Canvas(Vector2Int size){
        this.size = size;
        
        AddLayer();
    }

    public override void Save(CanvasSaveFile saveFile){

    }

    public override void Load(CanvasSaveFile saveFile){

    }

    public void AddLayer(){
        Layer newLayer = new Layer(size, layers.Count);
        layers.Add(newLayer);
        renderers.Add(newLayer, new LayerRenderer(newLayer));
    }
}
