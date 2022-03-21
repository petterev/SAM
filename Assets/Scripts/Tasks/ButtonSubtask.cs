using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSubtask : Subtask
{
    public Stack<Action> actions = new Stack<Action>();
    public Button button;


    public ButtonSubtask(Button btn)
    {
        button = btn;

    }

    public override string PrintTask()
    {
        return "press button " + button.name;
    }
    public bool Check(Button btn) 
    {

        if (btn != button)
        {
            return false;
        }
        MessageLog.instance.SendMessageToLog("Subtask Done");

        return true;

    }
    public override bool Check(Vehicle veh)
    {
        
        
        
        return true;
    }
    public void CheckAction(Action action)
    {
        Vehicle veh = TaskManager.instance.ActiveVehicle;

        if (veh.buttonTask == null)
        {
            Debug.Log("car has no task");
            return;
        }

        Action nextAction = veh.buttonTask.actions.Peek();

        if (nextAction != action)
        {
            Debug.Log("incorrect action");
            return;
        }
        veh.buttonTask.actions.Pop();
        if (veh.buttonTask.actions.Count == 0)
        {
            Debug.Log("Task complete");
        }

    }
}
