using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Layer
{
    private string name;
    private int index;
    public Vector2Int size;
    public Cell[,] cells;

    //-----------------------------------

    public Layer(Vector2Int size, int index){
        this.size = size;
        this.index = index;

        cells = new Cell[size.x, size.y];
        for (int y = 0; y < size.y; y++){
            for (int x = 0; x < size.x; x++){
                cells[x,y] = new Cell();
            }
        }
    }

    public Vector2Int GetSize(){
        return size;
    }

    public Cell GetCell(Vector2Int pos){
        if (!IsInLayerBounds(pos)) { return null; }

        return cells[pos.x, pos.y];
    }

    public bool IsInLayerBounds(Vector2Int pos){
        if (pos.x < 0 || pos.x >= size.x) { return false; }
        if (pos.y < 0 || pos.y >= size.y) { return false; }
        return true;
    }
}
