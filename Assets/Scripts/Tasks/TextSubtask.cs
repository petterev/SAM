using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextSubtask : Subtask
{
    public string command;
    public TMP_InputField inputField;

    public TextSubtask(string s)
    {

        command = s;
    }

    public TextSubtask(string cmd, TMP_InputField inf)
    {
        command = cmd;
        inputField = inf;

    }

    public override bool Check(Vehicle veh)
    {

        if (command != inputField.text)
        {
            return false;
        }
        MessageLog.instance.SendMessageToLog("Subtask Done");

        return true;
    }

    public override string PrintTask()
    {
        return "give command: " + command + " in " + inputField.name;
    }
}
