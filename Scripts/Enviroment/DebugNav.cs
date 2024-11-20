using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugNav : MonoBehaviour
{
    public void ReadStringInput(string input)
    {
        Debug.Log(input);
        try
        {
            GameManager.dayCounter = int.Parse(input.Split(',')[1]);
            GameManager.stageCounter = (stage)int.Parse(input.Split(',')[2]);
            GameManager.sceneCounter = int.Parse(input.Split(',')[3]);
            GameManager.LoadScene(input.Split(',')[0]);
        }
        catch
        {
            Debug.Log("Error: Debug navigation format");
        }
    }
}
