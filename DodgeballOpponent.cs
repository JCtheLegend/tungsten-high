using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeballOpponent : MonoBehaviour
{
    public float minHeight;
    public float maxHeight;
    public float minWidth;
    public float maxWidth;

    public float walkSpeed;
    public float throwSpeed;

    public float luck;
    public float aggressiveness;
    public float defensiveness;
    public float accurracy;

    public string walkAnim;
    public string throwAnim;
    public string holdAnim;

    public SpriteRenderer[] Lives;

    public int lives = 3;

    public GameObject hitAlert;
    public GameObject caughtAlert;

    public Vector2 holdPosition;

    public Animator legsAnim;

    MoveableObject m;
    Animator torsoAnim;
    Rigidbody2D rb;

    GameObject ball;
    GameObject target;

    Coroutine actionCoroutine;
    Coroutine moveCoroutine;

    public bool tutorial;

    enum state { noBall, foundBall, hasBall }

    state currentState = state.noBall;

    public DodgeballMatchController matchController;

    // Start is called before the first frame update
    void Start()
    {
        torsoAnim = GetComponent<Animator>();
        m = GetComponent<MoveableObject>();
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.Find("Grid Player");
        StartCoroutine(Main());
    }

    IEnumerator Main()
    {
        while (true)
        {
            if (currentState == state.noBall)
            {
                actionCoroutine = StartCoroutine(FindBall());
                while (currentState == state.noBall)
                {
                    yield return new WaitForEndOfFrame();
                }
                if (actionCoroutine != null)
                {
                    StopCoroutine(actionCoroutine);
                }
            }
            else if(currentState == state.foundBall)
            {
                if (moveCoroutine != null)
                {
                    StopCoroutine(moveCoroutine);
                }
                actionCoroutine = StartCoroutine(GoToBall());
                while (currentState == state.foundBall)
                {
                    yield return new WaitForEndOfFrame();
                }
                if (actionCoroutine != null)
                {
                    StopCoroutine(actionCoroutine);
                }
            }
            else
            {
                torsoAnim.Play(holdAnim);
                StartCoroutine(ThrowBall());
                while (currentState == state.hasBall)
                {
                    yield return new WaitForEndOfFrame();
                }
            }
            yield return new WaitForEndOfFrame();
        }
    }

    bool OnSide(GameObject t)
    {
        return t.transform.position.x < maxWidth
                    && t.transform.position.x > minWidth
                    && t.transform.position.y > minHeight
                    && t.transform.position.y < maxHeight;
    }

    IEnumerator FindBall()
    {
        Debug.Log("Finding Ball");
        while (currentState == state.noBall)
        {
            Dodgeball[] balls = FindObjectsOfType<Dodgeball>();
            Debug.Log(balls.Length);
            foreach (Dodgeball b in balls)
            {
                if (OnSide(b.gameObject) && !b.GetComponent<ArcJump>().jumping && b.gameObject.name != "Ally Dodgeball")
                {                   
                    ball = b.gameObject;
                    currentState = state.foundBall;
                }
            }
            if(ball == null)
            {
                StartCoroutine(WalkAround());
                while (moveCoroutine != null)
                {
                    yield return new WaitForEndOfFrame();
                }
            }
            yield return new WaitForSeconds(1);
        }
    }

    IEnumerator WalkAround()
    {
        Vector2 initialPos = transform.position;
        Vector2 newPos = new Vector2(Random.Range(minWidth, maxWidth), Random.Range(minHeight, maxHeight));   
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }
        moveCoroutine = StartCoroutine(m.Move(newPos, walkSpeed));
        legsAnim.Play(walkAnim);
        while (!MoveableObject.ArrivedToPoint(initialPos, transform.position, newPos) && moveCoroutine != null)
        {
            yield return new WaitForEndOfFrame();
        }
        legsAnim.Play("default_state");
        moveCoroutine = null;
    }

    IEnumerator GoToBall()
    {
        Debug.Log("Going to Ball");
        while (currentState != state.hasBall)
        {
            if (moveCoroutine != null)
            {
                StopCoroutine(moveCoroutine);
            }
            Vector2 initialPos = transform.position;
            moveCoroutine = StartCoroutine(m.Move(ball.transform.position, walkSpeed));
            while (!MoveableObject.ArrivedToPoint(initialPos, rb.position, ball.transform.position))
            {
                yield return new WaitForEndOfFrame();
            }
            if (!OnSide(ball)) {
                currentState = state.noBall;
                ball = null;
            }
            else
            {
                currentState = state.hasBall;
                ball.transform.parent = transform;
                ball.transform.localPosition = holdPosition;
            }
            yield return new WaitForEndOfFrame();
        }
 
    }

    IEnumerator ThrowBall()
    {
        Debug.Log("Throwing Ball");
        StartCoroutine(WalkAround());
        while (moveCoroutine != null)
        {
            yield return new WaitForEndOfFrame();
        }
        Vector2 pos = target.transform.position - ball.transform.position;
        ball.transform.parent = null;
        ball.name = "Opponent Dodgeball";
        torsoAnim.Play(throwAnim);
        yield return new WaitForSeconds(0.1f);
        ball.GetComponent<Dodgeball>().Throw(pos.normalized * throwSpeed);
        ball = null;
        yield return new WaitForSeconds(2);
        currentState = state.noBall;
    }

    void Catch(GameObject g)
    {
        ball = g;
        ball.transform.parent = this.gameObject.transform;
        currentState = state.hasBall;
    }

    void Dodge()
    {

    }

    IEnumerator Hit()
    {
        GameObject g = Instantiate(hitAlert, this.transform.position + new Vector3(1, 1, 0), Quaternion.identity, transform);
        yield return new WaitForSeconds(1);
        Destroy(g);
        lives--;
        Lives[lives].enabled = false;
        if(lives == 0)
        {
            Out();
        }
    }

    void Out()
    {
        matchController.CheckGame();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Ally Dodgeball")
        {
            Debug.Log("Hit");
            ////add chance to catch
            //if (true)
            //{
            //    Catch(collision.gameObject);
            //}
            //else if(true)
            //{
            //    Dodge();
            //}
            //add chance to dodge
            StartCoroutine(Hit());
        }
    }
}
