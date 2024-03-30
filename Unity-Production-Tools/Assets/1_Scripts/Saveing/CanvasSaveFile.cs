using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct CanvasSaveFile
{
    public string SaveLocation;
    public string Name;
    public Vector2Int Size;
    public Dictionary<int, Layer> Layers;
    public int LayerCount;
    public List<string> LayerNames;
}