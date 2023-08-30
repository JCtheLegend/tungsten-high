using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatableObject : MonoBehaviour
{
    protected Animator anim;


    public string upMoveAnim;
    public string downMoveAnim;
    public string rightMoveAnim;
    public string leftMoveAnim;
    public string defaultUpAnim;
    public string defaultDownAnim;
    public string defaultRightAnim;
    public string defaultLeftAnim;
    public direction initialFacing;
    public bool mirrorFace;
    public bool hasIdleAnims = false;
    protected void Awake()
    {
        anim = GetComponent<Animator>();
        anim.enabled = false;
        UpdateFacing(initialFacing);
    }

    public void StopAnimation()
    {
        anim.enabled = false;
    }

    public void UpdateFacing(direction f)
    {
        if (hasIdleAnims)
        {
            anim.enabled = true;
            switch (f)
            {
                case direction.up:
                    anim.gameObject.GetComponent<SpriteRenderer>().flipX = false;
                    anim.Play(defaultUpAnim);
                    break;
                case direction.down:
                    anim.gameObject.GetComponent<SpriteRenderer>().flipX = false;
                    anim.Play(defaultDownAnim);
                    break;
                case direction.left:
                    if (mirrorFace)
                    {
                        anim.gameObject.GetComponent<SpriteRenderer>().flipX = true;
                        anim.Play(defaultLeftAnim);
                    }
                    else
                    {
                        anim.gameObject.GetComponent<SpriteRenderer>().flipX = false;
                        anim.Play(defaultLeftAnim);
                    }
                    break;
                case direction.right:
                    anim.gameObject.GetComponent<SpriteRenderer>().flipX = false;
                    anim.Play(defaultRightAnim);
                    break;
            }
        }
    }

    public void Animate(string animation, bool isContinue)
    {
        anim.enabled = true;
        anim.Play(animation);
    }

    public void AnimateMove(direction dir)
    {
        anim.enabled = true;
        switch (dir)
        {
            case direction.left:
            case direction.downLeft:
            case direction.upLeft:
                anim.Play(leftMoveAnim);
                break;
            case direction.right:
            case direction.downRight:
            case direction.upRight:
                anim.Play(rightMoveAnim);
                break;
            case direction.up:
                anim.Play(upMoveAnim);
                break;
            case direction.down:
                anim.Play(downMoveAnim);
                break;
        }
    }
}
