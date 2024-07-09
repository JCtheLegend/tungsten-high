using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
public static class PlayerInventory
{
    public static Dictionary<string, int> inventory = new Dictionary<string, int>();

    public static List<string> condition = new List<string>();
    public static void UpdateInventory(string item, int changeAmt)
    {
        if(!inventory.ContainsKey(item))
        {
            inventory[item] = 0;
        }
        inventory[item] += changeAmt;
    }

    public static int CheckInventory(string item)
    {
        if (!inventory.ContainsKey(item))
        {
            return 0;
        }
        return inventory[item];
    }
}