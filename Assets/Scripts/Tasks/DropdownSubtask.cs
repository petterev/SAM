using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DropdownSubtask : Subtask
{
    public int targetValue;
    public TMP_Dropdown dropdown;


    public DropdownSubtask(int tv, TMP_Dropdown dd)
    {
        targetValue = tv;
        dropdown = dd;
    }
    public override bool Check(Vehicle veh)
    {      

        
        if (targetValue != dropdown.value)
        {
            return false;
        }
        EventLog.instance.AddEventToLog(null,"Subtask Done","");

        return true;
    }

    public override string PrintTask()
    {
        return dropdown.name + ": " + targetValue;
    }

  
}
