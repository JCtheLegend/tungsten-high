using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RoomManager : MonoBehaviour
{
    // Legacy fallback: parallel arrays indexed by RoomExit.toEntranceNum. Prefer placing Entrance
    // marker objects in the scene instead (see Entrance.cs) — the resolver below tries those first.
    public Vector2[] entrances;
    public direction[] entranceDirs;
    public string[] entranceSounds;

    public string songName;

    private void Start()
    {
        MusicController music = GameObject.Find("Music Manager").GetComponent<MusicController>();
        if (songName != "" && (music.audioSource.clip == null || music.audioSource.clip.name != songName))
        {
            music.ChangeSong(songName);
        }
    }

    // Find the Entrance marker in this scene with the given id, or null if there is none.
    public Entrance GetEntrance(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return null;
        }
        foreach (Entrance e in FindObjectsOfType<Entrance>())
        {
            if (e.id == id)
            {
                return e;
            }
        }
        return null;
    }

    // Resolve where the player should spawn, using the pending GameManager.RoomData set by the door
    // that was just used. Prefers an Entrance marker matching toEntranceId; otherwise falls back to
    // the legacy entrances[] arrays indexed by toEntranceNum. Returns false if neither resolves.
    public bool TryGetSpawn(out Vector2 position, out direction facing, out string sound)
    {
        string id = GameManager.RoomData.toEntranceId;
        Entrance marker = GetEntrance(id);
        if (marker != null)
        {
            position = marker.SpawnPosition();
            facing = marker.facing;
            sound = marker.sound;
            return true;
        }
        if (!string.IsNullOrEmpty(id))
        {
            Debug.LogWarning("No Entrance with id '" + id + "' in scene '" + gameObject.scene.name +
                             "'; falling back to legacy entrance index " + GameManager.RoomData.toEntranceNum + ".");
        }

        int num = GameManager.RoomData.toEntranceNum;
        if (entrances != null && num >= 0 && num < entrances.Length)
        {
            position = entrances[num];
            facing = (entranceDirs != null && num < entranceDirs.Length) ? entranceDirs[num] : direction.down;
            sound = (entranceSounds != null && num < entranceSounds.Length) ? entranceSounds[num] : "";
            return true;
        }

        position = Vector2.zero;
        facing = direction.down;
        sound = "";
        return false;
    }
}
