using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CanvasTab
{
    public Canvas Canvas { get; private set; }
    public RectTransform RectTransform;
    public Button SelectButton;
    public Button DeleteButton;
    public TMP_InputField InputField;

    //---------------------------------

    public CanvasTab(Canvas canvas, RectTransform rectTransform, Button selectButton, Button deleteButton, TMP_InputField text){
        Canvas = canvas;
        RectTransform = rectTransform;
        SelectButton = selectButton;
        DeleteButton = deleteButton;
        InputField = text;
    }
}
