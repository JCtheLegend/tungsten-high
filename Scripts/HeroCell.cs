using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class HeroCell : MonoBehaviour
{

    internal Animator animator;
    private string currentState;
    private int currentLayer;

    [SerializeField] CutsceneManager cut;
    [SerializeField] int moveSpeed;

    public bool inputEnabled = true;

    Rigidbody2D rb;

    [SerializeField] float rayLength;

    public enum State {Normal, Strength, Punch};

    public State state = State.Normal;

    [SerializeField] BoxCollider2D punchHitbox;

    float pushTimer = 0;

    float pushTimeLimit = 1;

    Coroutine strengthTimer;

    public ResetPuzzle currentPuzzle;

    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        pushTimer = pushTimeLimit;
    }

    // Update is called once per frame
    void Update()
    {
        int layerMask = LayerMask.GetMask("Pushable Block");
        RaycastHit2D downRay = Physics2D.Raycast(transform.position, Vector2.down, rayLength, layerMask);
        RaycastHit2D upRay = Physics2D.Raycast(transform.position, Vector2.up, rayLength, layerMask);
        RaycastHit2D rightRay = Physics2D.Raycast(transform.position, Vector2.right, rayLength, layerMask);
        RaycastHit2D leftRay = Physics2D.Raycast(transform.position, Vector2.left, rayLength, layerMask);
        if (downRay)
        {
            Debug.DrawRay(transform.position, Vector2.down * rayLength, Color.green);
        }
        else
        {
            Debug.DrawRay(transform.position, Vector2.down * rayLength, Color.red);
        }
        if (inputEnabled)
        {
            int v = Input.GetKey(KeyCode.UpArrow) ? 1 : Input.GetKey(KeyCode.DownArrow) ? -1 : 0;
            int h = Input.GetKey(KeyCode.RightArrow) ? 1 : Input.GetKey(KeyCode.LeftArrow) ? -1 : 0;
            Vector2 dirInput = new Vector2(h, v);
            rb.velocity = dirInput * moveSpeed;
            if (Input.GetKeyDown(KeyCode.R))
            {
                StartCoroutine(ResetPuzzle());
            }
            if(Input.GetKeyDown(KeyCode.Z) && state == State.Strength)
            {
                StartCoroutine(StrengthPunch());
            }
            if(v == 1 && upRay && state == State.Strength)
            {
                ChangeAnimationState("Hero Cell Push Up", 0);
                pushTimer -= Time.deltaTime;
                Debug.Log(pushTimer);
                if(pushTimer < 0)
                {
                    pushTimer = pushTimeLimit;
                    upRay.collider.GetComponent<PushableBlock>().Move(direction.up);
                    Debug.Log("Button Held");
                }
            }
            else if(v == -1 && downRay && state == State.Strength)
            {
                ChangeAnimationState("Hero Cell Push Down", 0);
                pushTimer -= Time.deltaTime;
                if (pushTimer < 0)
                {
                    pushTimer = pushTimeLimit;
                    downRay.collider.GetComponent<PushableBlock>().Move(direction.down);
                    Debug.Log("Button Held");
                }
            }
            else if(h == 1 && rightRay && state == State.Strength)
            {
                ChangeAnimationState("Hero Cell Push Right", 0);
                pushTimer -= Time.deltaTime;
                if (pushTimer < 0)
                {
                    pushTimer = pushTimeLimit;
                    rightRay.collider.GetComponent<PushableBlock>().Move(direction.right);
                    Debug.Log("Button Held");
                }
            }
            else if(h == -1 && leftRay && state == State.Strength)
            {
                ChangeAnimationState("Hero Cell Push Left", 0);
                pushTimer -= Time.deltaTime;
                if (pushTimer < 0)
                {
                    pushTimer = pushTimeLimit;
                    leftRay.collider.GetComponent<PushableBlock>().Move(direction.left);
                    Debug.Log("Button Held");
                }
            }
            else
            {
                pushTimer = pushTimeLimit;
                if(state == State.Normal)
                {
                    ChangeAnimationState("Hero Cell Default", 0);
                }
                else if (state == State.Strength)
                {
                    ChangeAnimationState("Hero Cell Strong", 0);
                }
                else if (state == State.Punch)
                {
                    ChangeAnimationState("Hero Cell Punch", 0);
                }
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        HandleCollision(collision.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        HandleCollision(collision.gameObject);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        HandleCollisionExit(collision.gameObject);
    }

    protected virtual void HandleCollisionExit(GameObject collisionObject)
    {
        if (collisionObject.CompareTag("Strength Laser"))
        {
            strengthTimer = StartCoroutine(StrengthTimer());
        }
    }

    protected virtual void HandleCollision(GameObject collisionObject)
    {
        if(collisionObject.CompareTag("Strength Laser"))
        {
            if (strengthTimer != null)
            {
                StopCoroutine(strengthTimer);
            }
            state = State.Strength;
        }
        if (collisionObject.CompareTag("CollideCutsceneTrigger"))
        {
            CutsceneTrigger cutsceneTrigger = collisionObject.GetComponent<CutsceneTrigger>();
            if (cutsceneTrigger)
            {
                if (cutsceneTrigger.destroy)
                {
                    Destroy(collisionObject.GetComponent<CutsceneTrigger>());
                }
                cut.BeginCutscene(cutsceneTrigger.cutsceneFileName);
            }
            }
        }

    IEnumerator StrengthTimer()
    {
        yield return new WaitForSeconds(10);
        while(state == State.Punch)
        {
            yield return new WaitForEndOfFrame();
        }
        state = State.Normal;
    }

    IEnumerator ResetPuzzle()
    {
        inputEnabled = false;
        if (strengthTimer != null)
        {
            StopCoroutine(strengthTimer);
        }
        state = State.Normal;
        StartCoroutine(GameManager.FadeIn(GameObject.Find("BlackFade").GetComponent<SpriteRenderer>(), 1));
        yield return new WaitForSeconds(1);
        currentPuzzle.ResetPositions();
        StartCoroutine(GameManager.FadeOut(GameObject.Find("BlackFade").GetComponent<SpriteRenderer>(), 1));
        yield return new WaitForSeconds(1);
        inputEnabled = true;
    }

    IEnumerator StrengthPunch()
    {
        state = State.Punch;
        punchHitbox.enabled = true;
        yield return new WaitForSeconds(animator.runtimeAnimatorController.animationClips.First(a => a.name == "Hero Cell Punch").length);
        punchHitbox.enabled = false;
        state = State.Strength;
    }

    public void DisableMovement()
    {
        rb.velocity = Vector2.zero;
        inputEnabled = false;
    }
}
