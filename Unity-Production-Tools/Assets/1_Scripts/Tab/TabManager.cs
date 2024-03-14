using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabManager
{
    private List<Tab> tabs = new List<Tab>();

    // References
    private CanvasManager canvasManager;

    //----------------------------------------

    public TabManager(){
        canvasManager = GameManager.GetService<CanvasManager>();
    }

    //---------------------------------------

    private void OnLoad(){

    }
}
