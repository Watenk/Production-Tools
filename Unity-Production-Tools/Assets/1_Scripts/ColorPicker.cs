using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Watenk;

public class ColorPicker
{
    private int hue;
    private RawImage rawImage;
    private RectTransform colorPickerInput;
    private RectTransform colorPickerHandle;
    private Slider hueSlider;

    //----------------------------------------------

    public ColorPicker(){
        colorPickerInput = GameManager.Instance.ColorPickerInput;
        colorPickerHandle = GameManager.Instance.ColorInputHandle;
        hueSlider = GameManager.Instance.ColorInputHueSlider;
        rawImage = colorPickerInput.gameObject.GetComponent<RawImage>();

        #if UNITY_EDITOR
            if (rawImage == null) { Debug.LogError("colorPickerInput doesn't contain a RawImage"); }
        #endif

        EventManager.AddListener("OnLeftMouse", OnLeftMouse);
        hueSlider.onValueChanged.AddListener(OnHueChanged);

        rawImage.material = new Material(Resources.Load<Shader>("UnlitTransparent"));
    }

    //---------------------------------------------

    private void OnLeftMouse(){
        RectTransformUtility.ScreenPointToLocalPointInRectangle(GameManager.Instance.ColorPickerInput, Input.mousePosition, null, out Vector2 colorPickerMousePos);
        if (GridUtil.IsInBounds(colorPickerMousePos, Vector2.zero - colorPickerInput.sizeDelta / 2, Vector2.zero + colorPickerInput.sizeDelta / 2)){

            float saturation = MathUtil.Map(colorPickerMousePos.x, 0, colorPickerInput.sizeDelta.x, 0, 100) + 50;
            float lightness = -MathUtil.Map(colorPickerMousePos.y, 0,  colorPickerInput.sizeDelta.y, 0, 100) + 50;

            colorPickerHandle.anchoredPosition = colorPickerMousePos;
        }
    }

    private void OnHueChanged(float hue){
        rawImage.texture = GenerateTexture(new Vector2Int((int)colorPickerInput.sizeDelta.x, (int)colorPickerInput.sizeDelta.y));
    }

    private Texture2D GenerateTexture(Vector2Int size)
    {
        Texture2D texture = new Texture2D(size.x, size.y);

        for (int y = 0; y < size.y; y++){
            for (int x = 0; x < size.x; x++){
                Color color = ColorUtil.HSLToRGB((int)hueSlider.value, x, y);
                texture.SetPixel(x, y, color);
            }
        }

        texture.Apply();
        return texture;
    }
}
