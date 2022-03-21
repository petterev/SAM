using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitSubtask : Subtask
{
    public int timeToWait;
    public override bool Check(Vehicle veh)
    {
        throw new System.NotImplementedException();
    }
    public WaitSubtask(int i)
    {
        timeToWait = i;

    }

    public IEnumerator Wait() 
    {
        Debug.Log("wait");
        yield return new WaitForSeconds(timeToWait);
        Debug.Log("done");
    }


    public override string PrintTask()
    {
        return "Wait " + timeToWait + " seconds.";
    }


}
