using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISaveable
{
    public void Save(SaveFile saveFile);
    public void Load(SaveFile saveFile);
}
