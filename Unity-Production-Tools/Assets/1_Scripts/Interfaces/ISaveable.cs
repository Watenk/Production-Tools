using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISaveable
{
    public SaveChunk Save();
    public void Load();
}
