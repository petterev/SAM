using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageLogOld : MonoBehaviour
{
    [SerializeField]
    List<Message> msgList = new List<Message>();

    public GameObject messagePanel, message;
    public static MessageLogOld instance;

    private void Awake()
    {
        if (instance == null) 
        {
            instance = this;        
        }

    }


    public void SendMessageToLog(string txt)        
    {
        Message newMessage = new Message();
        newMessage.text = txt;

        msgList.Add(newMessage);

        GameObject newtext = Instantiate(message, messagePanel.transform);
        newMessage.textObj = newtext.GetComponent<Text>();

        newMessage.textObj.text = newMessage.text;
    }
}

[System.Serializable]
public class Message 
{
    public string text;
    public Text textObj;

}
