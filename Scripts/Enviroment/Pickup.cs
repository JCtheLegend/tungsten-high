using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class PickupManager
{
    public static void CreatePickup(string name)
    {
        TextAsset t = Resources.Load<TextAsset>("Pickup/" + name);
        PickupData[] loadedPickups = JsonHelper.FromJson<PickupData>(t.text);
        string data = JsonHelper.ToJson(loadedPickups);
        File.WriteAllText(Application.persistentDataPath + "/Pickup/" + name + ".json", data);
    }
    public static void UpdatePickup(string name, string location)
    {
        string inData = File.ReadAllText(Application.persistentDataPath + "/Pickup/" + name + ".json");
        PickupData[] loadedPickups = JsonHelper.FromJson<PickupData>(inData);
        foreach (PickupData p in loadedPickups)
        {
           if(p.room == SceneManager.GetActiveScene().name && location == p.location)
           {
                p.obtained = true;
           }
        }
        string outData = JsonHelper.ToJson(loadedPickups);
        File.WriteAllText(Application.persistentDataPath + "/Pickup/" + name + ".json", outData);
    }

    public static void LoadPickup(string name)
    {
        GameObject g = Resources.Load<GameObject>("LoadPrefabs/Pickup/" + name);
        string data = File.ReadAllText(Application.persistentDataPath + "/Pickup/" + name + ".json");
        PickupData[] loadedPickups = JsonHelper.FromJson<PickupData>(data);
        foreach (PickupData p in loadedPickups)
        {
            if (!p.obtained && p.room == SceneManager.GetActiveScene().name)
            {
                var clone = GameObject.Instantiate(g, GameManager.ParseVector(p.location), Quaternion.identity);
                clone.name = g.name;
            }
        }
    }
}

[System.Serializable]
public class PickupData {
    public string room;
    public string location;
    public bool obtained;
}