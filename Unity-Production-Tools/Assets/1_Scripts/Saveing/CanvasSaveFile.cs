using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CanvasSaveFile
{
    public string Name;
    public Vector2Int Size;
    public Dictionary<int, Layer> Layers = new Dictionary<int, Layer>();
    public int LayerCount;
}