using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISaveable
{
    public void Save(CanvasSaveFile saveFile);
    public void Load(CanvasSaveFile saveFile);
}
