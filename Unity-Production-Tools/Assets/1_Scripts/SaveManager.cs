using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Linq;

public class SaveManager
{
    private SaveFile saveFile;

    //-------------------------------------------------

    public void Save(){
        
        string savePath = GetPath();
        SaveFile saveFile = GenerateSaveFile();

        StreamWriter writer = new StreamWriter(savePath, true);
        writer.WriteLine(JsonUtility.ToJson(saveFile, true));
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

    private SaveFile GenerateSaveFile(){

        // Get all types from the executing assembly
        var types = System.Reflection.Assembly.GetExecutingAssembly().GetTypes();
        // Filter types that implement ISaveable
        var saveableTypes = types.Where(type => typeof(ISaveable).IsAssignableFrom(type));

        SaveFile saveFile = new SaveFile();

        // Loop through each ISaveable type
        foreach (var currentSaveable in saveableTypes)
        {

        }

        return saveFile;
    }
}

