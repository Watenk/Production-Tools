using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Watenk;

public class UIManager
{
    // Canvas Tabs
    private List<CanvasTab> canvasTabs = new List<CanvasTab>();
    private NewCanvasTab plusTab;
    private NewCanvasInput newCanvasInput;
    private float tabStartPos;

    private ColorPicker colorPicker;

    // Prefabs
    private GameObject tabPrefab;

    //----------------------------------------

    public UIManager(){
        tabStartPos = ToolSettings.Instance.TabStartPos;
        tabPrefab = ToolSettings.Instance.CanvasTabPrefab;

        colorPicker = new ColorPicker();
        plusTab = CreateNewCanvasTab(ToolSettings.Instance.PlusTabPrefab);   
        newCanvasInput = CreateNewCanvasInput(ToolSettings.Instance.NewCanvasInputPrefab);

        EventManager.AddListener<Canvas>("OnNewCanvas", AddTab);
        EventManager.AddListener<Canvas>("OnLoadCanvas", AddTab);
        EventManager.AddListener<Canvas>("OnSwitchTab", SwitchTab);

        CalcTabsSizes();
    }

    //-------------------------------------------

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
        EventManager.Invoke("OnNewCanvasClicked", new Vector2Int(x, y));
    }

    private void OnTabClicked(CanvasTab canvasTab){
        EventManager.Invoke("OnSwitchCanvasClicked", canvasTab.GetCanvas());
    }

    private void OnTabDeleteClicked(CanvasTab canvasTab){
        canvasTabs.Remove(canvasTab);
        GameObject.Destroy(canvasTab.RectTransform.gameObject);
        CalcTabsSizes();
        EventManager.Invoke("OnRemoveCanvasClicked", canvasTab.GetCanvas());
    }

    //---------------------------------------------------

    private void AddTab(Canvas canvas){
        CanvasTab canvasTab = CreateCanvasTab();
        canvasTab.SetCanvas(canvas);
        canvasTab.SelectButton.onClick.AddListener(() => OnTabClicked(canvasTab));
        canvasTab.DeleteButton.onClick.AddListener(() => OnTabDeleteClicked(canvasTab));
        
        newCanvasInput.GameObject.SetActive(false);

        CalcTabsSizes();
    }

    private void SwitchTab(Canvas canvas){

        foreach (var current in canvasTabs){
            current.SelectButton.image.color = Color.white;
        }

        CanvasTab canvasTab = canvasTabs.Find(listCanvas => listCanvas.GetCanvas() == canvas);
        canvasTab.SelectButton.image.color = Color.gray;
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

    private CanvasTab CreateCanvasTab(){
        GameObject gameObject = GameObject.Instantiate(tabPrefab, References.Instance.TabParent.transform);
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        Button[] buttons = gameObject.GetComponentsInChildren<Button>(); 
        TextMeshProUGUI text = gameObject.GetComponentInChildren<TextMeshProUGUI>();

        #if UNITY_EDITOR
            if (rectTransform == null) { Debug.LogError(tabPrefab.name + " Doesn't contain a rectTransform"); }
            if (buttons.Length != 2) { Debug.LogError(tabPrefab.name +  " Doesn't contain 2 Buttons"); }
            if (text == null) { Debug.LogError(tabPrefab.name + " Doesn't contain a TextMeshPro"); }
        #endif

        CanvasTab newCanvasTab = new CanvasTab(rectTransform, buttons[0], buttons[1], text);
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
