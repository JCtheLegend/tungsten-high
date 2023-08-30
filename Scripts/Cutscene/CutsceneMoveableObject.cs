using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneMoveableObject : MoveableObject
{
    CutsceneManager cut;

    private new void Awake()
    {
        base.Awake();
        cut = GameObject.Find("Cutscene Manager").GetComponent<CutsceneManager>();
    }

    public IEnumerator CutsceneMove(Vector2 coords, float speed, bool isContinue)
    {
        Vector2 initialPos = rb.position;
        if (isContinue)
        {
            cut.setMovementDone();
        }
        StartCoroutine(Move(coords, speed));
        while(!ArrivedToPoint(initialPos, rb.position, coords))
        {
            yield return new WaitForFixedUpdate();
        }
        if (!isContinue)
        {
            cut.setMovementDone();
        }
    }

    public IEnumerator CutsceneRotate(bool clockwise, float deg, float speed, bool isContinue)
    {
        if (isContinue)
        {
            cut.setMovementDone();
        }
        StartCoroutine(Rotate(clockwise, deg, speed));
        while (transform.rotation.z < deg)
        {
            yield return new WaitForFixedUpdate();
        }
        if (!isContinue)
        {
            cut.setMovementDone();
        }
    }
}
