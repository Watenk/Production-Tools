using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CanvasTab
{
    public RectTransform RectTransform;
    public Button Button;
    public TextMeshProUGUI Text;
    private Canvas canvas;

    //---------------------------------

    public CanvasTab(RectTransform rectTransform, Button button, TextMeshProUGUI text){
        RectTransform = rectTransform;
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
