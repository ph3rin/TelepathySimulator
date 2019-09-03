using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

internal class GameSession : MonoBehaviour
{
    private string m_sessionName;
    private string[] m_players;
    public static GameSession Instance { get; set; }
    public GameSessionRecord Record { get; private set; }
    private void Awake()
    {
        Debug.Assert(Instance is null);
        if (Instance is null)
        {
            Instance = this;
        }
    }
    private void OnDestroy()
    {
        Instance = null;
    }
    //public void Init(string user1, string user2)
    //{
    //    m_sessionName = $"{user1.ToLower()}-{user2.ToLower()}";
    //    m_players = new string[] { user1, user2 };
    //    Record = GameSessionRecord.LoadOrCreate(user1, user2);
    //    Record.SetGameRecord("@LastLogin", DateTime.UtcNow);
    //    SyncRecord();
    //}
    public void Init(GameSessionRecord record)
    {
        DontDestroyOnLoad(gameObject);
        m_sessionName = record.GetName();
        bool result = record.TryGetGameRecord<string[]>("@Players", out m_players);
        Debug.Assert(result);
        Record = record;
        Record.SetGameRecord("@LastLogin", DateTime.UtcNow);
        SyncRecord();
    }
    public void SyncRecord()
    {
        var filePath = $@"{Application.persistentDataPath}/{m_sessionName}.json";
        using (var fs = File.Open(filePath, FileMode.Create))
        using (var sw = new StreamWriter(fs))
        {
            var jsonDeserializer = new JsonSerializer() { TypeNameHandling = TypeNameHandling.Auto };
            jsonDeserializer.Serialize(sw, Record);
        }
    }
    public void Logout()
    {
        SyncRecord();
        Destroy(gameObject);
    }

}