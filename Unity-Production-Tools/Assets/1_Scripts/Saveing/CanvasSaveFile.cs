using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CanvasSaveFile
{
    public string Name;
    public Vector2Int Size;
    public List<Layer> Layers = new List<Layer>();
}