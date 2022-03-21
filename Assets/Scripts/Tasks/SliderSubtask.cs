using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderSubtask : Subtask
{
    public float targetValue;
    
    public Slider slider;

    public SliderSubtask(int tv, Slider sl)
    {
        targetValue = tv;
       
        slider = sl;
    }

    public override string PrintTask()
    {
        if(complete == true)
        {
            return "Slider task done";
        }

        return slider.name + ": " + targetValue;
       // return description.text;
    }
    public override bool Check(Vehicle veh)
    {
        

     
        if (targetValue != slider.value )
        {
            return false;
        }
        MessageLog.instance.SendMessageToLog("Subtask Done");

        return true;
    }
}
