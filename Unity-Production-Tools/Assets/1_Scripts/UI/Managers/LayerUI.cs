using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LayerUI 
{
    private List<LayerUITab> layerUITabs = new List<LayerUITab>();
    private RectTransform layerScrollContent;
    private GameObject layerUIPrefab;

    //------------------------------------------

    public LayerUI(){
        layerScrollContent = References.Instance.LayerScrollContent;
        layerUIPrefab = ToolSettings.Instance.LayerUIPrefab;

        References.Instance.AddLayerButton.onClick.AddListener(() => EventManager.Invoke(Events.OnAddLayerClicked));
        References.Instance.RemoveLayerButton.onClick.AddListener(() => EventManager.Invoke(Events.OnRemoveLayerClicked));
        References.Instance.PromoteLayerButton.onClick.AddListener(() => EventManager.Invoke(Events.OnPromoteLayerClicked));
        References.Instance.DemoteLayerButton.onClick.AddListener(() => EventManager.Invoke(Events.OnDemoteLayerClicked));

        EventManager.AddListener<Layer>(Events.OnNewLayer, AddLayerUI);
        EventManager.AddListener<Layer>(Events.OnRemoveLayer, RemoveLayerUI);
        EventManager.AddListener<Layer, Color>(Events.OnLayerChangeColor, OnLayerChangeColor);
        EventManager.AddListener<Canvas>(Events.OnSwitchTab, OnSwitchTab);
        EventManager.AddListener(Events.OnCanvasNull, () => ClearLayers());
    }

    //---------------------------------------------

    private void OnSwitchTab(Canvas canvas){
        ClearLayers();
        AddLayers(canvas);
        OnLayerChangeColor(canvas.CurrentLayer, new Color(140f / 255f, 154f / 255f, 176f / 255f));
    }

    private void AddLayerUI(Layer layer){
        GameObject gameObject = GameObject.Instantiate(layerUIPrefab, layerScrollContent);
        gameObject.transform.SetSiblingIndex(0);
        Button[] buttons = gameObject.GetComponentsInChildren<Button>(); 
        TMP_InputField inputField = gameObject.GetComponentInChildren<TMP_InputField>();
        Image background = gameObject.GetComponentInChildren<Image>();

        #if UNITY_EDITOR
            if (buttons.Length != 2) { Debug.LogError(layerUIPrefab.name +  " Doesn't contain 2 Buttons"); }
            if (inputField == null) { Debug.LogError(layerUIPrefab.name + " Doesn't contain a InputField"); }
            if (background == null) { Debug.LogError(layerUIPrefab.name + " Doesn't contain a background Image"); }
        #endif

        inputField.text = layer.Name;
        LayerUITab layerUITab = new LayerUITab(gameObject, layer, inputField, background);
        layerUITabs.Add(layerUITab);

        inputField.onValueChanged.AddListener((newName) => layerUITab.Layer.SetName(newName));
        buttons[0].onClick.AddListener(() => EventManager.Invoke(Events.OnLayerVisiblityClicked, layer));
        buttons[1].onClick.AddListener(() => EventManager.Invoke(Events.OnLayerSelectClicked, layer));
    }

    private void RemoveLayerUI(Layer layer){
        LayerUITab layerUITab = layerUITabs.Find((currentUITab) => currentUITab.Layer == layer);
        layerUITabs.Remove(layerUITab);
        GameObject.Destroy(layerUITab.GameObject);
    }

    private void ClearLayers(){
        foreach (LayerUITab current in layerUITabs){
            GameObject.Destroy(current.GameObject);
        }
        layerUITabs.Clear();
    }

    private void AddLayers(Canvas canvas){
        Dictionary<int, Layer> layers = canvas.GetLayers();
        foreach (var kvp in layers){
            AddLayerUI(kvp.Value);
        }
    }

    private void OnLayerChangeColor(Layer layer, Color color){
        LayerUITab layerUITab = layerUITabs.Find((currentUITab) => currentUITab.Layer == layer);
        layerUITab.Background.color = color;
    }
}
