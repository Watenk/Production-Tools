using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : Saveable
{
    public Vector2Int GridSize { get; private set; } // Size of grid in cells

    private Chunk[,] chunks;

    //----------------------------------------------

    public void New(Vector2Int size){
        GridSize = size;
    }

    public override void Load(SaveFile saveFile){

    }

    public override void Save(SaveFile saveFile){

    }
    
    public bool IsInGridBounds(Vector2Int pos){
        if (pos.x < 0 || pos.x >= GridSize.x) { return false; }
        if (pos.y < 0 || pos.y >= GridSize.y) { return false; }
        return true;
    }

    //---------------------------------------------

    private Vector2Int CalcChunkSize(Vector2Int gridSize, Vector2Int desiredChunkSize){
        
        if (gridSize.x < desiredChunkSize.x || gridSize.y < desiredChunkSize.y) { Debug.LogError("GridSize is smaller than ChunkSize"); }
        int xChunkSize = FindClosestDivisor(gridSize.x, desiredChunkSize.x);
        int yChunkSize = FindClosestDivisor(gridSize.y, desiredChunkSize.y);

        return new Vector2Int(xChunkSize, yChunkSize);
    }

    private int FindClosestDivisor(int number, int devisor){

        int maxTries = 1000;
        int remainder = 1;
        int maxDivisorOffset = 0;
        int minDivisorOffset = 0;

        if (devisor > number) { Debug.LogError("Devisor: " + devisor + " is bigger than number: " + number); }
        if (devisor <= 0) { Debug.LogError("Devisor is <= 0"); }

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

        Debug.LogError("Couldn't find a divisor for (Number: " + number + ", Devisor: " + devisor + ")");
        return 0;
    }
}
