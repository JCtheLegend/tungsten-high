using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableBlock : MonoBehaviour
{
    MoveableObject move;
    SpriteRenderer s;
    RaycastHit2D upRay;
    RaycastHit2D downRay;
    RaycastHit2D leftRay;
    RaycastHit2D rightRay;
    int rayLength = 1;
    [SerializeField] Sprite notMoveable;
    // Start is called before the first frame update
    void Start()
    {
        move = GetComponent<MoveableObject>();
        s = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        int layerMask = LayerMask.GetMask("Wall");
        downRay = Physics2D.Raycast(transform.position, Vector2.down, rayLength, layerMask);
        upRay = Physics2D.Raycast(transform.position, Vector2.up, rayLength, layerMask);
        rightRay = Physics2D.Raycast(transform.position, Vector2.right, rayLength, layerMask);
        leftRay = Physics2D.Raycast(transform.position, Vector2.left, rayLength, layerMask);
    }

    public void Move(direction dir)
    {
        if(s.sprite == notMoveable)
        {
            return;
        }
        switch (dir)
        {
            case direction.down:
                if (!downRay)
                {
                    StartCoroutine(move.Move(new Vector2(transform.position.x, transform.position.y - 2), 3));
                    s.sprite = notMoveable;
                }
                break;
            case direction.up:
                if (!upRay)
                {
                    StartCoroutine(move.Move(new Vector2(transform.position.x, transform.position.y + 2), 3));
                    s.sprite = notMoveable;
                }
                break;
            case direction.right:
                if (!rightRay)
                {
                    StartCoroutine(move.Move(new Vector2(transform.position.x + 2, transform.position.y), 3));
                    s.sprite = notMoveable;
                }
                break;
            case direction.left:
                if (!leftRay)
                {
                    StartCoroutine(move.Move(new Vector2(transform.position.x - 2, transform.position.y), 3));
                    s.sprite = notMoveable;
                }
                break;
        }
    }
}
