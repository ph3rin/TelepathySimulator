using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

internal class GameSessionRecord
{
    public Dictionary<string, object> Records { get; set; } = new Dictionary<string, object>();
    public bool TryGetGameRecord<T>(string gameMode, out T result)
    {
        if (!Records.TryGetValue(gameMode, out var record))
        {
            result = default(T);
            return false;
        }
        try
        {
            result = (T)record;
            return true;
        }
        catch
        {
            result = default(T);
            return false;
        }
    }
    public void SetGameRecord(string gameMode, object record)
    {
        if (!Records.ContainsKey(gameMode))
        {
            Records.Add(gameMode, record);
        }
        else
        {
            Records[gameMode] = record;
        }
    }
    public string GetName()
    {
        TryGetGameRecord<string[]>("@Players", out var users);
        return $"{users[0].ToLower()}-{users[1].ToLower()}";
    }
    public static GameSessionRecord LoadFromFile(string fileName)
    {
        try
        {
            using (var fs = File.OpenRead(fileName))
            using (var sr = new StreamReader(fileName))
            {
                var serializer = new JsonSerializer() { TypeNameHandling = TypeNameHandling.Auto };
                return serializer.Deserialize<GameSessionRecord>(new JsonTextReader(sr));
            }
        }
        catch
        {
            return null;
        }
    }
    public static GameSessionRecord LoadFromFile(string user1, string user2)
    {
        return LoadFromFile($"{user1.ToLower()}-{user2.ToLower()}");
    }
    public static GameSessionRecord CreateNew(string user1, string user2)
    {
        try
        {
            GameSessionRecord record = new GameSessionRecord();
            var players = new string[] { user1, user2 };
            record.SetGameRecord("@Players", players);
            var path = $"{Application.persistentDataPath}/{record.GetName()}.json";
            using (var fs = File.Open(path, FileMode.Create))
            using (var sw = new StreamWriter(fs))
            {
                var jsonDeserializer = new JsonSerializer() { TypeNameHandling = TypeNameHandling.Auto };
                jsonDeserializer.Serialize(sw, record);
            }
            return record;
        }
        catch
        {
            return null;
        }
    }
    public static GameSessionRecord LoadOrCreate(string user1, string user2)
    {
        var record = LoadFromFile(user1, user2);
        if (record == null)
            record = CreateNew(user1, user2);
        return record;
    }
    public static List<GameSessionRecord> EnumerateSavedSessions()
    {
        List<GameSessionRecord> records = new List<GameSessionRecord>();
        foreach (var fileInfo in new DirectoryInfo(Application.persistentDataPath).EnumerateFiles("*.json", SearchOption.TopDirectoryOnly))
        {
            var record = LoadFromFile(fileInfo.FullName);
            if (record != null) records.Add(record);
        }
        records.Sort((a, b) =>
        {
            a.TryGetGameRecord<DateTime>("@LastLogin", out var c);
            b.TryGetGameRecord<DateTime>("@LastLogin", out var d);
            return -c.CompareTo(d);
        });
        return records;
    }
}