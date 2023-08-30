using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

public static class GameManager
{
    public static int gameCounter = 0;
    public static int sceneCounter = 0;
    public static bool debugCounterToggle = true;
    public static void LoadScene(string sceneName)
    {
        //Image canvasImage = GameObject.Find("Canvas").GetComponent<Image>();
        Input.ResetInputAxes();
        SceneManager.LoadScene(sceneName);
    }

    //public static IEnumerator FadeIn(int fadeSpeed = 3)
    //{
    //    Image canvasImage = GameObject.Find("Canvas").GetComponentInChildren<Image>();
    //    canvasImage.color = new Color(canvasImage.color.r, canvasImage.color.g, canvasImage.color.b, 1);
    //    float fadeAmount;
    //    while (canvasImage.color.a > 0)
    //    {
    //        fadeAmount = canvasImage.color.a - (fadeSpeed * Time.deltaTime);
    //        canvasImage.color = new Color(canvasImage.color.r, canvasImage.color.g, canvasImage.color.b, fadeAmount);
    //        yield return null;
    //    }
    //}

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
            fadeAmount = image.color.a + (fadeSpeed * Time.deltaTime);
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
            fadeAmount = image.color.a - (fadeSpeed * Time.deltaTime);
            image.color = new Color(image.color.r, image.color.g, image.color.b, fadeAmount);
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);
    }
}
