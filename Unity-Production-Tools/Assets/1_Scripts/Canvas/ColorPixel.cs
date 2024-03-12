using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorPixel
{
    public Vector2Int pos;
    public Color color;

    //---------------------------------------

    public ColorPixel(Vector2Int pos, Color color){
        this.pos = pos;
        this.color = color;
    }
}
