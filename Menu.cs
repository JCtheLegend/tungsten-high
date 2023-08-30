using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public Vector2[] selectorPos;
    public enum MenuState { pressZ, newGameSelect, continueSelect };

    public GameObject pressZ;
    public GameObject newGameSelect;
    public GameObject continueSelect;
    public GameObject selector;

    public GameObject chalk;
    public GameObject eraser;

    public MenuState state;

    public bool inputDisabled = false;

    // Update is called once per frame
    void Update()
    {
        if (!inputDisabled)
        {
            if (state == MenuState.pressZ)
            {
                if (Input.GetKeyDown(KeyCode.Z))
                {
                    ProgressMenuPart1();
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.Z))
                {
                    ProgressMenuPart2();
                }
                if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
                {
                    ChangeSelection();
                }
            }
        }
    }

    void ChangeSelection()
    {
        if(state == MenuState.continueSelect)
        {
            state = MenuState.newGameSelect;
            selector.transform.position = selectorPos[0];
        }
        else
        {
            state = MenuState.continueSelect;
            selector.transform.position = selectorPos[1];
        }
    }

    void ProgressMenuPart1()
    {
        inputDisabled = true;
        pressZ.SetActive(false);
        newGameSelect.SetActive(true);
        continueSelect.SetActive(true);
        selector.SetActive(true);
        selector.transform.position = selectorPos[0];
        state = MenuState.newGameSelect;
        inputDisabled = false;
    }

    void ProgressMenuPart2()
    {
        pressZ.SetActive(false);
        newGameSelect.SetActive(false);
        continueSelect.SetActive(false);
        selector.SetActive(false);
        inputDisabled = true;
    }
}
