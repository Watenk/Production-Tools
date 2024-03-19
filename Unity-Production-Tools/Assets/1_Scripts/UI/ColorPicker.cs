using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Watenk;

public class ColorPicker
{
    private Color currentColor;
    private RectTransform colorPickerInputField;
    private RectTransform colorPickerHandle;
    private Slider hueSlider;
    private RawImage inputFieldImage;
    private RawImage hueSliderBackground;
    private Image currentColorImage;
    private bool isMouseDown;

    //----------------------------------------------

    public ColorPicker(){
        colorPickerInputField = References.Instance.ColorPickerInputField;
        colorPickerHandle = References.Instance.ColorPickerInputHandle;
        hueSlider = References.Instance.ColorPickerInputHueSlider;
        currentColorImage = References.Instance.ColorPickerCurrentColor;
        hueSliderBackground = References.Instance.ColorPickerHueSliderBackground;
        inputFieldImage = colorPickerInputField.gameObject.GetComponent<RawImage>();

        #if UNITY_EDITOR
            if (inputFieldImage == null) { Debug.LogError("colorPickerInputField doesn't contain a RawImage"); }
        #endif

        EventManager.AddListener("OnLeftMouseUp", OnLeftMouseUp);
        EventManager.AddListener("OnLeftMouse", OnLeftMouse);
        EventManager.AddListener("OnLeftMouseDown", OnLeftMouseDown);
        hueSlider.onValueChanged.AddListener(OnHueChanged);
        
        inputFieldImage.material = new Material(Resources.Load<Shader>("UnlitTransparent"));
        hueSliderBackground.texture = GenerateHueTexture(new Vector2Int(100, 100));

        OnHueChanged(0);
    }

    //---------------------------------------------

    private void OnLeftMouseDown(){
        RectTransformUtility.ScreenPointToLocalPointInRectangle(colorPickerInputField, Input.mousePosition, null, out Vector2 colorPickerMousePos);
        if (!GridUtil.IsInBounds(colorPickerMousePos, Vector2.zero - colorPickerInputField.sizeDelta / 2, Vector2.zero + colorPickerInputField.sizeDelta / 2)) return;

        isMouseDown = true;
    }

    private void OnLeftMouse(){

        if (!isMouseDown) return;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(colorPickerInputField, Input.mousePosition, null, out Vector2 colorPickerMousePos);
        
        colorPickerHandle.anchoredPosition = new Vector2(
            Mathf.Clamp(colorPickerMousePos.x, Vector2.zero.x - colorPickerInputField.sizeDelta.x / 2, Vector2.zero.x + colorPickerInputField.sizeDelta.x / 2),
            Mathf.Clamp(colorPickerMousePos.y, Vector2.zero.y - colorPickerInputField.sizeDelta.y / 2, Vector2.zero.y + colorPickerInputField.sizeDelta.y / 2));

        CalcColor();
    }

    private void OnLeftMouseUp(){
        isMouseDown = false;
    }

    private void OnHueChanged(float hue){
        inputFieldImage.texture = GenerateFieldTexture(new Vector2Int((int)colorPickerInputField.sizeDelta.x, (int)colorPickerInputField.sizeDelta.y));
        CalcColor();
    }

    private void CalcColor(){
        float saturation = MathUtil.Map(colorPickerHandle.anchoredPosition.x, 0, colorPickerInputField.sizeDelta.x, 0, 100) + 50;
        float lightness = MathUtil.Map(colorPickerHandle.anchoredPosition.y, 0,  colorPickerInputField.sizeDelta.y, 0, 100) + 50;
        currentColor = ColorUtil.HSLToRGB((int)hueSlider.value, (int)saturation, (int)lightness);
        currentColorImage.color = currentColor;
    }

    private Texture2D GenerateFieldTexture(Vector2Int size){
        Texture2D texture = new Texture2D(size.x, size.y);

        for (int y = 0; y < size.y; y++){
            for (int x = 0; x < size.x; x++){
                Color color = ColorUtil.HSLToRGB((int)hueSlider.value, (int)MathUtil.Map(x, 0, size.x, 0, 100), (int)MathUtil.Map(y, 0, size.y, 0, 100));
                texture.SetPixel(x, y, color);
            }
        }

        texture.Apply();
        return texture;
    }
    
    private Texture2D GenerateHueTexture(Vector2Int size){
        Texture2D texture = new Texture2D(size.x, size.y);

        for (int y = 0; y < size.y; y++){
            for (int x = 0; x < size.x; x++){
                Color color = ColorUtil.HSLToRGB((int)MathUtil.Map(x, 0, size.x, 0, 360), 50, 50);
                texture.SetPixel(x, y, color);
            }
        }

        texture.Apply();
        return texture;
    }
}
