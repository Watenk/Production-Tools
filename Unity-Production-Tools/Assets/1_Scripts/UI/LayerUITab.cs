using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LayerUITab
{
    public GameObject GameObject { get; private set; }
    public Layer Layer { get; private set; }
    public TMP_InputField InputField { get; private set; }
    public Image Background { get; private set; }

    //------------------------------------

    public LayerUITab(GameObject gameObject, Layer layer, TMP_InputField inputField, Image background){
        GameObject = gameObject;
        Layer = layer;
        InputField = inputField;
        Background = background;
    }
}
