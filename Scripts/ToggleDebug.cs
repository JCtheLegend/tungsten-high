using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleDebug : MonoBehaviour
{
    public GameObject debugInput;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (debugInput.activeInHierarchy)
            {
                DisableInput();
            }
            else
            {
                EnableInput();
            }
        }
    }

    public void DisableInput()
    {
        debugInput.SetActive(false);
    }

    public void EnableInput()
    {
        debugInput.SetActive(true);
    }
}
