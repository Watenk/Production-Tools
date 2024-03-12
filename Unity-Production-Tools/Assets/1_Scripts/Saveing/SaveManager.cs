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

        Directory.CreateDirectory(GetPath() + "/saves/" + fileName);
        StreamWriter writer = new StreamWriter(GetPath() + "/saves/" + fileName + "/" + fileName +".txt", false);
        writer.WriteLine(JsonUtility.ToJson(saveFile, true));
        writer.Close();
        writer.Dispose();

        Debug.Log("Saved " + saveables.Count + " objects to " + GetPath() + "/saves/" + fileName + "/" + fileName + ".txt");
    }

    public void Load(string fileName){

        Debug.Log("Loaded");
    }

    public void AddSaveable(Saveable saveable){
        saveables.Add(saveable);
    }

    //----------------------------------------------------
    
    private string GetPath(){
        if (Application.isEditor){
            return Application.dataPath;
        }
        else{
            return Application.persistentDataPath;
        }
    }
}

