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

    // ---- Persistence --------------------------------------------------------------------------
    // JsonUtility cannot serialize a Dictionary, so gameData.json holds items and counts as two
    // parallel arrays, the same way Friendship does.

    public static string[] SaveItems()
    {
        return new List<string>(inventory.Keys).ToArray();
    }

    public static int[] SaveCounts(string[] items)
    {
        int[] counts = new int[items.Length];
        for (int i = 0; i < items.Length; i++)
        {
            counts[i] = CheckInventory(items[i]);
        }
        return counts;
    }

    // Rebuild from a loaded save. Missing arrays (a gameData.json from before these were saved)
    // just leave the inventory and the flags empty.
    public static void Restore(string[] items, int[] counts, string[] conditions)
    {
        inventory.Clear();
        condition.Clear();
        if (items != null && counts != null)
        {
            for (int i = 0; i < items.Length && i < counts.Length; i++)
            {
                if (!string.IsNullOrEmpty(items[i]))
                {
                    inventory[items[i]] = counts[i];
                }
            }
        }
        if (conditions != null)
        {
            condition.AddRange(conditions);
        }
    }
}