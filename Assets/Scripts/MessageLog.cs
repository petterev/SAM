using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MessageLog : MonoBehaviour
{
    public GameObject messagePanel;
    public TextMeshProUGUI messagePrefab;

    public static MessageLog instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }
    public void SendMessageToLog(string s)
    {
        var newMessage = Instantiate(messagePrefab, messagePanel.transform);
        newMessage.text = s;

    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
