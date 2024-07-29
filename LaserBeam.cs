using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeam : MonoBehaviour
{
    public string type;

    public SpriteRenderer[] inner;
    public SpriteRenderer[] middle;
    public SpriteRenderer[] outer;

    public Color innerColor;
    public Color middleColor;
    public Color outerColor;

    public direction dir;

    private void Start()
    {
        foreach (SpriteRenderer s in inner)
        {
            s.color = innerColor;
        }
        foreach (SpriteRenderer s in middle)
        {
            s.color = middleColor;
        }
        foreach (SpriteRenderer s in outer)
        {
            s.color = outerColor;
        }
    }
}
