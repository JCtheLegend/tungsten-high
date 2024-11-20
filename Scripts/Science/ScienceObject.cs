using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScienceObject : MonoBehaviour
{
    protected Sprite defaultSprite;
    protected Vector2 defaultPosition;
    protected SpriteRenderer s;
    protected Quaternion defaultRotation;
    public virtual void ResetValues()
    {
        if(s != null)
        {
            s.sprite = defaultSprite;
        }
        transform.position = defaultPosition;
        transform.localRotation = defaultRotation;
    }

    public virtual void Start()
    {
        if(GetComponent<SpriteRenderer>() != null)
        {
            s = GetComponent<SpriteRenderer>();
            defaultSprite = s.sprite;
        }
        defaultPosition = transform.position;
        defaultRotation = transform.rotation;
    }
}
