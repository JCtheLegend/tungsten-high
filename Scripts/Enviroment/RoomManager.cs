using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RoomManager : MonoBehaviour
{
    public Vector2[] entrances;
    public direction[] entranceDirs;
    public string[] entranceSounds;

    public static class RoomData
    {
        public static int entranceNum = 0;

        public static direction entranceDirection = direction.left;
    }
}
