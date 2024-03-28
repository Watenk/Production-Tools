using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;
using SFB;

public static class SaveManager
{
    public static void Save(Canvas canvas){
        
        if (canvas == null) { Debug.LogWarning("Save Failed... Canvas is null"); return; }

        // Generate SaveFile
        CanvasSaveFile saveFile = new CanvasSaveFile();
        canvas.SaveTo(ref saveFile);

        if (saveFile.Size == null || saveFile.Size == Vector2Int.zero) { Debug.LogWarning("Save Failed... Size is null"); return; }

        // Get Save Location (if neccecary)
        if (saveFile.SaveLocation == null){
            saveFile.SaveLocation = StandaloneFileBrowser.SaveFilePanel(saveFile.Name, "", "", "");
            if (saveFile.SaveLocation == "" || saveFile.SaveLocation == null) { Debug.LogWarning("Save Failed... SaveLocation is null"); return; }
        }
        if (!Directory.Exists(saveFile.SaveLocation)){
            GenerateSaveDirFor(saveFile);
        }

        // Write Data Json
        StreamWriter writer = new StreamWriter(saveFile.SaveLocation + "/data.txt", false);
        writer.WriteLine(JsonUtility.ToJson(saveFile, true));
        writer.Close();
        writer.Dispose();

        // Write Layers PNG
        if (saveFile.Layers == null)  { Debug.LogWarning("Save Failed... Layers is null"); return; }
        foreach (var current in saveFile.Layers){
             byte[] bytes = current.Value.Texture.EncodeToPNG();
             File.WriteAllBytes(saveFile.SaveLocation + "/layers/" + current.Key.ToString() + ".png", bytes);
        }

        Debug.Log("Saved " + saveFile.Name + " to " + saveFile.SaveLocation);
    }

    public static void Export(Canvas canvas){

        if (canvas == null) { Debug.LogWarning("Export Failed... Canvas is null"); return; }

        // Generate SaveFile
        CanvasSaveFile saveFile = new CanvasSaveFile();
        canvas.SaveTo(ref saveFile);

        if (saveFile.Size == null || saveFile.Size == Vector2Int.zero) { Debug.LogWarning("Export Failed... Size is null"); return; }

        // Get Export Location
        string exportLocation = StandaloneFileBrowser.SaveFilePanel(saveFile.Name, "", "", "");
        if (exportLocation == "" || exportLocation == null) { Debug.LogWarning("Export Failed... ExportLocation is null"); return; }

        // Write PNG
        Texture2D combinedTexture = new Texture2D(canvas.Size.x, canvas.Size.y);
        foreach (var kvp in saveFile.Layers)
        {
            Color[] layerPixels = kvp.Value.Texture.GetPixels();
            BlendLayer(ref combinedTexture, layerPixels, 0, 0);
        }
        combinedTexture.Apply();

        byte[] bytes = combinedTexture.EncodeToPNG();
        File.WriteAllBytes(exportLocation + ".png", bytes);

        Debug.Log("Exported " + saveFile.Name + " to " + exportLocation + ".png");
    }

    public static Canvas Load(){

        // Get SaveLocation
        string[] savePaths = StandaloneFileBrowser.OpenFolderPanel("", "", false);
        if (savePaths.Length == 0) { Debug.LogWarning("Load Failed... Couldn't find specified dir"); return null; }
        string savePath = savePaths[0];

        // Read Data Json
        if (!File.Exists(savePath + "/data.txt")) { Debug.LogWarning("Load Failed... Couldn't find " + savePath + "/data.txt"); return null; }
        string json = File.ReadAllText(savePath + "/data.txt");
        CanvasSaveFile saveFile = JsonUtility.FromJson<CanvasSaveFile>(json);
        saveFile.Layers = new Dictionary<int, Layer>();

        // Read Layers PNG's
        if (!Directory.Exists(savePath + "/layers")) { Debug.LogWarning("Load Failed... Couldn't find " + savePath + "/layers"); return null; }
        for (int i = 0; i < saveFile.LayerCount; i++){
            if (!File.Exists(savePath + "/data.txt")) { Debug.LogWarning("Load Failed... Couldn't find " + savePath + "/layers/" + i + ".png"); return null; }
            Texture2D texture = new Texture2D(saveFile.Size.x, saveFile.Size.y);
            byte[] layerData = File.ReadAllBytes(savePath + "/layers/" + i.ToString() + ".png");
            texture.LoadImage(layerData);
            saveFile.Layers.Add(i, new Layer(texture, i));
        }

        Debug.Log("Loaded " + saveFile.Name + " from " + savePath);
        return new Canvas(saveFile);
    }

    //----------------------------------------------------
    
    private static string GetConfigPath(){
        if (Application.isEditor){
            return Application.dataPath;
        }
        else{
            return Application.persistentDataPath;
        }
    }

    private static void GenerateSaveDirFor(CanvasSaveFile saveFile){
        Directory.CreateDirectory(saveFile.SaveLocation);
        Directory.CreateDirectory(saveFile.SaveLocation + "/layers");
        saveFile.Name = Path.GetFileName(saveFile.SaveLocation);
    }

    private static void BlendLayer(ref Texture2D combinedTexture, Color[] layerPixels, int xOffset, int yOffset)
    {
        Color[] combinedPixels = combinedTexture.GetPixels(xOffset, yOffset, combinedTexture.width, combinedTexture.height);
        
        for (int i = 0; i < combinedPixels.Length; i++){
            combinedPixels[i] = Color.Lerp(combinedPixels[i], layerPixels[i], layerPixels[i].a);
        }

        combinedTexture.SetPixels(xOffset, yOffset, combinedTexture.width, combinedTexture.height, combinedPixels);
    }
}

