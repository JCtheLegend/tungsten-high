using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] PlayerMasterController player;
    [SerializeField] internal Animator animator;

    [SerializeField] Sprite forwardSprite;
    [SerializeField] Sprite sideSprite;
    [SerializeField] Sprite backSprite;
    SpriteRenderer sprite;

    private string currentState;
    private int currentLayer;

    public bool inPjs = false;
    bool inCutscene = false;

    const int playerBaseLayer = 0;
    const string walkForward = "walk_forward";
    const string walkSide = "walk_side";
    const string walkBack = "walk_back";
    const string defaultForward = "default_forward";
    const string defaultSide = "default_side";
    const string defaultBack = "default_back";
    const string pjWalkForward = "pj_walk_forward";
    const string pjWalkBack = "pj_walk_back";
    const string pjWalkSide = "pj_walk_side";
    const string pjDefaultForward = "pj_default_forward";
    const string pjDefaultBack = "pj_default_back";
    const string pjDefaultSide = "pj_default_side";

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (inCutscene)
        {
            return;
        }
        sprite.sortingOrder = 32767 - (int)Mathf.Ceil((transform.position.y + 50) * 100);
        if (player.input.dirInput.x > 0 && player.movement.rb.velocity.x != 0) {            
            if (player.input.facing != direction.up && player.input.facing != direction.down)
            {
            transform.eulerAngles = new Vector3(0, 0, 0);
                if (inPjs)
                {
                    ChangeAnimationState(pjWalkSide, playerBaseLayer);
                }
                else
                {
                    ChangeAnimationState(walkSide, playerBaseLayer);
                }
            }
        }
        else if (player.input.dirInput.x < 0 && player.movement.rb.velocity.x != 0)
        {  
            if (player.input.facing != direction.up && player.input.facing != direction.down)
            {
            transform.eulerAngles = new Vector3(0, 180, 0);
            if (inPjs)
            {
                ChangeAnimationState(pjWalkSide, playerBaseLayer);
            }
            else
            {
                ChangeAnimationState(walkSide, playerBaseLayer);
            }
            }
        }
        else if (player.input.dirInput.y < 0 && player.movement.rb.velocity.y != 0)
        {
        transform.eulerAngles = new Vector3(0, 0, 0);
        if (inPjs)
        {
            ChangeAnimationState(pjWalkForward, playerBaseLayer);
        }
        else
        {
            ChangeAnimationState(walkForward, playerBaseLayer);
        }
        }
        else if (player.input.dirInput.y > 0 && player.movement.rb.velocity.y != 0)
        {
        transform.eulerAngles = new Vector3(0, 0, 0);
            if (inPjs)
            {
                ChangeAnimationState(pjWalkBack, playerBaseLayer);
            }
            else
            {
                ChangeAnimationState(walkBack, playerBaseLayer);
            }
        }
        else
        {
            switch (player.input.facing)
            {
                case direction.up:
                    if (inPjs)
                    {
                        ChangeAnimationState(pjDefaultBack, playerBaseLayer);
                    }
                    else
                    {
                        ChangeAnimationState(defaultBack, playerBaseLayer);
                    }
                    break;
                case direction.left:
                case direction.right:
                    if (inPjs)
                    {
                        ChangeAnimationState(pjDefaultSide, playerBaseLayer);
                    }
                    else
                    {
                        ChangeAnimationState(defaultSide, playerBaseLayer);
                    }
                    break;
                case direction.down:
                default:
                    if (inPjs)
                    {
                        ChangeAnimationState(pjDefaultForward, playerBaseLayer);
                    }
                    else
                    {
                        ChangeAnimationState(defaultForward, playerBaseLayer);
                    }
                    break;
            }
        }
    }

    internal void ChangeAnimationState(string newState, int layer)
    {
        if (currentState == newState && currentLayer == layer) return;
        currentState = newState;
        currentLayer = layer;
        animator.Play(newState, layer);
    }

    public IEnumerator CutsceneAnimate(string animation, bool isContinue)
    {
        inCutscene = true;
        CutsceneManager cut = GameObject.Find("Cutscene Manager").GetComponent<CutsceneManager>();
        animator.Play(animation);
        if (isContinue)
        {
            cut.setAnimateDone();
            inCutscene = false;
        }
        else
        {
            yield return new WaitForSeconds(animator.runtimeAnimatorController.animationClips[0].length);
            cut.setAnimateDone();
            inCutscene = false;
        }
    }
}
