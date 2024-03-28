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
        References.Instance.LayerAlphaSlider.onValueChanged.AddListener((newValue) => EventManager.Invoke(Events.OnLayerAlphaChanged, newValue));

        EventManager.AddListener<Layer>(Events.OnNewLayer, AddLayerUI);
        EventManager.AddListener<Layer>(Events.OnRemoveLayer, RemoveLayerUI);
        EventManager.AddListener<Layer>(Events.OnLayerSwitchBackgroundToWhite, OnLayerSwitchBackgroundToWhite);
        EventManager.AddListener<Layer>(Events.OnLayerSwitchBackgroundToGray, OnLayerSwitchBackgroundToGray);
        EventManager.AddListener<Canvas>(Events.OnSwitchTab, OnSwitchTab);
        EventManager.AddListener(Events.OnCanvasNull, () => ClearLayers());
    }

    //---------------------------------------------

    private void OnSwitchTab(Canvas canvas){
        ClearLayers();
        AddLayers(canvas);
        OnLayerSwitchBackgroundToGray(canvas.CurrentLayer);
    }

    private void AddLayerUI(Layer layer){
        GameObject gameObject = GameObject.Instantiate(layerUIPrefab, layerScrollContent);
        gameObject.transform.SetSiblingIndex(0);
        Button[] buttons = gameObject.GetComponentsInChildren<Button>(); 
        TMP_InputField inputField = gameObject.GetComponentInChildren<TMP_InputField>();
        Image background = gameObject.GetComponentInChildren<Image>();

        #if UNITY_EDITOR
            if (buttons.Length != 3) { Debug.LogError(layerUIPrefab.name +  " Doesn't contain 3 Buttons"); }
            if (inputField == null) { Debug.LogError(layerUIPrefab.name + " Doesn't contain a InputField"); }
            if (background == null) { Debug.LogError(layerUIPrefab.name + " Doesn't contain a background Image"); }
        #endif

        LayerUITab layerUITab = new LayerUITab(gameObject, layer, inputField, background);
        layerUITabs.Add(layerUITab);

        buttons[0].onClick.AddListener(() => EventManager.Invoke(Events.OnLayerVisiblityClicked, layer));
        buttons[1].onClick.AddListener(() => EventManager.Invoke(Events.OnLayerLockClicked, layer));
        buttons[2].onClick.AddListener(() => EventManager.Invoke(Events.OnLayerSelectClicked, layer));
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

    // What amazing non-hardcoded methods :)
    private void OnLayerSwitchBackgroundToWhite(Layer layer){
        LayerUITab layerUITab = layerUITabs.Find((currentUITab) => currentUITab.Layer == layer);
        layerUITab.Background.color = Color.white;
    }

    private void OnLayerSwitchBackgroundToGray(Layer layer){
        LayerUITab layerUITab = layerUITabs.Find((currentUITab) => currentUITab.Layer == layer);
        layerUITab.Background.color = Color.gray;
    }
}
