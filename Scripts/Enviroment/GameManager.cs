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
}
public enum direction { right, left, up, down, upRight, upLeft, downRight, downLeft };

public enum stage { pre, pr, gym, lunch, psych, sci, post, dream}

public class Flags
{
   public bool hasPlanner = true;
}

public static class GameManager
{
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
    }

    public static void SaveGameData()
    {
        gameData.dayCounter = dayCounter;
        gameData.stageCounter = stageCounter;
        gameData.sceneCounter = sceneCounter;
        gameData.room = SceneManager.GetActiveScene().name;
        gameData.entranceNum = RoomData.toEntranceNum;
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
    }

    public static class RoomData
    {
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


