using System.Collections.Generic;
using UnityEngine;

// Who the player is friends with.
//
// A static store alongside PlayerInventory -- it survives scene loads for free, the same way
// GameManager does. Unlike PlayerInventory this one *is* written to gameData.json (see
// GameManager.SaveGameData), because a friendship is meant to be a lasting thing: the payoff is that
// you never have to pass that NPC's conversation check again, and that has to hold after a reload.
//
// NPCs are keyed by name -- SimpleDialogTrigger.speaker, which ObjectLoader sets from the `name` in
// the scene JSON. Those are the same names the portrait sheet uses, so the Friends tab can show a
// face for anyone in the roster.
public static class Friendship
{
    // Successful conversations needed before someone counts as a friend. Three means a friendship is
    // something you build over a few encounters rather than a switch the first chat flips.
    public const int FRIEND_THRESHOLD = 3;

    // Name -> points. An entry exists as soon as the player has had one real conversation, so the
    // roster can show acquaintances working their way up, not just finished friends.
    static readonly Dictionary<string, int> points = new Dictionary<string, int>();

    public static int Points(string npc)
    {
        int p;
        return !string.IsNullOrEmpty(npc) && points.TryGetValue(npc, out p) ? p : 0;
    }

    // True once this NPC will talk to the player without a conversation check.
    public static bool IsFriend(string npc)
    {
        return Points(npc) >= FRIEND_THRESHOLD;
    }

    // True if the player has met them at all -- i.e. they belong on the Friends tab.
    public static bool Knows(string npc)
    {
        return !string.IsNullOrEmpty(npc) && points.ContainsKey(npc);
    }

    // Move a friendship along. Returns true only on the step that crosses the threshold, so the
    // caller can announce it once and never again.
    public static bool Add(string npc, int amount)
    {
        if (string.IsNullOrEmpty(npc))
        {
            return false;
        }
        bool wasFriend = IsFriend(npc);
        int updated = Mathf.Max(Points(npc) + amount, 0);
        points[npc] = updated;
        return !wasFriend && IsFriend(npc);
    }

    // Make someone a friend outright, for a story beat that decides it rather than earning it.
    // Returns true if this is the moment they became one.
    public static bool MakeFriend(string npc)
    {
        // Top up to the threshold, never down: someone already past it stays where they are.
        return Add(npc, Mathf.Max(FRIEND_THRESHOLD - Points(npc), 0));
    }

    public static int FriendCount()
    {
        int n = 0;
        foreach (KeyValuePair<string, int> entry in points)
        {
            if (entry.Value >= FRIEND_THRESHOLD)
            {
                n++;
            }
        }
        return n;
    }

    // Everyone the player has met, friends first, then closest acquaintances, then alphabetical --
    // so the Friends tab opens on the people who matter without needing a scroll.
    public static List<string> Roster()
    {
        List<string> names = new List<string>(points.Keys);
        names.Sort(delegate (string a, string b)
        {
            int byPoints = Points(b).CompareTo(Points(a));
            return byPoints != 0 ? byPoints : string.Compare(a, b, System.StringComparison.Ordinal);
        });
        return names;
    }

    // ---- Persistence --------------------------------------------------------------------------
    // JsonUtility cannot serialize a Dictionary, so gameData.json holds two parallel arrays.

    public static string[] SaveNames()
    {
        return Roster().ToArray();
    }

    public static int[] SavePoints(string[] names)
    {
        int[] values = new int[names.Length];
        for (int i = 0; i < names.Length; i++)
        {
            values[i] = Points(names[i]);
        }
        return values;
    }

    // Rebuild from a loaded save. Tolerates missing or mismatched arrays -- an older gameData.json
    // written before friendships existed simply has none.
    public static void Restore(string[] names, int[] values)
    {
        points.Clear();
        if (names == null || values == null)
        {
            return;
        }
        for (int i = 0; i < names.Length && i < values.Length; i++)
        {
            if (!string.IsNullOrEmpty(names[i]))
            {
                points[names[i]] = values[i];
            }
        }
    }

    public static void Clear()
    {
        points.Clear();
    }
}
