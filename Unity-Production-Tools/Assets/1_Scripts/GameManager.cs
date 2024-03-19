using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject Canvas { get { return canvas; } }
    [SerializeField] private GameObject canvas;
    public Transform TabParent { get { return tabParent; } }
    [SerializeField] private Transform tabParent;
    public RectTransform ColorPickerInput { get { return colorPickerInput; } }
    [SerializeField] private RectTransform colorPickerInput;
    public RectTransform ColorInputHandle { get { return colorInputHandle; } }
    [SerializeField] private RectTransform colorInputHandle;
    public Slider ColorInputHueSlider { get { return colorInputHueSlider; } }
    [SerializeField] private Slider colorInputHueSlider;

    private static Dictionary<System.Type, object> services = new Dictionary<System.Type, object>();
    private static List<IUpdateable> updateables = new List<IUpdateable>();
    private static List<IFixedUpdateable> fixedUpdateables = new List<IFixedUpdateable>();

    //----------------------------------------

    public void Awake(){
        Instance = this;

        AddService(new EventManagerNoParamater());
        AddService(new InputHandler());
        AddService(new UIManager());
        AddService(new CanvasManager());
        AddService(new CameraController());
    }

    public void Update(){
        foreach (IUpdateable updateable in updateables) { updateable.OnUpdate(); }
    }

    public void FixedUpdate(){
        foreach (IFixedUpdateable fixedUpdateable in fixedUpdateables) { fixedUpdateable.OnFixedUpdate(); }
    }

    public static T GetService<T>(){
        services.TryGetValue(typeof(T), out object service);

        #if UNITY_EDITOR
            if (service == null) { Debug.LogError(typeof(T).Name + " Sevice not found"); }
        #endif

        return (T)service;
    }

    //---------------------------------------

    private void AddService<T>(T service){
        services.Add(typeof(T), service);

        if (service is IUpdateable) { updateables.Add((IUpdateable)service); }
        if (service is IFixedUpdateable) { fixedUpdateables.Add((IFixedUpdateable)service); }
    }
}
