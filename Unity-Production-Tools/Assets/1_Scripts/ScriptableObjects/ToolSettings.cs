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

    [Header("Input")]
    public float MaxCamSize;
    public float MinCamSize;
    public float ScrollSpeed;

    [Header("UI")]
    public float TabStartPos;

    [Header("Prefabs")]
    public GameObject CanvasTabPrefab;
    public GameObject PlusTabPrefab;
    public GameObject NewCanvasInputPrefab;

}