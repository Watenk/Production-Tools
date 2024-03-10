using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ToolSettings", menuName = "ScriptableObjects/ToolSettings")]
public class ToolSettings : ScriptableObject
{
    public static ToolSettings Instance { 
        get{
            if (instance == null){
                instance = Resources.Load<ToolSettings>("ToolSettings");

                #if UNITY_EDITOR
                    if (instance == null) {Debug.LogError("ToolSettings couldn't be loaded...");}
                #endif
            }
            return instance;
        }
    }
    private static ToolSettings instance;

    //------------------------------------------------------------------

    [Header("Rendering")]
    public Vector2Int DesiredMeshChunkSize;

    [Header("Input")]
    public float MaxCamSize;
    public float MinCamSize;
    public float ScrollSpeed;
}