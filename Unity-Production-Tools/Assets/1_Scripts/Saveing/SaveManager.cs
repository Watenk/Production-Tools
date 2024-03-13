using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;
using SFB;

public class SaveManager
{

    //-------------------------------------------------

    public void Save(){
        
        // Get App Data
        CanvasSaveFile saveFile = new CanvasSaveFile();
        GameManager.GetService<CanvasManager>().CurrentCanvas.Save(saveFile);

        // Get Save Location (if neccecary)
        if (saveFile.SaveLocation == null){
            GenerateSaveDir(saveFile);
        }
        if (!Directory.Exists(saveFile.SaveLocation)){
            GenerateSaveDir(saveFile);
        }

        // Write Data Json
        StreamWriter writer = new StreamWriter(saveFile.SaveLocation + "/data.txt", false);
        writer.WriteLine(JsonUtility.ToJson(saveFile, true));
        writer.Close();
        writer.Dispose();

        // Write Layers PNG
        foreach (var current in saveFile.Layers){
             byte[] bytes = current.Value.Texture.EncodeToPNG();
             File.WriteAllBytes(saveFile.SaveLocation + "/layers/" + current.Key.ToString() + ".png", bytes);
        }

        Debug.Log("Saved " + saveFile.Name + " to " + saveFile.SaveLocation);
    }

    public void Load(){

        // Get Save
        string[] savePaths = StandaloneFileBrowser.OpenFolderPanel("", "", false);
        if (savePaths.Length == 0) { Debug.LogWarning("Load Failed... Couldn't find specified dir"); return; }
        string savePath = savePaths[0];

        // Read Data Json
        if (!File.Exists(savePath + "/data.txt")) { Debug.LogWarning("Load Failed... Couldn't find " + savePath + "/data.txt"); return; }
        string json = File.ReadAllText(savePath + "/data.txt");
        CanvasSaveFile saveFile = JsonUtility.FromJson<CanvasSaveFile>(json);

        // Read Layers PNG's
        if (!Directory.Exists(savePath + "/layers")) { Debug.LogWarning("Load Failed... Couldn't find " + savePath + "/layers"); return; }
        for (int i = 0; i < saveFile.LayerCount; i++){
            if (!File.Exists(savePath + "/data.txt")) { Debug.LogWarning("Load Failed... Couldn't find " + savePath + "/layers/" + i + ".png"); return; }
            Texture2D texture = new Texture2D(saveFile.Size.x, saveFile.Size.y);
            byte[] layerData = File.ReadAllBytes(savePath + "/layers/" + i.ToString() + ".png");
            texture.LoadImage(layerData);
            saveFile.Layers.Add(i, new Layer(texture, saveFile.Size, i));
        }

        // Set App Data
        GameManager.GetService<CanvasManager>().CurrentCanvas.Load(saveFile);

        Debug.Log("Loaded " + saveFile.Name + " from " + savePath);
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

    private void GenerateSaveDir(CanvasSaveFile saveFile){
        saveFile.SaveLocation = StandaloneFileBrowser.SaveFilePanel(saveFile.Name, "", "", "");
        Directory.CreateDirectory(saveFile.SaveLocation);
        Directory.CreateDirectory(saveFile.SaveLocation + "/layers");
        saveFile.Name = Path.GetFileName(saveFile.SaveLocation);
    }
}

