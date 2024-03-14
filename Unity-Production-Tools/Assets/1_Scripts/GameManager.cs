using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private static Dictionary<System.Type, object> services = new Dictionary<System.Type, object>();
    private static List<IUpdateable> updateables = new List<IUpdateable>();
    private static List<IFixedUpdateable> fixedUpdateables = new List<IFixedUpdateable>();

    //----------------------------------------

    public void Awake(){
        Instance = this;

        AddService(new InputHandler());
        AddService(new CanvasManager());
        AddService(new CameraController());
        AddService(new TabManager());
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
