using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager
{
    // Canvas Tabs
    private List<CanvasTab> canvasTabs = new List<CanvasTab>();
    private CanvasTab plusTab;
    private NewCanvasInput newCanvasInput;
    private float tabStartPos;

    // Prefabs
    private GameObject canvasTabPrefab;

    // Dependencies
    private CanvasManager canvasManager;

    //----------------------------------------

    public UIManager(){
        canvasManager = GameManager.GetService<CanvasManager>();
        canvasTabPrefab = ToolSettings.Instance.CanvasTabPrefab;
        tabStartPos = ToolSettings.Instance.TabStartPos;

        plusTab = AddCanvasTab(ToolSettings.Instance.PlusTabPrefab);   
        plusTab.Button.onClick.AddListener(OnPlusTab);

        newCanvasInput = AddNewCanvasInput(ToolSettings.Instance.NewCanvasInputPrefab);
        newCanvasInput.CancelButton.onClick.AddListener(OnCancelNewCanvas);
        newCanvasInput.ConfirmButton.onClick.AddListener(OnCreateNewCanvas);

        CalcTabsSizes();
    }

    //---------------------------------------

    private CanvasTab AddCanvasTab(GameObject prefab){
        GameObject gameObject = GameObject.Instantiate(prefab, GameManager.Instance.Canvas.transform);
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        Button button = gameObject.GetComponent<Button>(); 
        TextMeshProUGUI text = gameObject.GetComponentInChildren<TextMeshProUGUI>();

        #if UNITY_EDITOR
            if (rectTransform == null) { Debug.LogError(prefab.name + " Doesn't contain a rectTransform"); }
            if (button == null) { Debug.LogError(prefab.name + " Doesn't contain a button"); }
            if (text == null) { Debug.LogError(prefab.name + " Doesn't contain a TextMeshPro"); }
        #endif

        CanvasTab newCanvasTab = new CanvasTab(rectTransform, button, text);
        canvasTabs.Add(newCanvasTab);
        return newCanvasTab;
    }

    private NewCanvasInput AddNewCanvasInput(GameObject prefab){
        GameObject gameObject = GameObject.Instantiate(prefab, GameManager.Instance.Canvas.transform);
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

        return new NewCanvasInput(gameObject, cancelButton, confirmButton, widthInput, heightInput);
    }

    // TODO: Fix performance
    private void CalcTabsSizes(){

        float currentPos = Screen.width * tabStartPos;
        for (int i = 0; i < canvasTabs.Count; i++)
        {
            CanvasTab currentTab = canvasTabs[i];

            currentTab.RectTransform.anchoredPosition = new Vector3(-1260 + currentPos + (currentTab.RectTransform.sizeDelta.x / 2), 0f, 0f);
            currentPos += currentTab.RectTransform.sizeDelta.x;
        }

        plusTab.RectTransform.anchoredPosition = new Vector3(-1260 + currentPos + (plusTab.RectTransform.sizeDelta.x / 2), 0f ,0f);
    }

    // UI Events
    private void OnPlusTab(){
        newCanvasInput.GameObject.SetActive(true);
    }

    private void OnCancelNewCanvas(){
        newCanvasInput.GameObject.SetActive(false);
    }

    private void OnCreateNewCanvas(){

        if (newCanvasInput.WidthInput.text == "") return;
        if (newCanvasInput.HeightInput.text == "") return;

        int x = int.Parse(newCanvasInput.WidthInput.text);
        int y = int.Parse(newCanvasInput.HeightInput.text);
        if (x == 0 || y == 0) return;
        
        Canvas newCanvas = canvasManager.NewCanvas(new Vector2Int(x, y));

        // Canvas Tab
        CanvasTab canvasTab = AddCanvasTab(canvasTabPrefab);
        canvasTab.SetCanvas(newCanvas);
        canvasTab.Button.onClick.AddListener(() => OnTab(canvasTab));
        
        newCanvasInput.GameObject.SetActive(false);

        CalcTabsSizes();
    }

    private void OnTab(CanvasTab canvasTab){
        canvasManager.SwitchCanvas(canvasTab.GetCanvas());
    }
}
