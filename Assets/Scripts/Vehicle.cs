using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Vehicle : MonoBehaviour
{
    private string carName = "";
    public TextMeshProUGUI nameText;
    private Button btn;
    public string description;
    
    public bool active  {get;set;}

    [Range(0, 1)]
    public float batteryLoad;
    public string route;
    public string streetPosition;
    public Tuple<float, float> coordinates;
    
    [Range(0, 10)]
    public float slider1, slider2;
    public int dropdown;
    public bool toggle1, toggle2, toggle3;

    public string Name
    {
        get { return carName; }
        set { carName = value; }
    }

    public Task task;
    public SliderSubtask sliderTask;
    public ButtonSubtask buttonTask;
    public TextSubtask textTask;
    public bool[] activeTasks;

    private void Awake()
    {
        slider1 = UnityEngine.Random.Range(0, 10);
        slider2 = UnityEngine.Random.Range(0, 10);
    }

    private void Start()
    {
        activeTasks = new bool[4];

       // task = new List<Subtask>();
       // tasks.Add(new ButtonTask());
        nameText.text = carName;

        btn = GetComponent<Button>();
       // btn.onClick.AddListener(() => TaskManager.instance.SetCarText(carName));
        ChangeColor(Color.green);
        

    }

    public void UpdateTaskStatus() 
    { 
        if(task != null)
        {
            for(int i =0; i <task.subtasks.Length; ++i) 
            { 
                if(task.subtasks[i] != null)
                {
                    activeTasks[i] = true;
                    Debug.Log(i);
                }
            }

        }
    }
 
    // public void AddTask(Subtask t)
    // {
    //     tasks.Add(t);
    //     ChangeColor(Color.yellow);
    // }
    // public void CompleteTask(Subtask t)
    // {
    //     tasks.Remove(t);
    //     
    // }

    public void ChangeColor(Color color)
    {
        GetComponent<Image>().color = color;
        
    }


}
