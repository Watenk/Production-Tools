using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Watenk;

// What is this class.... needs a rewrite

public class TabUI
{
    // Canvas Tabs
    private List<CanvasTab> canvasTabs = new List<CanvasTab>();
    private NewCanvasTab plusTab;
    private NewCanvasInput newCanvasInput;
    private float tabStartPos;

    // Prefabs
    private GameObject tabPrefab;

    //----------------------------------------

    public TabUI(){
        tabStartPos = ToolSettings.Instance.TabStartPos;
        tabPrefab = ToolSettings.Instance.CanvasTabPrefab;

        plusTab = CreateNewCanvasTab(ToolSettings.Instance.PlusTabPrefab);   
        newCanvasInput = CreateNewCanvasInput(ToolSettings.Instance.NewCanvasInputPrefab);

        EventManager.AddListener<Canvas>(Events.OnNewCanvas, AddTab);
        EventManager.AddListener<Canvas>(Events.OnLoadCanvas, AddTab);
        EventManager.AddListener<Canvas>(Events.OnSwitchTab, SwitchTab);

        CalcTabsSizes();
    }

    //-------------------------------------------
    // Events

    private void OnPlusTabClicked(){
        newCanvasInput.GameObject.SetActive(true);
    }

    private void OnCancelNewCanvasClicked(){
        newCanvasInput.GameObject.SetActive(false);
    }

    private void OnConfirmNewCanvasClicked(){

        if (newCanvasInput.WidthInput.text == "") return;
        if (newCanvasInput.HeightInput.text == "") return;

        int x = int.Parse(newCanvasInput.WidthInput.text);
        int y = int.Parse(newCanvasInput.HeightInput.text);
        if (x == 0 || y == 0) return;
        
        newCanvasInput.GameObject.SetActive(false);
        EventManager.Invoke(Events.OnNewCanvasClicked, new Vector2Int(x, y));
    }

    // Tab Events

    private void OnTabClicked(CanvasTab canvasTab){
        EventManager.Invoke(Events.OnSwitchCanvasClicked, canvasTab.Canvas);
    }

    private void OnTabDeleteClicked(CanvasTab canvasTab){
        canvasTabs.Remove(canvasTab);
        GameObject.Destroy(canvasTab.RectTransform.gameObject);
        CalcTabsSizes();
        EventManager.Invoke(Events.OnRemoveCanvasClicked, canvasTab.Canvas);
    }

    //---------------------------------------------------

    private void AddTab(Canvas canvas){
        CanvasTab canvasTab = CreateCanvasTab(canvas);
        canvasTab.SelectButton.onClick.AddListener(() => OnTabClicked(canvasTab));
        canvasTab.DeleteButton.onClick.AddListener(() => OnTabDeleteClicked(canvasTab));
        
        newCanvasInput.GameObject.SetActive(false);

        CalcTabsSizes();
    }

    private void SwitchTab(Canvas canvas){

        foreach (var current in canvasTabs){
            current.SelectButton.image.color = Color.white;
        }

        CanvasTab canvasTab = canvasTabs.Find(listCanvas => listCanvas.Canvas == canvas);
        canvasTab.SelectButton.image.color = new Color(140f / 255f, 154f / 255f, 176f / 255f);
    }

    private void CalcTabsSizes(){

        float currentPos = tabStartPos;
        for (int i = 0; i < canvasTabs.Count; i++)
        {
            CanvasTab currentTab = canvasTabs[i];

            currentTab.RectTransform.anchoredPosition = new Vector3(currentPos + (currentTab.RectTransform.sizeDelta.x / 2), 0f, 0f);
            currentPos += currentTab.RectTransform.sizeDelta.x;
        }

        plusTab.RectTransform.anchoredPosition = new Vector3(currentPos + (plusTab.RectTransform.sizeDelta.x / 2), 0f ,0f);
    }

    //-----------------------------------------------------------
    // Factory's

    private CanvasTab CreateCanvasTab(Canvas canvas){
        GameObject gameObject = GameObject.Instantiate(tabPrefab, References.Instance.TabParent.transform);
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        Button[] buttons = gameObject.GetComponentsInChildren<Button>(); 
        TMP_InputField inputField = gameObject.GetComponentInChildren<TMP_InputField>();

        #if UNITY_EDITOR
            if (rectTransform == null) { Debug.LogError(tabPrefab.name + " Doesn't contain a rectTransform"); }
            if (buttons.Length != 2) { Debug.LogError(tabPrefab.name +  " Doesn't contain 2 Buttons"); }
            if (inputField == null) { Debug.LogError(tabPrefab.name + " Doesn't contain a TMP_InputField"); }
        #endif

        inputField.text = canvas.Name;
        CanvasTab newCanvasTab = new CanvasTab(canvas, rectTransform, buttons[0], buttons[1], inputField);
        inputField.onValueChanged.AddListener((newName) => newCanvasTab.Canvas.SetName(newName));
        canvasTabs.Add(newCanvasTab);
        return newCanvasTab;
    }

    private NewCanvasTab CreateNewCanvasTab(GameObject prefab){
        GameObject gameObject = GameObject.Instantiate(prefab, References.Instance.TabParent.transform);
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        Button selectButton = gameObject.GetComponent<Button>(); 
        TextMeshProUGUI text = gameObject.GetComponentInChildren<TextMeshProUGUI>();

        #if UNITY_EDITOR
            if (rectTransform == null) { Debug.LogError(prefab.name + " Doesn't contain a rectTransform"); }
            if (selectButton == null) { Debug.LogError(prefab.name +  " selectbutton is null"); }
            if (text == null) { Debug.LogError(prefab.name + " Doesn't contain a TextMeshPro"); }
        #endif

        NewCanvasTab newCanvasTab = new NewCanvasTab(rectTransform, selectButton, text);
        newCanvasTab.SelectButton.onClick.AddListener(OnPlusTabClicked);
        return newCanvasTab;
    }

    private NewCanvasInput CreateNewCanvasInput(GameObject prefab){
        GameObject gameObject = GameObject.Instantiate(prefab, References.Instance.Canvas.transform);
        Button[] buttons = gameObject.GetComponentsInChildren<Button>(); 
        TMP_InputField[] inputs = gameObject.GetComponentsInChildren<TMP_InputField>();

        #if UNITY_EDITOR
            if (buttons.Length != 2) { Debug.LogError("NewCanvasInputPrefab can't find 2 buttons"); }
            if (inputs.Length != 2) { Debug.LogError("NewCanvasInputPrefab can't find 2 inputFields"); } 
        #endif

        Button cancelButton = buttons[0];
        Button confirmButton = buttons[1];

        TMP_InputField widthInput = inputs[0];
        TMP_InputField heightInput = inputs[1];

        gameObject.SetActive(false);

        NewCanvasInput newCanvasInput = new NewCanvasInput(gameObject, cancelButton, confirmButton, widthInput, heightInput);

        newCanvasInput.CancelButton.onClick.AddListener(OnCancelNewCanvasClicked);
        newCanvasInput.ConfirmButton.onClick.AddListener(OnConfirmNewCanvasClicked);
        return newCanvasInput;
    }
}
