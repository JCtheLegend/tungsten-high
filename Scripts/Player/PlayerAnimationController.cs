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

    public string costume = "";
    public bool inCutscene = false;

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
    const string lunchWalkForward = "lunch_walk_forward";
    const string lunchWalkBack = "lunch_walk_back";
    const string lunchWalkSide = "lunch_walk_side";
    const string lunchDefaultForward = "lunch_default_forward";
    const string lunchDefaultBack = "lunch_default_back";
    const string lunchDefaultSide = "lunch_default_side";

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        sprite.sortingOrder = 32767 / 2 - (int)Mathf.Ceil((transform.position.y) * 10);
        if (inCutscene)
        {
            return;
        }
        if (player.input.dirInput.x > 0 && player.movement.rb.velocity.x != 0) {            
            if (player.input.facing != direction.up && player.input.facing != direction.down)
            {
            transform.eulerAngles = new Vector3(0, 0, 0);
                switch (costume)
                {
                    case "pjs":
                        ChangeAnimationState(pjWalkSide, playerBaseLayer);
                        break;
                    case "lunch":
                        ChangeAnimationState(lunchWalkSide, playerBaseLayer);
                        break;
                    case "":
                        ChangeAnimationState(walkSide, playerBaseLayer);
                        break;
                }
            }
        }
        else if (player.input.dirInput.x < 0 && player.movement.rb.velocity.x != 0)
        {  
            if (player.input.facing != direction.up && player.input.facing != direction.down)
            {
            transform.eulerAngles = new Vector3(0, 180, 0);
                switch (costume)
                {
                    case "pjs":
                        ChangeAnimationState(pjWalkSide, playerBaseLayer);
                        break;
                    case "lunch":
                        ChangeAnimationState(lunchWalkSide, playerBaseLayer);
                        break;
                    case "":
                        ChangeAnimationState(walkSide, playerBaseLayer);
                        break;
                }
            }
        }
        else if (player.input.dirInput.y < 0 && player.movement.rb.velocity.y != 0)
        {
        transform.eulerAngles = new Vector3(0, 0, 0);
            switch (costume)
            {
                case "pjs":
                    ChangeAnimationState(pjWalkForward, playerBaseLayer);
                    break;
                case "lunch":
                    ChangeAnimationState(lunchWalkForward, playerBaseLayer);
                    break;
                case "":
                    ChangeAnimationState(walkForward, playerBaseLayer);
                    break;
            }
        }
        else if (player.input.dirInput.y > 0 && player.movement.rb.velocity.y != 0)
        {
        transform.eulerAngles = new Vector3(0, 0, 0);
            switch (costume)
            {
                case "pjs":
                    ChangeAnimationState(pjWalkBack, playerBaseLayer);
                    break;
                case "lunch":
                    ChangeAnimationState(lunchWalkBack, playerBaseLayer);
                    break;
                case "":
                    ChangeAnimationState(walkBack, playerBaseLayer);
                    break;
            }
        }
        else
        {
            FaceDir();
        }
    }

    public void FaceDir()
    {
        switch (player.input.facing)
        {
            case direction.up:
                switch (costume)
                {
                    case "pjs":
                        ChangeAnimationState(pjDefaultBack, playerBaseLayer);
                        break;
                    case "lunch":
                        ChangeAnimationState(lunchDefaultBack, playerBaseLayer);
                        break;
                    case "":
                        ChangeAnimationState(defaultBack, playerBaseLayer);
                        break;
                }
                break;
            case direction.left:
            case direction.right:
                switch (costume)
                {
                    case "pjs":
                        ChangeAnimationState(pjDefaultSide, playerBaseLayer);
                        break;
                    case "lunch":
                        ChangeAnimationState(lunchDefaultSide, playerBaseLayer);
                        break;
                    case "":
                        ChangeAnimationState(defaultSide, playerBaseLayer);
                        break;
                }
                break;
            case direction.down:
            default:
                switch (costume)
                {
                    case "pjs":
                        ChangeAnimationState(pjDefaultForward, playerBaseLayer);
                        break;
                    case "lunch":
                        ChangeAnimationState(lunchDefaultForward, playerBaseLayer);
                        break;
                    case "":
                        ChangeAnimationState(defaultForward, playerBaseLayer);
                        break;
                }
                break;
        }
    }

    internal void ChangeAnimationState(string newState, int layer)
    {
        if (currentState == newState && currentLayer == layer && !player.cutscene.inCutscene) return;
        currentState = newState;
        currentLayer = layer;
        animator.Play(newState, layer);
    }

    public IEnumerator CutsceneAnimate(string animation, bool isContinue)
    {
        CutsceneManager cut = GameObject.Find("Cutscene Manager").GetComponent<CutsceneManager>();
        animator.Play(animation);
        if (isContinue)
        {
            cut.setAnimateDone();
        }
        else
        {
            yield return new WaitForSeconds(animator.runtimeAnimatorController.animationClips[0].length);
            cut.setAnimateDone();
        }
    }
    public void CutsceneAnimateMove(direction dir, bool animateOverride)
    {
        transform.eulerAngles = new Vector3(0, 0, 0);
        if (!animateOverride)
        {
            switch (dir)
            {
                case direction.down:
                    transform.eulerAngles = new Vector3(0, 0, 0);
                    switch (costume)
                    {
                        case "pjs":
                            ChangeAnimationState(pjWalkForward, playerBaseLayer);
                            break;
                        case "lunch":
                            ChangeAnimationState(lunchWalkForward, playerBaseLayer);
                            break;
                        case "":
                            ChangeAnimationState(walkForward, playerBaseLayer);
                            break;
                    }

                    break;
                case direction.up:
                    transform.eulerAngles = new Vector3(0, 0, 0);
                    switch (costume)
                    {
                        case "pjs":
                            ChangeAnimationState(pjWalkBack, playerBaseLayer);
                            break;
                        case "lunch":
                            ChangeAnimationState(lunchWalkBack, playerBaseLayer);
                            break;
                        case "":
                            ChangeAnimationState(walkBack, playerBaseLayer);
                            break;
                    }
                    break;
                case direction.right:
                    transform.eulerAngles = new Vector3(0, 0, 0);
                    switch (costume)
                    {
                        case "pjs":
                            ChangeAnimationState(pjWalkSide, playerBaseLayer);
                            break;
                        case "lunch":
                            ChangeAnimationState(lunchWalkSide, playerBaseLayer);
                            break;
                        case "":
                            ChangeAnimationState(walkSide, playerBaseLayer);
                            break;
                    }
                    break;
                case direction.left:
                    transform.eulerAngles = new Vector3(0, 180, 0);
                    switch (costume)
                    {
                        case "pjs":
                            ChangeAnimationState(pjWalkSide, playerBaseLayer);
                            break;
                        case "lunch":
                            ChangeAnimationState(lunchWalkSide, playerBaseLayer);
                            break;
                        case "":
                            ChangeAnimationState(walkSide, playerBaseLayer);
                            break;
                    }
                    break;
            }
        }
    }
}
