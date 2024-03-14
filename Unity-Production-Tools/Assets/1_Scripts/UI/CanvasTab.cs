using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CanvasTab
{
    public GameObject GameObject;
    public Button Button;
    public TextMeshProUGUI Text;
    private Canvas canvas;

    //---------------------------------

    public CanvasTab(GameObject gameObject, Button button, TextMeshProUGUI text){
        GameObject = gameObject;
        Button = button;
        Text = text;
    }

    public void SetCanvas(Canvas canvas){
        this.canvas = canvas;
    }

    public Canvas GetCanvas(){
        return canvas;
    }
}
