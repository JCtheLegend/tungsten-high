using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Faces : MonoBehaviour
{
    public Sprite faceUp;
    public Sprite faceDown;
    public Sprite faceRight;
    public Sprite faceLeft;
    SpriteRenderer sprite;
    public direction initialFacing;
    public bool mirrorFace = true;

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        Face(initialFacing);
    }

    public void FacePlayer(direction playerFacing)
    {
        switch (playerFacing)
        {
            case direction.down:
                sprite.flipX = false;
                sprite.sprite = faceUp;
                break;
            case direction.up:
                sprite.flipX = false;
                sprite.sprite = faceDown;
                break;
            case direction.left:
                sprite.flipX = false;
                sprite.sprite = faceRight;
                break;
            case direction.right:
                if (mirrorFace)
                {
                    sprite.flipX = true;
                    sprite.sprite = faceRight;
                }
                else
                {
                    sprite.sprite = faceLeft;
                }
                break;
        }
    }

    public void Face(direction dir)
    {
        switch (dir)
        {
            case direction.down:
                sprite.flipX = false;
                sprite.sprite = faceDown;
                break;
            case direction.up:
                sprite.flipX = false;
                sprite.sprite = faceUp;
                break;
            case direction.left:
                if (mirrorFace)
                {
                    sprite.flipX = true;
                    sprite.sprite = faceRight;
                }
                else
                {
                    sprite.sprite = faceLeft;
                }
                break;
            case direction.right:
                sprite.flipX = false;
                sprite.sprite = faceRight;
                break;
        }
    }
}
