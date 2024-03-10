using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saveable : ISaveable
{
    public Saveable(){
        GameManager.GetService<SaveManager>().AddSaveable(this);
    }

    public virtual void Load(CanvasSaveFile saveFile){

    }

    public virtual void Save(CanvasSaveFile saveFile){

    }
}
