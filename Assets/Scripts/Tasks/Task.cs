using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Each task consists of one or more subtasks an is usually assosiated with a Vehicle.
/// </summary>
public class Task 
{
    public DateTime startTime;
    public int mistakes;

    public Subtask[] subtasks;
    public SliderSubtask slidertask;
    public ButtonSubtask buttontask;
    public TextSubtask texttask;

    public Queue<Subtask> taskQueue = new Queue<Subtask>();

    public Task( SliderSubtask st, ButtonSubtask bt, TextSubtask tt,DropdownSubtask dt)
    {
        subtasks = new Subtask[4];

        subtasks[(int)subtaskType.slider] = st;
        subtasks[(int)subtaskType.button] = bt;
        subtasks[(int)subtaskType.text] = tt;
        subtasks[(int)subtaskType.dropdown] = dt;
        slidertask = st;
        buttontask = bt;
        texttask = tt;
    }
    public Task(Subtask[] subtasks)
    {
        startTime = DateTime.Now;

        foreach(Subtask s in subtasks)
        {
            taskQueue.Enqueue(s);

        }
       
    }
   public void NextSubTask()
    {
        taskQueue.Dequeue();
        if(taskQueue.Count == 0) 
        {
            Complete();
        }
    }
    public void Complete()
    {
        EventLog.instance.AddEventToLog(null,"Task complete. Time: " + (DateTime.Now - startTime).Seconds + " Mistakes: " + mistakes,"");

        TaskManager.instance.ActiveVehicle.task = null;
        
    }

    public string PrintNext()
    {
        if(taskQueue.Count == 0)
        {
            return "No task";
        }
        return taskQueue.Peek().PrintTask();

    }

    public List<string> PrintSubtasks() 
    {
        List<string> list = new List<string>();

        foreach (Subtask s in subtasks)
        { 
            if(s == null)
            {
                continue;
            }
            list.Add(s.PrintTask());
        }

        return list;
    }


    public void UpdateSubtasks()
    {
        foreach (Subtask s in subtasks)
        {
            s.Check(TaskManager.instance.ActiveVehicle);
        }
        if(subtasks.Length == 0)
        {

        }
    }

}
