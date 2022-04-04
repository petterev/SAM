using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class TaskManager : MonoBehaviour
{
    //UI Elements
    public GameObject carButton;
    public Transform carPanel, taskPanel, newVehiclePanel;
    public Slider slider1, slider2;
    public Button taskButton, checkSlidersButton, taskButton1, taskButton2, taskButton3, newVehicleButton, checkInputButton;
    public TMP_InputField commandInput;
    public TextMeshProUGUI timer, userID, slider1Value, slider2Value, taskStatus;
    public TextMeshProUGUI carName, taskText;
    public Toggle toggle1, toggle2, toggle3;
    public TMP_Dropdown dropdown, newVehicleDropdown;

    public List<GameObject> taskList;

    public DateTime startTime;
    public string userId;
    public int vehicleCount;

    private Vehicle selectedVehicle;
    private List<Vehicle> vehicleList;
    public Vehicle ActiveVehicle
    {
        get { return selectedVehicle; }
        set { selectedVehicle = value; }

    }
    


    public static TaskManager instance;

    private void Awake()
    {
        vehicleCount = 0;
        vehicleList = new List<Vehicle>();
        taskList = new List<GameObject>();
        startTime = DateTime.Now;
        userId = Guid.NewGuid().ToString().Substring(0, 6);
        if(instance == null)
        {
            instance = this;

        }
        userID.text = userId;
    }

    void Start()
    {
        
        CreateVehicles();
        
        slider1.onValueChanged.AddListener(delegate { SliderValueChange(1, slider1); });
       
         slider2.onValueChanged.AddListener(delegate { SliderValueChange(2, slider2); });
        
        checkSlidersButton.onClick.AddListener(CheckSlidersQueue);
        dropdown.onValueChanged.AddListener(delegate { CheckDropdownQueue(); });
        taskButton1.onClick.AddListener(delegate { CheckButtonQueue(taskButton1); });
        taskButton2.onClick.AddListener(delegate { CheckButtonQueue(taskButton2); });
        taskButton3.onClick.AddListener(delegate { CheckButtonQueue(taskButton3); });
        checkInputButton.onClick.AddListener(CheckInputQueue );

        int i = UnityEngine.Random.Range(0, 10);

        ChangeActiveCar(vehicleList[i]);

        SliderSubtask s = new SliderSubtask(3, slider1);
        ButtonSubtask b = new ButtonSubtask(taskButton1);
        TextSubtask t = new TextSubtask("ddd");
        DropdownSubtask d = new DropdownSubtask(1, dropdown);
        DropdownSubtask d2 = new DropdownSubtask(3, dropdown);
        TextSubtask tt = new TextSubtask("aaa", commandInput);
        WaitSubtask wt = new WaitSubtask(5);
        Task ta = new Task(s, null, null, d);
        Subtask[] st = {d2, s, wt, tt, b };
        Task t2 = new Task(st) ;

        taskButton.onClick.AddListener(() => IssueTask(vehicleList[i], new Task(st)));

        for (int j = 0; j < 2000; ++j)
        {
            int r = UnityEngine.Random.Range(0, 20);
            EventLog.instance.AddEventToLog(vehicleList[r],"aaaa aaaa aaaa aaaaaa aaaaa" + j, "");
        }
    }

    private void Update()
    {
        timer.text = (DateTime.Now - startTime).Seconds.ToString();
        

    }

    public bool CheckBooleanValues(bool b, float f)
    {
        if (selectedVehicle.active != b)
            return false;
        if (selectedVehicle.batteryLoad != f)
            return false;

        return true;

    }
    public void SetCarText(string name)
    {
        carName.text = name;
        
      
    }
    public void ChangeActiveCar(Vehicle veh)
    {
        ActiveVehicle = veh;
        
        AdjustControlpanel();
        ClearTaskWindow();
        SetCarText(veh.Name);
        taskStatus.text = veh.description;
        EventLog.instance.GetEvents(veh);
        //PrintSubtasks(veh);
        PrintNextSubtask(veh);

    }
    public void PrintNextSubtask(Vehicle v)
    {
        ClearTaskWindow();
        if (v.task != null && v.task.taskQueue.Count != 0)
        {          

            TextMeshProUGUI newTaskText = Instantiate(taskText, taskPanel);
            Subtask s = v.task.taskQueue.Peek();
            newTaskText.text = s.PrintTask();
            taskList.Add(newTaskText.gameObject);
            var wt = s as WaitSubtask;
            if (wt != null)
            {
                StartCoroutine( CheckWaitQueue());
            }
            return;
        }
        v.ChangeColor(Color.green);

    }


    public void PrintSubtasks(Vehicle veh)
    {
        if (veh.task != null)
        {
            foreach (Subtask s in veh.task.subtasks)
            {
                if (s == null)
                {
                    continue;
                }
                TextMeshProUGUI newTaskText = Instantiate(taskText, taskPanel);
                newTaskText.text = s.PrintTask();
                taskList.Add(newTaskText.gameObject);


            }
        }

    }
    public void ClearTaskWindow()
    {
        foreach(GameObject t in taskList)
        {
            Destroy(t);
        }

    }
   public void IssueTask(Vehicle v, Task task) 
   {
        v.task = task;
       // Debug.Log(v.task.subtasks[(int)subtaskType.text]);
       // v.UpdateTaskStatus();
        v.ChangeColor(Color.yellow);
     
   
        EventLog.instance.AddEventToLog(null,"new task: "+ v.name,"");
   }


   

    private void AdjustControlpanel() 
    {
        slider1.value = selectedVehicle.slider1;
        slider1Value.text = selectedVehicle.slider1.ToString();

        slider2.value = selectedVehicle.slider2;
        slider2Value.text = selectedVehicle.slider2.ToString();

        toggle1.isOn = selectedVehicle.toggle1;
        toggle2.isOn = selectedVehicle.toggle2;
        toggle3.isOn = selectedVehicle.toggle3;

        dropdown.value = selectedVehicle.dropdown;
    
    
    }
    private void SliderValueChange(int i, Slider s) 
    {
        if(selectedVehicle == null)
        {
            return;
        }
        if (i == 1) 
        {
            selectedVehicle.slider1 = s.value;
            slider1Value.text = s.value.ToString();
        }
        if (i == 2) 
        {
            selectedVehicle.slider2 = s.value;
            slider2Value.text = s.value.ToString();
        } 
    }

    public void CreateVehicle() 
    {
        GameObject newCar = Instantiate(carButton, carPanel);
        Vehicle newVehicle = newCar.GetComponent<Vehicle>();
        newVehicle.Name = "car00" + vehicleCount.ToString();
        newCar.name = "car00" + vehicleCount.ToString();
        newCar.GetComponent<Button>().onClick.AddListener(delegate { ChangeActiveCar(newVehicle);  });
        vehicleCount++;
        vehicleList.Add(newVehicle);
    }
    public void CreateVehicles() 
    { 
        for(int i =0; i < 20; ++i) 
        {
            CreateVehicle();
        }
    }
    public void CheckInputQueue() 
    {
        if (selectedVehicle.task == null || selectedVehicle.task.taskQueue.Count == 0)
        {
            Debug.Log("no task");
            return;
        }
        var tt = selectedVehicle.task.taskQueue.Peek() as TextSubtask;
        if (tt != null)
        {
            if (tt.Check(ActiveVehicle) == false)
            {
                selectedVehicle.task.mistakes++;
                Debug.Log("wrong values");
                return;
            }

            //  ClearTaskWindow();
            selectedVehicle.task.NextSubTask();
            PrintNextSubtask(selectedVehicle);

            return;
        }
        EventLog.instance.AddEventToLog(null,"wrong action","");
        selectedVehicle.task.mistakes++;
    }
    public void CheckButtonQueue(Button btn)
    {
        if (selectedVehicle.task == null || selectedVehicle.task.taskQueue.Count == 0)
        {
            Debug.Log("no task");
            return;
        }
        var bt = selectedVehicle.task.taskQueue.Peek() as ButtonSubtask;
        if (bt != null)
        {
            if (bt.Check(btn) == false)
            {
                selectedVehicle.task.mistakes++;
                Debug.Log("wrong values");
                return;
            }

            //  ClearTaskWindow();
            selectedVehicle.task.NextSubTask();
            PrintNextSubtask(selectedVehicle);

            return;
        }
        EventLog.instance.AddEventToLog(null,"wrong action","");
        selectedVehicle.task.mistakes++;
    }
    public IEnumerator CheckWaitQueue()
    {
        if (selectedVehicle.task == null || selectedVehicle.task.taskQueue.Count == 0)
        {
            Debug.Log("no task");
            yield return null;
        }
        var wt = selectedVehicle.task.taskQueue.Peek() as WaitSubtask;
        if (wt != null)
        {
            yield return new WaitForSeconds(wt.timeToWait);

            //  ClearTaskWindow();
            selectedVehicle.task.NextSubTask();
            PrintNextSubtask(selectedVehicle);

            yield return null;
        }
        //MessageLog.instance.SendMessageToLog("wrong action");
        //selectedVehicle.task.mistakes++;

    }

    public void CheckDropdownQueue()
    {
        selectedVehicle.dropdown = dropdown.value;        
       
        if(selectedVehicle.task == null || selectedVehicle.task.taskQueue.Count == 0)
        {
            Debug.Log("no task");
            return;
        }

        var st = selectedVehicle.task.taskQueue.Peek() as DropdownSubtask; 
        if(st != null)
        {
            if (st.Check(selectedVehicle) == false)
            {
                selectedVehicle.task.mistakes++;
                Debug.Log("wrong values");
                return;
            }
         
            //  ClearTaskWindow();
            selectedVehicle.task.NextSubTask();
            PrintNextSubtask(selectedVehicle);
          
            return;
        }
        EventLog.instance.AddEventToLog(null,"wrong action","");
        selectedVehicle.task.mistakes++;
    }


    public void CheckSlidersQueue()
    {
        if (selectedVehicle.task == null || selectedVehicle.task.taskQueue.Count == 0)
        {
            Debug.Log("no task");
            return;
        }
        var st = selectedVehicle.task.taskQueue.Peek() as SliderSubtask;

        if (st != null)
        {
            if (st.Check(selectedVehicle) == false)
            {
                selectedVehicle.task.mistakes++;
                Debug.Log("wrong values");
                return;
            }
          
            //  ClearTaskWindow();
            selectedVehicle.task.NextSubTask();
            PrintNextSubtask(selectedVehicle);
           
            return;
        }
        EventLog.instance.AddEventToLog(null,"wrong action","");
        selectedVehicle.task.mistakes++;
    }

}

