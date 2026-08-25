using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Direction
{ 
    public static direction ParseDirection(string s)
    {
        switch (s)
        {
            case "up":
                return direction.up;
            case "down":
                return direction.down;
            case "right":
                return direction.right;
            case "left":
                return direction.left;
            case "upLeft":
                return direction.upLeft;
            case "upRight":
                return direction.upRight;
            case "downLeft":
                return direction.downLeft;
            case "downRight":
                return direction.downRight;
            default:
                return direction.up;
        }
    }

    // Unit vector for a cardinal direction. Diagonals return their combined (unnormalized) vector.
    public static Vector2 ToVector(direction d)
    {
        switch (d)
        {
            case direction.up: return Vector2.up;
            case direction.down: return Vector2.down;
            case direction.left: return Vector2.left;
            case direction.right: return Vector2.right;
            case direction.upLeft: return new Vector2(-1, 1);
            case direction.upRight: return new Vector2(1, 1);
            case direction.downLeft: return new Vector2(-1, -1);
            case direction.downRight: return new Vector2(1, -1);
            default: return Vector2.down;
        }
    }

    // Nearest cardinal direction for a movement vector, used to pick a facing/animation for a
    // heading that may be diagonal. A zero vector falls back to `down`, matching ToVector.
    public static direction FromVector(Vector2 v)
    {
        if (Mathf.Abs(v.x) > Mathf.Abs(v.y))
        {
            return v.x > 0 ? direction.right : direction.left;
        }
        if (Mathf.Abs(v.y) > 0)
        {
            return v.y > 0 ? direction.up : direction.down;
        }
        return direction.down;
    }
}
public enum direction { right, left, up, down, upRight, upLeft, downRight, downLeft };

public enum stage { pre, pr, gym, lunch, psych, sci, post, dream}

public class Flags
{
   public bool hasPlanner = true;
}

public static class GameManager
{
    // Which chapter (week) we're in. Cutscene JSON lives under Resources/Cutscenes/Chapter<chapterCounter>/.
    // Only Chapter 1 has content today; when multi-chapter progression lands, persist this in GameData.
    public static int chapterCounter = 1;
    public static int dayCounter = 0;
    public static stage stageCounter;
    public static int sceneCounter = 0;
    public static bool startedInClient = true;
    public static List<string> followers = new List<string>();
    public static Flags flags = new Flags();
    public static GameData gameData = new GameData("", 0, 0, 0, 0);
    public static void LoadScene(string sceneName)
    {
        SaveGameData();
        startedInClient = false;
        Input.ResetInputAxes();
        SceneManager.LoadScene(sceneName);
    }

    public static void ResetGameData()
    {
        dayCounter = 0;
        stageCounter = 0;
        sceneCounter = 0;
        RoomData.toEntranceNum = 0;
        RoomData.toEntranceId = "";
    }

    public static void SaveGameData()
    {
        gameData.dayCounter = dayCounter;
        gameData.stageCounter = stageCounter;
        gameData.sceneCounter = sceneCounter;
        gameData.room = SceneManager.GetActiveScene().name;
        gameData.entranceNum = RoomData.toEntranceNum;
        gameData.entranceId = RoomData.toEntranceId;
        string data = JsonUtility.ToJson(gameData);
        File.WriteAllText(Application.persistentDataPath + "/gameData.json", data);
    }

    public static void LoadGameData()
    {
        string data = File.ReadAllText(Application.persistentDataPath + "/gameData.json");
        GameData loadedGameData = JsonUtility.FromJson<GameData>(data);
        Debug.Log(loadedGameData);
        gameData.room = loadedGameData.room;
        dayCounter = loadedGameData.dayCounter;
        stageCounter = loadedGameData.stageCounter;
        sceneCounter = loadedGameData.sceneCounter;
        RoomData.toEntranceNum = loadedGameData.entranceNum;
        RoomData.toEntranceId = loadedGameData.entranceId;
    }

    public static class RoomData
    {
        // Which Entrance marker (by id) to spawn at in the next scene. Preferred over toEntranceNum;
        // blank means "use the legacy numeric index". Set by the RoomExit door that was used.
        public static string toEntranceId = "";

        public static int toEntranceNum = 0;

        public static direction entranceDirection = direction.left;

        public static string startingCutscene;
    }

    public static IEnumerator FadeIn(SpriteRenderer image, float fadeSpeed)
    {
        float fadeAmount;
        while (image.color.a < 1)
        {
            fadeAmount = image.color.a + (fadeSpeed * 0.005f);
            image.color = new Color(image.color.r, image.color.g, image.color.b, fadeAmount);
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);
    }

    public static IEnumerator FadeOut(SpriteRenderer image, float fadeSpeed)
    {
        float fadeAmount;
        while (image.color.a > 0)
        {
            fadeAmount = image.color.a - (fadeSpeed * 0.005f);
            image.color = new Color(image.color.r, image.color.g, image.color.b, fadeAmount);
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);
    }

    public static IEnumerator FlickerRed(SpriteRenderer image)
    {
        image.color = new Color(1, 0.5f, 0.5f);
        yield return new WaitForSeconds(0.1f);
        image.color = new Color(1, 0, 0);
        yield return new WaitForSeconds(0.1f);
        image.color = new Color(1, 0.5f, 0.5f);
        yield return new WaitForSeconds(0.1f);
        image.color = new Color(1, 1, 1);
    }

    public static Vector2 ParseVector(string s)
    {
        return new Vector2(float.Parse(s.Split(',')[0]), float.Parse(s.Split(',')[1]));
    }
}

[System.Serializable]
public class GameData
{
    public string room = "";
    public int dayCounter = 0;
    public stage stageCounter = 0;
    public int entranceNum = 0;
    public string entranceId = "";
    public int sceneCounter= 0;

    public GameData(string room, int day, int stage, int entrance, int scene)
    {
        this.room = room;
        this.stageCounter = (stage)stage;
        dayCounter = day; 
        entranceNum = entrance;
        sceneCounter = scene;
    }
}


