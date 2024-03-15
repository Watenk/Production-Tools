using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NewCanvasTab
{
    public RectTransform RectTransform;
    public Button SelectButton;
    public TextMeshProUGUI Text;
    private Canvas canvas;

    //---------------------------------

    public NewCanvasTab(RectTransform rectTransform, Button selectButton, TextMeshProUGUI text){
        RectTransform = rectTransform;
        SelectButton = selectButton;
        Text = text;
    }

    public void SetCanvas(Canvas canvas){
        this.canvas = canvas;
    }

    public Canvas GetCanvas(){
        return canvas;
    }
}
