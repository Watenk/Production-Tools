using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canvas : Saveable
{
    private string name;
    private Vector2Int size;
    private List<Layer> layers = new List<Layer>();

    // Rendering
    private Vector2Int meshChunkSize;
    private Dictionary<Layer, LayerRenderer> renderers = new Dictionary<Layer, LayerRenderer>();

    //------------------------------------------------

    public Canvas(Vector2Int size){
        this.size = size;
        Vector2Int desiredMeshChunkSize = ToolSettings.Instance.DesiredMeshChunkSize;

        #if UNITY_EDITOR
            if (desiredMeshChunkSize.x == 0 || desiredMeshChunkSize.y == 0) { Debug.LogError("DesiredMeshChunkSize in ToolSettings is 0"); }
        #endif

        meshChunkSize = CalcChunkSize(desiredMeshChunkSize);
        AddLayer();
    }

    public override void Save(CanvasSaveFile saveFile){

    }

    public override void Load(CanvasSaveFile saveFile){

    }

    public void AddLayer(){
        Layer newLayer = new Layer(size, layers.Count);
        layers.Add(newLayer);
        renderers.Add(newLayer, new LayerRenderer(newLayer, meshChunkSize));
    }

    //---------------------------------------------------

    private Vector2Int CalcChunkSize(Vector2Int desiredMeshChunkSize){
        
        // If size is smaller than desired size
        if (size.x < desiredMeshChunkSize.x) { desiredMeshChunkSize.x = size.x; }
        if (size.y < desiredMeshChunkSize.y) { desiredMeshChunkSize.y = size.y; }

        int xMeshChunkSize = FindClosestDivisor(size.x, desiredMeshChunkSize.x);
        int yMeshChunkSize = FindClosestDivisor(size.y, desiredMeshChunkSize.y);

        return new Vector2Int(xMeshChunkSize, yMeshChunkSize);
    }

    private int FindClosestDivisor(int number, int devisor){

        int maxTries = 1000;
        int remainder = 1;
        int maxDivisorOffset = 0;
        int minDivisorOffset = 0;

        for (int i = 0; i < maxTries; i++){
            // Check max offset
            if (devisor + maxDivisorOffset < number){
                remainder = number % (devisor + maxDivisorOffset);
                if (remainder == 0) { return devisor + maxDivisorOffset; }
                else { maxDivisorOffset++; }
            }


            // Check min offset
            if (devisor - minDivisorOffset > 0) {  
                remainder = number % (devisor - minDivisorOffset);
                if (remainder == 0) { return devisor - minDivisorOffset; }
                else { minDivisorOffset++; }
            }
        }

        Debug.LogError("CalcChunkSize failed: Couldn't find a divisor for (Number: " + number + ", Devisor: " + devisor + ")");
        return 0;
    }
}
