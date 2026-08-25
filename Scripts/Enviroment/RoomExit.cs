using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomExit : MonoBehaviour
{
    public string roomName;

    [Tooltip("Preferred: the id of the Entrance marker to spawn at in the destination scene. " +
             "If set and a matching Entrance exists, it wins over toEntranceNum.")]
    public string toEntranceId = "";

    [Tooltip("Legacy fallback: index into the destination RoomManager's entrances[] arrays. " +
             "Used only when toEntranceId is blank or no matching Entrance is found.")]
    public int toEntranceNum;

    public bool locked = false;
}
