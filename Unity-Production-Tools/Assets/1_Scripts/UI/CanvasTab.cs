using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CanvasTab
{
    public RectTransform RectTransform;
    public Button SelectButton;
    public Button DeleteButton;
    public TextMeshProUGUI Text;
    private Canvas canvas;

    //---------------------------------

    public CanvasTab(RectTransform rectTransform, Button selectButton, Button deleteButton, TextMeshProUGUI text){
        RectTransform = rectTransform;
        SelectButton = selectButton;
        DeleteButton = deleteButton;
        Text = text;
    }

    public void SetCanvas(Canvas canvas){
        this.canvas = canvas;
    }

    public Canvas GetCanvas(){
        return canvas;
    }
}
