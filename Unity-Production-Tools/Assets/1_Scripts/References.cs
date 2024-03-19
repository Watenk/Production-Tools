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

    // UI
    public GameObject Canvas;
    public Transform TabParent;
    public RectTransform ColorPickerInputField;
    public RectTransform ColorPickerInputHandle;
    public Slider ColorPickerInputHueSlider;
    public RawImage ColorPickerHueSliderBackground;
    public Image ColorPickerCurrentColor;
}
