using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Linq;
using System;

public class SaveManager
{
    public SaveFile SaveFile { get; private set; }

    private List<Saveable> saveables = new List<Saveable>();

    //-------------------------------------------------

    public void Save(){
        
        foreach (Saveable currentSaveable in saveables){
            currentSaveable.Save(SaveFile);
        }

        StreamWriter writer = new StreamWriter(GetPath(), true);
        writer.WriteLine(JsonUtility.ToJson(SaveFile, true));
        writer.Close();
        writer.Dispose();
    }

    public void AddSaveable(Saveable saveable){
        saveables.Add(saveable);
    }

    //----------------------------------------------------

    private string GetPath(){
        if (Application.isEditor){
            return Application.dataPath + "Save" + ".txt";
        }
        else{
            return Application.persistentDataPath + "Save" + ".txt";
        }
    }
}

