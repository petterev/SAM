using UnityEngine;
using System.Collections;
using System;
using System.Data;
using Npgsql;
using System.IO;
using System.Collections.Generic;
using TMPro;

/// <summary>
/// 
/// </summary>
public class Npsgql : MonoBehaviour
{
    public string server;   //localhost
    public string database; //newdb
    public string UserId;   //postgres
    public string password; //fleotmgv45D.
    public TextMeshProUGUI connStatus;
    public string sesId;

    public string connectionString =
      "Server=localhost;" +
      "Database=newdb;" +
      "User ID=postgres;" +
      "Password=fleotmgv45D.;";

   

    NpgsqlConnection dbcon;
    DateTime startTime;

    private void Start()
    {
        connectionString = "Server=" + server + ";Database=" + database + ";User ID=" + UserId + ";Password=" + password + ";";
        sesId = GetSessionIdGuid();
       // Debug.Log(sesId);
        startTime = DateTime.Now;

        //  TestConnection();
        TestPost();
    }
    private void TestConnection() 
    {
        try
        {
            dbcon = new NpgsqlConnection(connectionString);
            dbcon.Open();
            connStatus.text = "Datrabase connection succesful";
        }
        catch
        {
            connStatus.text = "Database conection failed";
        }
        dbcon.Close();
      
    }
    private void GetData()
    {

       dbcon = new NpgsqlConnection(connectionString);
       dbcon.Open();

        NpgsqlCommand dbcmd = dbcon.CreateCommand();
        //string sql = "SELECT name, score " +"FROM public.table";
        dbcmd.CommandText = "SELECT name, score " + "FROM public.table";

        NpgsqlDataReader reader = dbcmd.ExecuteReader();
        while (reader.Read())
        {
            string name = (string)reader["name"];
            int score = (int)reader["score"];

            Debug.Log("Name: " + name + " " + score);
        }

        // clean up
        reader.Close();
        //reader = null;
        dbcmd.Dispose();
        dbcon.Close();
        //dbcmd = null;
    
    }

    private string GetSessionIdGuid()
    {
        return Guid.NewGuid().ToString("n").Substring(0, 6);

    }

    public void TestPost()
    {
        DateTime now = DateTime.Now;
        //  Debug.Log((now - startTime).Seconds);
        //DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss") 2016-06-22 19:10:25
        
        
        string time = now.Year +"-" +
            now.Month.ToString().PadLeft(2, '0') + "-" +
            now.Day.ToString().PadLeft(2, '0') + " " +
            now.Hour.ToString().PadLeft(2, '0') + ":" +
            now.Minute.ToString().PadLeft(2, '0') + ":" +
            now.Second.ToString().PadLeft(2, '0');
        
        Debug.Log(time);
       // int code = UnityEngine.Random.Range(0, 100);
        //PostEvent(sesId, time, code);
    }
    public string GetTimestamp() 
    {
        DateTime now = DateTime.Now;
        
        return now.Year + "-" +
            now.Month.ToString().PadLeft(2, '0') + "-" +
            now.Day.ToString().PadLeft(2, '0') + " " +
            now.Hour.ToString().PadLeft(2, '0') + ":" +
            now.Minute.ToString().PadLeft(2, '0') + ":" +
            now.Second.ToString().PadLeft(2, '0');
    }

    private async void PostSession()
    {
        dbcon = new NpgsqlConnection(connectionString);
        await dbcon.OpenAsync();

        var sql = "INSERT INTO sam.sessions (session_id, start_time, end_time) " + "VALUES (@p1, @p2, @p3)";
        using var cmd = new NpgsqlCommand(sql, dbcon);

         cmd.Parameters.AddWithValue("p1", sesId);
         cmd.Parameters.AddWithValue("p2", startTime);
         cmd.Parameters.AddWithValue("p3", DateTime.Now);
         cmd.Prepare();
      
         await cmd.ExecuteNonQueryAsync();
      
         cmd.Dispose();
        //cmd = null;
        dbcon.Close();
   

    }
    private async void PostEvent(string sesID, string time, string vehicle, int eventCode)
    {
        //OpenConnection();
          dbcon = new NpgsqlConnection(connectionString);
          await dbcon.OpenAsync();


        var sql = "INSERT INTO sam.events(session_id, time, event_code) values(@s, @t, @e)";
        using var cmd = new NpgsqlCommand(sql, dbcon);

        cmd.Parameters.AddWithValue("s", sesID);
        cmd.Parameters.AddWithValue("t", time);
        cmd.Parameters.AddWithValue("e", eventCode);
        cmd.Prepare();

        await cmd.ExecuteNonQueryAsync();
        cmd.Dispose();
        dbcon.Close();

    }
     private  void CreateAllCsvs()
     {
        List<string> ids = new List<string>();

        dbcon = new NpgsqlConnection(connectionString);
        dbcon.Open();

        NpgsqlCommand dbcmd = dbcon.CreateCommand();

        dbcmd.CommandText = "Select session_id FROM sam.sessions";

        NpgsqlDataReader reader = dbcmd.ExecuteReader();
        while (reader.Read())
        {
           
            string sessionId =(string)reader["session_id"];
            ids.Add(sessionId);
            Debug.Log(sessionId);
            //writer.WriteLine(id + sessionId + time + eventCode);

        }
        dbcmd.Dispose();
       
        dbcon.Close();
        foreach(string i in ids) 
        {
            CreateCSV(i+".scv", ';',i);

        }
    }
    private void CreateCSV(string filepath, char dl, string sid)
    {

        dbcon = new NpgsqlConnection(connectionString);
        dbcon.Open();

        NpgsqlCommand dbcmd = dbcon.CreateCommand();
       
        dbcmd.CommandText = "Select * FROM sam.events WHERE session_id like @p1 ";// + sessionId;
        dbcmd.Parameters.AddWithValue("@p1", sid);
        NpgsqlDataReader reader = dbcmd.ExecuteReader();
        StreamWriter writer = new StreamWriter(filepath);
        writer.WriteLine("id;session_id;time;event_code");
        while (reader.Read())
        {
            int id = (int)reader["id"];
            string sessionId = dl +(string)reader["session_id"] + dl;
            string time = (string)reader["time"] + dl;
            int eventCode = (int)reader["event_code"];
           // Debug.Log(id + sessionId + time + eventCode);
            writer.WriteLine(id + sessionId + time + eventCode);

        }
        writer.Close();
        // clean up
        reader.Close();
        reader = null;
        dbcmd.Dispose();
        dbcmd = null;
      

    }

    void OnApplicationQuit()
    {
        //CreateCSV("table.scv", ';', "215ea3");

        //Debug.Log("Application ending after " + Time.time + " seconds");
      //  PostSession();
       // CreateAllCsvs();
        //  CloseConnection();
    }

    private async void OpenConnection() 
    {
        dbcon = new NpgsqlConnection(connectionString);
        await dbcon.OpenAsync();
    }

    private void CloseConnection()
    {
        dbcon.Close();
        dbcon = null;
    }
    


    private async void PostData(string s, int i) 
    {
        var conn = new NpgsqlConnection(connectionString);
        await conn.OpenAsync();
     
        var sql = "INSERT INTO public.table (id,name, score) VALUES ('3',@name, @score)";
        using var cmd = new NpgsqlCommand(sql, conn);

        cmd.Parameters.AddWithValue("name", s);
        cmd.Parameters.AddWithValue("score", i);
        cmd.Prepare();

        await cmd.ExecuteNonQueryAsync();
        Debug.Log("data added");
        cmd.Dispose();
        //cmd = null;
        conn.Close();
        conn = null;

    }
  
}


