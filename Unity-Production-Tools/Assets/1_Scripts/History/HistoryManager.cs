using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HistoryManager 
{
    private Stack<IHistory> undoHistory = new Stack<IHistory>();
    private Stack<IHistory> redohistory = new Stack<IHistory>();

    public void AddHistory(IHistory history){
        undoHistory.Push(history);
        redohistory.Clear();
    }

    public void Undo(){
        if (undoHistory.Count == 0) return;

        IHistory history = undoHistory.Pop();
        history.Undo();
        redohistory.Push(history);
    }

    public void Redo(){
        if (redohistory.Count == 0) return;

        IHistory history = redohistory.Pop();
        history.Redo();
        undoHistory.Push(history);
    }
}
