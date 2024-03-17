using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: Optimization

public static class EventManager
{
    private static Dictionary<Type, object> eventManagers = new Dictionary<Type, object>();

    //--------------------------------------------------

    public static void AddListener(string eventName, Action listener){
        Get().AddListener(eventName, listener);
    }

    public static void AddListener<T>(string eventName, Action<T> listener){
        Get<T>().AddListener(eventName, listener);
    }

    public static void RemoveListener(string eventName, Action listener){
        Get().RemoveListener(eventName, listener);
    }

    public static void RemoveListener<T>(string eventName, Action<T> listener){
        Get<T>().RemoveListener(eventName, listener);
    }

    public static void Invoke(string eventName){
        Get().Invoke(eventName);
    }

    public static void Invoke<T>(string eventName, T eventParam){
        Get<T>().Invoke(eventName, eventParam);
    }

    //---------------------------------------------------

    private static EventManagerNoParamater Get(){
        Type type = typeof(EventManagerNoParamater);

        if (eventManagers.ContainsKey(type)){
            return eventManagers[type] as EventManagerNoParamater;
        }
        else{
            EventManagerNoParamater newEventManager = new EventManagerNoParamater();
            eventManagers.Add(type, newEventManager);
            return newEventManager;
        }
    }

    private static EventManagerParameter<T> Get<T>(){
        Type type = typeof(T);

        if (eventManagers.ContainsKey(type)){
            return eventManagers[type] as EventManagerParameter<T>;
        }
        else{
            EventManagerParameter<T> newEventManager = new EventManagerParameter<T>();
            eventManagers.Add(type, newEventManager);
            return newEventManager;
        }
    }
}

public class EventManagerNoParamater
{
    private Dictionary<string, Action> eventDictionary = new Dictionary<string, Action>();

    //---------------------------------------------------

    public void AddListener(string eventName, Action listener){
        if (eventDictionary.TryGetValue(eventName, out Action currentEvent)){
            currentEvent += listener;
            eventDictionary[eventName] = currentEvent;
        }
        else{
            currentEvent += listener;
            eventDictionary.Add(eventName, currentEvent);
        }
    }

    public void RemoveListener(string eventName, Action listener){
        if (eventDictionary.TryGetValue(eventName, out Action currentEvent)){
            currentEvent -= listener;
            eventDictionary[eventName] = currentEvent;
        }
    }

    public void Invoke(string eventName){
        if (eventDictionary.TryGetValue(eventName, out Action thisEvent)){
            thisEvent.Invoke();
        }
        #if UNITY_EDITOR
            else{
                Debug.LogWarning("Tried to invoke event " + eventName + " but there are no listeners");
            }
        #endif
    }
}

public class EventManagerParameter<T>
{
    private Dictionary<string, Action<T>> eventDictionary = new Dictionary<string, Action<T>>();

    //---------------------------------------------------

    public void AddListener(string eventName, Action<T> listener){
        Action<T> currentEvent;
        if (eventDictionary.TryGetValue(eventName, out currentEvent)){
            currentEvent += listener;
            eventDictionary[eventName] = currentEvent;
        }
        else{
            currentEvent += listener;
            eventDictionary.Add(eventName, currentEvent);
        }
    }

    public void RemoveListener(string eventName, Action<T> listener){
        Action<T> currentEvent;
        if (eventDictionary.TryGetValue(eventName, out currentEvent)){
            currentEvent -= listener;
            eventDictionary[eventName] = currentEvent;
        }
    }

    public void Invoke(string eventName, T eventParam){
        Action<T> currentEvent = null;
        if (eventDictionary.TryGetValue(eventName, out currentEvent)){
            currentEvent.Invoke(eventParam);
        }
        #if UNITY_EDITOR
            else{
                Debug.LogWarning("Tried to invoke event " + eventName + " but there are no listeners");
            }
        #endif
    }
}