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
    private NewCanvasTab plusTab;
    private NewCanvasInput newCanvasInput;
    private float tabStartPos;

    // Prefabs
    private GameObject canvasTabPrefab;

    //----------------------------------------

    public UIManager(){
        tabStartPos = ToolSettings.Instance.TabStartPos;

        plusTab = AddNewCanvasTab(ToolSettings.Instance.PlusTabPrefab);   
        plusTab.SelectButton.onClick.AddListener(OnPlusTabClicked);

        newCanvasInput = AddNewCanvasInput(ToolSettings.Instance.NewCanvasInputPrefab);
        newCanvasInput.CancelButton.onClick.AddListener(OnCancelNewCanvasClicked);
        newCanvasInput.ConfirmButton.onClick.AddListener(OnConfirmNewCanvasClicked);

        CalcTabsSizes();
    }

    //-------------------------------------------

    // UI Events
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
        
        EventManager.Invoke("OnNewCanvas", new Vector2Int(x, y));
    }

    private void OnTab(CanvasTab canvasTab){
        //canvasManager.SwitchCanvas(canvasTab.GetCanvas());
    }

    private void OnTabDelete(CanvasTab canvasTab){
        //canvasManager.RemoveCanvas(canvasTab.GetCanvas());
        canvasTabs.Remove(canvasTab);
        GameObject.Destroy(canvasTab.RectTransform.gameObject);
        CalcTabsSizes();
    }

    private void AddTab(Canvas canvas){
        // Canvas Tab
        CanvasTab canvasTab = AddCanvasTab();
        canvasTab.SetCanvas(canvas);
        canvasTab.SelectButton.onClick.AddListener(() => OnTab(canvasTab));
        canvasTab.DeleteButton.onClick.AddListener(() => OnTabDelete(canvasTab));
        
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

    private CanvasTab AddCanvasTab(){
        GameObject gameObject = GameObject.Instantiate(canvasTabPrefab, GameManager.Instance.Canvas.transform);
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        Button[] buttons = gameObject.GetComponentsInChildren<Button>(); 
        TextMeshProUGUI text = gameObject.GetComponentInChildren<TextMeshProUGUI>();

        #if UNITY_EDITOR
            if (rectTransform == null) { Debug.LogError(canvasTabPrefab.name + " Doesn't contain a rectTransform"); }
            if (buttons.Length != 2) { Debug.LogError(canvasTabPrefab.name +  " Doesn't contain 2 Buttons"); }
            if (text == null) { Debug.LogError(canvasTabPrefab.name + " Doesn't contain a TextMeshPro"); }
        #endif

        CanvasTab newCanvasTab = new CanvasTab(rectTransform, buttons[0], buttons[1], text);
        canvasTabs.Add(newCanvasTab);
        return newCanvasTab;
    }

    private NewCanvasTab AddNewCanvasTab(GameObject prefab){
        GameObject gameObject = GameObject.Instantiate(prefab, GameManager.Instance.Canvas.transform);
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        Button selectButton = gameObject.GetComponent<Button>(); 
        TextMeshProUGUI text = gameObject.GetComponentInChildren<TextMeshProUGUI>();

        #if UNITY_EDITOR
            if (rectTransform == null) { Debug.LogError(prefab.name + " Doesn't contain a rectTransform"); }
            if (selectButton == null) { Debug.LogError(prefab.name +  " selectbutton is null"); }
            if (text == null) { Debug.LogError(prefab.name + " Doesn't contain a TextMeshPro"); }
        #endif

        NewCanvasTab newCanvasTab = new NewCanvasTab(rectTransform, selectButton, text);
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

    private void CalcTabsSizes(){

        float currentPos = tabStartPos;
        for (int i = 0; i < canvasTabs.Count; i++)
        {
            CanvasTab currentTab = canvasTabs[i];

            currentTab.RectTransform.anchoredPosition = new Vector3(-1230 + currentPos + (currentTab.RectTransform.sizeDelta.x / 2), 0f, 0f);
            currentPos += currentTab.RectTransform.sizeDelta.x;
        }

        plusTab.RectTransform.anchoredPosition = new Vector3(-1230 + currentPos + (plusTab.RectTransform.sizeDelta.x / 2), 0f ,0f);
    }


}
