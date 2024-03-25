using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHistory
{
    public void Redo();
    public void Undo();
}
