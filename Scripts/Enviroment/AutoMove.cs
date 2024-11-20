using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoMove : MonoBehaviour
{
    public bool isMoving = true;
    public int pause = 0;
    public int speed;
    public List<Vector2> moveCoords;
    MoveableObject move;
    // Start is called before the first frame update
    void Start()
    {
        move = GetComponent<MoveableObject>();
        StartCoroutine(StartMoveSet());
    }

    IEnumerator StartMoveSet()
    {
        int i = 0;
        Vector2 initialPos = transform.position;
        while (isMoving)
        {
            GetComponent<SpriteRenderer>().flipX = i % 2 == 1;
            StartCoroutine(move.Move(moveCoords[i], speed));
            while (!MoveableObject.ArrivedToPoint(initialPos, move.rb.position, moveCoords[i])) {
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForEndOfFrame();
            i++;
            if(i == moveCoords.Count)
            {
                i = 0;
            }
        }
    }

    public void StopMoving()
    {
        isMoving = false;
    }

    public void StartMoving()
    {
        isMoving = true;
    }
}
