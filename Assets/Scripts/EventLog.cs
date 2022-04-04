using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class EventLog : MonoBehaviour
{
    public GameObject messagePanel;
    public TextMeshProUGUI messagePrefab;
    public Toggle singleVehicleToggle;
    public static EventLog instance;
    public List<EventLogItem> events;
    public bool singleVehicleLog = true;
    public TextMeshProUGUI logText;

    public void Awake()
    {
        if (instance == null)
            instance = this;

        events = new List<EventLogItem>();

        singleVehicleToggle.onValueChanged.AddListener(delegate { SwichToggle(); } );

       
        //AddEventToLog(null, "aaaaaa aaaaaaaa aaaa aaaa", "");
   
        //AddEventToLog(null, "aaaaaa", "");
    }

    public void SwichToggle()
    {
        
        singleVehicleLog = singleVehicleToggle.isOn;
       // Debug.Log(singleVehicleToggle.isOn);
        GetEvents(TaskManager.instance.ActiveVehicle);
    }
    public void GetEvents(Vehicle v)
    {
        ClearLog();
        foreach (EventLogItem m in events)
        {
            if(singleVehicleLog == true && m.vehicle != v)
            {
                continue;
            }
           // var newMessage = Instantiate(messagePrefab, messagePanel.transform);
           // newMessage.text = m.vehicle.name + ": " + m.message;
             logText.text += m.vehicle.name + ": " + m.message + "\n";
        }

    }
    public void AddEventToLog(Vehicle v,  string s, string t)
    {
        events.Add(new EventLogItem(v, s, t));
      
        if (v == TaskManager.instance.ActiveVehicle)
        {
            logText.text += v.name + ": " + s + "\n";
            // var newMessage = Instantiate(messagePrefab, messagePanel.transform);
            //  newMessage.text = v.name + ": " + s;
        }
   

    }
    public void ClearLog()
    {
        logText.text = "";
        //foreach(Transform child in messagePanel.transform)
        //{
        //    Destroy(child.gameObject);
        //}

    }
 
        
}
public class EventLogItem 
{
    public Vehicle vehicle;
    public string message;
    public string timeStamp;

    public EventLogItem(Vehicle v, string m, string t)
    {
        vehicle = v;
        message = m;
        timeStamp = t;
    }

}