public enum Action { action1, action2, action3 }

/*
 *    public void CheckDropdown() 
    {
        Vehicle veh = TaskManager.instance.ActiveVehicle;

        if (veh.activeTasks[(int)subtaskType.dropdown] == false)
        {
            Debug.Log("no dropdown task");
            return;
        }
        DropdownSubtask dropdownTask = (DropdownSubtask)veh.task.subtasks[(int)subtaskType.dropdown];

        if(dropdown.value != dropdownTask.targetValue)
        {
            Debug.Log("wrong values");
            return;
        }
        MessageLog.instance.SendMessageToLog("Task Done");
        veh.activeTasks[(int)subtaskType.dropdown] = false;
        return;
    }
 * 
 * 
 * 
    public void CheckSliders()
    {
        Vehicle veh = TaskManager.instance.ActiveVehicle;
       
       // 
        if (veh.activeTasks[(int)subtaskType.slider] == false) 
        {
            Debug.Log("no slider task");
            return;
        }
        SliderSubtask sliderTask = (SliderSubtask)veh.task.subtasks[(int)subtaskType.slider];
        
       
        if (slider1.value != sliderTask.slider1 || slider2.value != sliderTask.slider2)
        {
            Debug.Log("wrong values");
            return;
        }
        MessageLog.instance.SendMessageToLog("Task Done");
        veh.activeTasks[(int)subtaskType.slider] = false;
        return;    
       
 
        
    }
 */