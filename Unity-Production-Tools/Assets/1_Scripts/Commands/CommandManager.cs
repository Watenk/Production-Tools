using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CommandManager 
{
    private static Stack<ICommand> undoStack = new Stack<ICommand>();
    private static Stack<ICommand> redoStack = new Stack<ICommand>();

    public static void ExecuteCommand(ICommand command){
        command.Execute();
        undoStack.Push(command);
        redoStack.Clear();
    }

    public static void Undo(){
        if (undoStack.Count > 0){
            ICommand command = undoStack.Pop();
            command.Undo();
            redoStack.Push(command);
        }
    }

    public static void Redo(){
        if (redoStack.Count > 0){
            ICommand command = redoStack.Pop();
            command.Execute();
            undoStack.Push(command);
        }
    }
}
