using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Linq;
using System;

public class SaveManager
{
    private List<Saveable> saveables = new List<Saveable>();

    //-------------------------------------------------

    public void Save(string fileName){
        
        CanvasSaveFile saveFile = new CanvasSaveFile();

        foreach (Saveable currentSaveable in saveables){
            currentSaveable.Save(saveFile);
        }

        StreamWriter writer = new StreamWriter(GetPath(fileName), true);
        writer.WriteLine(JsonUtility.ToJson(saveFile, true));
        writer.Close();
        writer.Dispose();
    }

    public void Load(string fileName){

    }

    public void AddSaveable(Saveable saveable){
        saveables.Add(saveable);
    }

    //----------------------------------------------------

    private string GetPath(string fileName){
        if (Application.isEditor){
            return Application.dataPath + fileName + ".txt";
        }
        else{
            return Application.persistentDataPath + fileName + ".txt";
        }
    }
}

