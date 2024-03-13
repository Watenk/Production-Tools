using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;
using SFB;

public class SaveManager
{

    //-------------------------------------------------

    public void Save(string fileName){
        
        // Get data
        CanvasSaveFile saveFile = new CanvasSaveFile();
        GameManager.GetService<CanvasManager>().CurrentCanvas.Save(saveFile);

        // Get Save Location
        if (saveFile.SaveLocation == null){
            saveFile.SaveLocation = StandaloneFileBrowser.SaveFilePanel(saveFile.Name, saveFile.Name, "NewDrawing", "txt");
        }
        if (!Directory.Exists(saveFile.SaveLocation)){
            saveFile.SaveLocation = StandaloneFileBrowser.SaveFilePanel(saveFile.Name, saveFile.Name, "NewDrawing", "txt");
            Directory.CreateDirectory(saveFile.SaveLocation);
        }

        // Write Json
        Directory.CreateDirectory(GetConfigPath() + "/saves/" + fileName);
        StreamWriter writer = new StreamWriter(GetConfigPath() + "/saves/" + fileName + "/" + fileName +".txt", false);
        writer.WriteLine(JsonUtility.ToJson(saveFile, true));
        writer.Close();
        writer.Dispose();

        // Write PNG's
        foreach (var current in saveFile.Layers){
             byte[] bytes = current.Value.Texture.EncodeToPNG();
             File.WriteAllBytes(GetConfigPath() + "/saves/" + fileName + "/" + current.Key.ToString() + ".png", bytes);
        }

        Debug.Log("Saved to " + GetConfigPath() + "/saves/" + fileName + "/" + fileName + ".txt");
    }

    public void Load(string fileName){

        // Get Data
        string json = File.ReadAllText(GetConfigPath() + "/saves/" + fileName + "/" + fileName +".txt");
        CanvasSaveFile saveFile = JsonUtility.FromJson<CanvasSaveFile>(json);

        // Load Layers
        for (int i = 0; i < saveFile.LayerCount; i++){
            if (!File.Exists(GetConfigPath() + "/saves/" + fileName + "/" + i.ToString() + ".png")) { Debug.LogError("Couldn't find layer " + i.ToString()); }
            byte[] layerData = File.ReadAllBytes(GetConfigPath() + "/saves/" + fileName + "/" + i.ToString() + ".png");
            Texture2D texture = new Texture2D(saveFile.Size.x, saveFile.Size.y);
            texture.LoadImage(layerData);
            saveFile.Layers.Add(i, new Layer(texture, i));
        }

        // Set Data
        GameManager.GetService<CanvasManager>().CurrentCanvas.Load(saveFile);

        Debug.Log("Loaded");
    }

    //----------------------------------------------------
    
    private string GetConfigPath(){
        if (Application.isEditor){
            return Application.dataPath;
        }
        else{
            return Application.persistentDataPath;
        }
    }
}

