using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
/// <summary>
/// Parent class for subtasks
/// </summary>
public abstract class Subtask
{
    public TextMeshProUGUI description;
    public bool complete;

    public abstract string PrintTask();
    public abstract bool Check(Vehicle veh);

    
}
public enum subtaskType {slider,button, text, dropdown }
