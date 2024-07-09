using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneMoveableObject : MoveableObject
{
    Collider2D col;
    private new void Awake()
    {
        base.Awake();
        col = GetComponent<Collider2D>();
    }

    public IEnumerator CutsceneMove(Vector2 coords, float speed, bool isContinue, Ref<bool> isDone, direction d)
    {
        if (col)
        {
            col.enabled = false;
        }
        Vector2 initialPos = rb.position;
        if (isContinue)
        {
            isDone.Value = true;
        }
        StartCoroutine(Move(coords, speed));
        while(!ArrivedToPoint(initialPos, rb.position, coords))
        {
            yield return new WaitForFixedUpdate();
        }
        if (!isContinue)
        {
            isDone.Value = true;
        }
        if (col)
        {
            col.enabled = true;
        }
        if(GetComponent<AnimatableObject>() != null)
        {
            GetComponent<AnimatableObject>().StopAnimation();
        }
        if (GetComponent<Faces>() != null) {
            GetComponent<Faces>().Face(d);
        }
    }

    public IEnumerator CutsceneRotate(bool clockwise, float deg, float speed, bool isContinue, Ref<bool> isDone)
    {
        if (isContinue)
        {
            isDone.Value = true;
        }
        StartCoroutine(Rotate(clockwise, deg, speed));
        while (transform.rotation.z < deg)
        {
            yield return new WaitForFixedUpdate();
        }
        if (!isContinue)
        {
            isDone.Value = true;
        }
    }
}
