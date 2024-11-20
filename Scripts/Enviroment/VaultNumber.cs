using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VaultNumber : MonoBehaviour
{
    public int val = 0;
    public TextMeshPro text;
    bool enabled;
    private void Start()
    {
        text = GetComponentInChildren<TextMeshPro>();
    }
    public void Increase()
    {
        if(val != 9)
        {
            val++;
        }
        else
        {
            val = 0;
        }
        text.text = val.ToString();
    }

    public void Decrease()
    {
        if (val != 0)
        {
            val--;
        }
        else
        {
            val = 9;
        }
        text.text = val.ToString();
    }

    public void ChangeColor(Color c)
    {
        text.color = c;
    }
}
