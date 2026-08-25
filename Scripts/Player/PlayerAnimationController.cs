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

    private string currentState = "";
    private int currentLayer = 0;

    public string costume = "";
    public bool inCutscene = false;

    const int playerBaseLayer = 0;
    const string walkForward = "walk_forward";
    const string walkSide = "walk_side";
    const string walkBack = "walk_back";
    const string defaultForward = "default_forward";
    const string defaultSide = "default_side";
    const string defaultBack = "default_back";

    // Costume is a prefix on the animation-state name: "" (default), "pjs" -> "pj_",
    // "lunch" -> "lunch_". A costume not in this map plays nothing (preserves legacy behavior).
    static readonly Dictionary<string, string> costumePrefixes = new Dictionary<string, string>
    {
        { "", "" },
        { "pjs", "pj_" },
        { "lunch", "lunch_" },
    };

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    // Plays baseState with the current costume's prefix. No-op for an unknown costume.
    void ChangeCostumeState(string baseState, int layer)
    {
        if (costumePrefixes.TryGetValue(costume, out string prefix))
        {
            ChangeAnimationState(prefix + baseState, layer);
        }
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
                ChangeCostumeState(walkSide, playerBaseLayer);
            }
        }
        else if (player.input.dirInput.x < 0 && player.movement.rb.velocity.x != 0)
        {
            if (player.input.facing != direction.up && player.input.facing != direction.down)
            {
                transform.eulerAngles = new Vector3(0, 180, 0);
                ChangeCostumeState(walkSide, playerBaseLayer);
            }
        }
        else if (player.input.dirInput.y < 0 && player.movement.rb.velocity.y != 0)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            ChangeCostumeState(walkForward, playerBaseLayer);
        }
        else if (player.input.dirInput.y > 0 && player.movement.rb.velocity.y != 0)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            ChangeCostumeState(walkBack, playerBaseLayer);
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
                ChangeCostumeState(defaultBack, playerBaseLayer);
                break;
            case direction.left:
            case direction.right:
                ChangeCostumeState(defaultSide, playerBaseLayer);
                break;
            case direction.down:
            default:
                ChangeCostumeState(defaultForward, playerBaseLayer);
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
                    ChangeCostumeState(walkForward, playerBaseLayer);
                    break;
                case direction.up:
                    ChangeCostumeState(walkBack, playerBaseLayer);
                    break;
                case direction.right:
                    ChangeCostumeState(walkSide, playerBaseLayer);
                    break;
                case direction.left:
                    transform.eulerAngles = new Vector3(0, 180, 0);
                    ChangeCostumeState(walkSide, playerBaseLayer);
                    break;
            }
        }
    }
}
