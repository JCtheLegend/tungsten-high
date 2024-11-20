using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableBlock : ScienceObject
{
    MoveableObject move;
  
    RaycastHit2D upRay;
    RaycastHit2D downRay;
    RaycastHit2D leftRay;
    RaycastHit2D rightRay;
    float rayLength = 0.9f;
    [SerializeField] Sprite notMoveable;
   
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        move = GetComponent<MoveableObject>();
    }

    // Update is called once per frame
    void Update()
    {
        int layerMask = LayerMask.GetMask("Wall");
        downRay = Physics2D.Raycast(transform.position + 1.1f * Vector3.down, Vector2.down, rayLength, layerMask);
        upRay = Physics2D.Raycast(transform.position + 1.1f * Vector3.up, Vector2.up, rayLength, layerMask);
        rightRay = Physics2D.Raycast(transform.position + 1.1f * Vector3.right, Vector2.right, rayLength, layerMask);
        leftRay = Physics2D.Raycast(transform.position + 1.1f * Vector3.left, Vector2.left, rayLength, layerMask);
        Debug.DrawRay(transform.position + 1.1f * Vector3.down, Vector2.down, Color.red, rayLength);
    }

    public override void ResetValues()
    {
        base.ResetValues();
    }

    public void Move(direction dir)
    {
        if(s && s.sprite == notMoveable)
        {
            return;
        }
        switch (dir)
        {
            case direction.down:
                if (!downRay)
                {
                    StartCoroutine(move.Move(new Vector2(transform.position.x, transform.position.y - 2), 3));
                    if(name == "Pushable Block")
                    {
                        s.sprite = notMoveable;
                    }
                }
                break;
            case direction.up:
                if (!upRay)
                {
                    StartCoroutine(move.Move(new Vector2(transform.position.x, transform.position.y + 2), 3));
                    if (name == "Pushable Block")
                    {
                        s.sprite = notMoveable;
                    }
                }
                break;
            case direction.right:
                if (!rightRay)
                {
                    StartCoroutine(move.Move(new Vector2(transform.position.x + 2, transform.position.y), 3));
                    if (name == "Pushable Block")
                    {
                        s.sprite = notMoveable;
                    }
                }
                break;
            case direction.left:
                if (!leftRay)
                {
                    StartCoroutine(move.Move(new Vector2(transform.position.x - 2, transform.position.y), 3));
                    if (name == "Pushable Block")
                    {
                        s.sprite = notMoveable;
                    }
                }
                break;
        }
    }
}
