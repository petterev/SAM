/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TaskManagerOld : MonoBehaviour
{
    public static TaskManagerOld instance;
    public Car activeCar;
    List<Car> cars;
    public List<TaskOld> tasks;
    public Button carButton1, carButton2, carButton3, actionButton1, actionButton2, actionButton3;
    public TextMeshProUGUI activeCarText;
    private TaskOld[] TaskPool;

    private void Awake()
    {


        if (instance == null) 
        {
            instance = this;
        }
    }
    private void Start()
    {
        TaskPool = new TaskOld[5];
        Car car1 = new Car("car1");
        Car car2 = new Car("car2");
        Car car3 = new Car("car3");

        carButton1.onClick.AddListener(() =>ChangeActiveCar(car1));
        carButton2.onClick.AddListener(() => ChangeActiveCar(car2));
        carButton3.onClick.AddListener(() => ChangeActiveCar(car3));

        actionButton1.onClick.AddListener(() => CheckAction(Action.action1));
        actionButton2.onClick.AddListener(() => CheckAction(Action.action2));
        actionButton3.onClick.AddListener(() => CheckAction(Action.action3));

       // TaskOld t = new TaskOld( new Action[] { Action.action1, Action.action2, Action.action3 });

      //  IssueTask(car1, t);
      //  Debug.Log(t.Print());
      //  MessageLogOld.instance.SendMessageToLog("sssadw");
    }
    public void ChangeActiveCar(Car car) 
    {
        activeCar = car;
        activeCarText.text = car.name;
    }
    public void IssueTask(Car car, TaskOld task)
    {
        car.task = task;
    
    
    }
    public void Action1() { }
    public void Action2() { }
    public void Action3() { }

    public void CheckAction(Action action)
    {
        if (activeCar.task == null)
        {
            Debug.Log("car has no task");
            return;
        }

        Action nextAction = activeCar.task.actions.Peek();
        
        if (nextAction != action) 
        {
            Debug.Log("incorrect action");
            return;
        }
        activeCar.task.actions.Pop();
        if (activeCar.task.actions.Count == 0)
        {
            Debug.Log("Task complete");
        }

    }
}

[System.Serializable]
public class TaskOld
{
    
    public Stack<Action> actions;

    public TaskOld( Action[] act) 
    {
        actions = new Stack<Action>();
        foreach (Action a in act)
        {
            actions.Push(a);
        }
    }
    public string Print()
    {
        string s = "Actions to take: ";
        for(int i = 0; i <= actions.Count + 1; i++)
        {
            s += actions.Peek().ToString() + " ";
            actions.Pop();
        }
       

        return s;
    }
}

[System.Serializable]
public class Car 
{
    public TaskOld task;
    public string name;

    public Car(string n) 
    { name = n; }

}
//[System.Serializable]
//public enum Action { action1, action2, action3 }
*/