using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class References : MonoBehaviour
{
    public static References Instance { 
        get{
            if (instance == null){
                Debug.LogError("Couldn't find References Instance");
            }
            return instance;
        }
    }
    private static References instance;

    //------------------------------------------

    public void Awake(){
        instance = this;
    }

    //-------------------------------------------

    public Texture2D MouseTexture;

    [Header("Tools")]
    public Button SaveButton;
    public Button ExportButton;
    public Button LoadButton;

    [Header("Parents")]
    public GameObject Canvas;
    public Transform TabParent;
    public RectTransform ToolBarBackground;
    public RectTransform TabsBarBackground;
    public RectTransform RightBarBackground;

    [Header("Layers")]
    public RectTransform LayerScrollContent;
    public Button AddLayerButton;
    public Button RemoveLayerButton;
    public Button PromoteLayerButton;
    public Button DemoteLayerButton;
    public Slider LayerAlphaSlider;

    [Header("Color Picker")]
    public RectTransform ColorPickerInputField;
    public RectTransform ColorPickerInputHandle;
    public Slider ColorPickerInputHueSlider;
    public Slider ColorPickerAlphaSlider;
    public RawImage ColorPickerHueSliderBackground;
    public Image ColorPickerCurrentColor;
}
