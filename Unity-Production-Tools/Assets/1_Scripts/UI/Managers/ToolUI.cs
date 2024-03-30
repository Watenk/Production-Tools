using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolUI
{
    public ToolUI(){
        References.Instance.SaveButton.onClick.AddListener(() => EventManager.Invoke(Events.OnSave));
        References.Instance.ExportButton.onClick.AddListener(() => EventManager.Invoke(Events.OnExport));
        References.Instance.LoadButton.onClick.AddListener(() => EventManager.Invoke(Events.OnLoad));
    }
}
