using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPuzzle : MonoBehaviour
{
    public GameObject[] objects;

    public void ResetPositions()
    {
        foreach (GameObject g in objects)
        {
            if (g.GetComponent<ScienceObject>() != null)
            {
                g.GetComponent<ScienceObject>().ResetValues();
            }
        }
    }
}
