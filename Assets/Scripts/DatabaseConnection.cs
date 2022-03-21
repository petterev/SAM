using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor.Playables;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;

public class DatabaseConnection : MonoBehaviour
{
    private string secretKey = "mySecretKey"; // Edit this value and make sure it's the same as the one stored on the server
    public string addScoreURL = ""; //be sure to add a ? to your url
    public string highscoreURL = "";
    public TextMeshProUGUI _name, score, sessionId, clock, seconds, seconds37;
    public Button button;
    private char[] base37 = {'a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','x','.','0','1','2','3','4','5','6','7','8','9'};

    private void Start()
    {
        DateTime d = DateTime.Now;
        sessionId.text = GetRandomString(6);
        int sec = d.Hour * 1440 + d.Minute * 60 + d.Second;
        seconds.text = sec.ToString();

        int str37a = sec / (37 * 37);       
        int str37b = (sec - (str37a * 37 * 37)) /37;       
        int str37c = sec - (str37a * 37 * 37 ) - (str37b * 37);   
       
        var sec37 = (base37[str37a]+""+base37[str37b]+""+base37[str37c]);
     
        seconds37.text = sec37.ToString();
    }
    private void Update()
    {
        clock.text = DateTime.Now.ToString();
    }

    private string GetRandomString(int length) 
    {
        string d = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss");
        //return Guid.NewGuid().ToString("n").Substring(0, length);
        
        
        return d.Substring(0,2)+d.Substring(3,2);
    }
    private string GetSessionIdGuid() 
    {
        return Guid.NewGuid().ToString("n").Substring(0, 6);

    }

    private KeyValuePair<string, string>[] scores;
    public KeyValuePair<string, string>[] Scores
    {
        get { return scores; }
    }

    public void PressButton() 
    {
        int num;
        int.TryParse(score.text, out num);

        StartCoroutine( PostScores(_name.text, num));
    
    }
    public string Md5Sum(string strToEncrypt)
    {
        System.Text.UTF8Encoding ue = new System.Text.UTF8Encoding();
        byte[] bytes = ue.GetBytes(strToEncrypt);

        // encrypt bytes
        System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
        byte[] hashBytes = md5.ComputeHash(bytes);

        // Convert the encrypted bytes back to a string (base 16)
        string hashString = "";

        for (int i = 0; i < hashBytes.Length; i++)
        {
            hashString += System.Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');
        }

        return hashString.PadLeft(32, '0');
    }
    // Send the new score to the database
    public IEnumerator PostScores(string name, int score)
    {
        //This connects to a server side php script that will add the name and score to a MySQL DB.
        // Supply it with a string representing the players name and the players score.
        string hash = Md5Sum(name + score + secretKey);

        string post_url = addScoreURL + "name=" + UnityWebRequest.EscapeURL(name) + "&score=" + score + "&hash=" + hash;

        // Post the URL to the site and create a download object to get the result.
        Debug.Log("Submitting score");
        UnityWebRequest hs_post = new UnityWebRequest(post_url);
        yield return hs_post; // Wait until the download is done
        Debug.Log("Score submitted");

        if (hs_post.error != null)
        {
            Debug.Log("There was an error posting the high score: " + hs_post.error);
        }
    }

    // Get the scores from the database
  //  public IEnumerator GetScores(Action<int> returnCode)
  //  {
  //      scores = new KeyValuePair<string, string>[1];
  //      scores[0] = new KeyValuePair<string, string>("Loading Scores", "");
  //
  //      UnityWebRequest hs_get = new unityweb(highscoreURL);
  //      yield return hs_get;
  //
  //      if (hs_get.error != null)
  //      {
  //          Debug.Log("There was an error getting the high score: " + hs_get.error);
  //          scores[0] = new KeyValuePair<string, string>("There was an error getting the high score", hs_get.error);
  //          returnCode(1);
  //      }
  //      else
  //      {
  //          // split the results into an array
  //          Regex regex = new Regex(@"[\t\n]");
  //          string[] rawScores = regex.Split(hs_get.text);
  //
  //          // Restructure the string array into an array of KeyValuePairs
  //          scores = new KeyValuePair<string, string>[rawScores.Length / 2];
  //          int rawScoreIndex = 0;
  //          for (int i = 0; i < rawScores.Length / 2; i++)
  //          {
  //              scores[i] = new KeyValuePair<string, string>(rawScores[rawScoreIndex], rawScores[rawScoreIndex + 1]);
  //              rawScoreIndex += 2;
  //          }
  //
  //          returnCode(0);
  //      }
  //  }
}
