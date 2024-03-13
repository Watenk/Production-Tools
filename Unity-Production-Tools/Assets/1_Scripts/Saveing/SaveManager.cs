using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Linq;
using System;

public class SaveManager
{

    //-------------------------------------------------

    public void Save(string fileName){
        
        // Get data
        CanvasSaveFile saveFile = new CanvasSaveFile();
        GameManager.GetService<CanvasManager>().CurrentCanvas.Save(saveFile);

        // Write Json
        Directory.CreateDirectory(GetPath() + "/saves/" + fileName);
        StreamWriter writer = new StreamWriter(GetPath() + "/saves/" + fileName + "/" + fileName +".txt", false);
        writer.WriteLine(JsonUtility.ToJson(saveFile, true));
        writer.Close();
        writer.Dispose();

        // Write PNG's
        foreach (var current in saveFile.Layers){
             byte[] bytes = current.Value.Texture.EncodeToPNG();
             File.WriteAllBytes(GetPath() + "/saves/" + fileName + "/" + current.Key.ToString() + ".png", bytes);
        }

        Debug.Log("Saved to " + GetPath() + "/saves/" + fileName + "/" + fileName + ".txt");
    }

    public void Load(string fileName){

        // Get Data
        string json = File.ReadAllText(GetPath() + "/saves/" + fileName + "/" + fileName +".txt");
        CanvasSaveFile saveFile = JsonUtility.FromJson<CanvasSaveFile>(json);

        // Load Layers
        for (int i = 0; i < saveFile.LayerCount; i++){
            if (!File.Exists(GetPath() + "/saves/" + fileName + "/" + i.ToString() + ".png")) { Debug.LogError("Couldn't find layer " + i.ToString()); }
            byte[] layerData = File.ReadAllBytes(GetPath() + "/saves/" + fileName + "/" + i.ToString() + ".png");
            Texture2D texture = new Texture2D(saveFile.Size.x, saveFile.Size.y);
            texture.LoadImage(layerData);
            saveFile.Layers.Add(i, new Layer(texture, i));
        }

        // Set Data
        GameManager.GetService<CanvasManager>().CurrentCanvas.Load(saveFile);

        Debug.Log("Loaded");
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

