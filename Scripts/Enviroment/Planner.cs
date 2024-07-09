using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Planner : MonoBehaviour
{
    public PlannerSelect currentSelect;
    public TextMeshPro quarters;
    public bool isActive = false;

    public void SetActive()
    {
        isActive = true;
        UpdateInventory();
        gameObject.transform.GetChild(0).gameObject.SetActive(true);
    }

    public void SetInactive()
    {
        isActive = false;
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
    }

    public void UpdateInventory()
    {
        quarters.text = PlayerInventory.CheckInventory("Quarter").ToString();
    }
    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (currentSelect.right != null)
                {
                    currentSelect.Dehover();
                    currentSelect = currentSelect.right;
                    currentSelect.Hover();
                }
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (currentSelect.left != null)
                {
                    currentSelect.Dehover();
                    currentSelect = currentSelect.left;
                    currentSelect.Hover();
                }
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (currentSelect.up != null)
                {
                    currentSelect.Dehover();
                    currentSelect = currentSelect.up;
                    currentSelect.Hover();
                }
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (currentSelect.down != null)
                {
                    currentSelect.Dehover();
                    currentSelect = currentSelect.down;
                    currentSelect.Hover();
                }
            }
            else if (Input.GetKeyDown(KeyCode.Z))
            {
                currentSelect.Select();
            }
        }
    }
}
