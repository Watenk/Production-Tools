using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public struct NewCanvasInput
{
    public GameObject GameObject;
    public Button CancelButton;
    public Button ConfirmButton;
    public TMP_InputField WidthInput;
    public TMP_InputField HeightInput;

    //------------------------------------------------

    public NewCanvasInput(GameObject gameObject, Button cancelButton, Button confirmButton, TMP_InputField widthInput, TMP_InputField heightInput){
        GameObject = gameObject;
        CancelButton = cancelButton;
        ConfirmButton = confirmButton;
        WidthInput = widthInput;
        HeightInput = heightInput;
    }
}
