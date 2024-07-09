using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchingBagController : EnemyController
{
    MoveableObject move;
    // Start is called before the first frame update
    [Header("Bounds")]
    public float minX;
    public float minY;
    public float maxX;
    public float maxY;

    [SerializeField] string arriveAnimationX;
    [SerializeField] string arriveAnimationY;

    IEnumerator moveCoroutine;
    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        move = GetComponent<MoveableObject>();
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (state == combatState.arrivedX)
        {
            ChangeAnimationState(arriveAnimationX, 0);
        }
        else if (state == combatState.arrivedY)
        {
            ChangeAnimationState(arriveAnimationY, 0);
        }
        base.Update();
    }

    internal override IEnumerator StartFight()
    {
        StartCoroutine(base.StartFight());
        yield return new WaitForSeconds(3);
        player.GetComponent<CutsceneMoveableObject>().enabled = false;
        player.Heal(100);
        Heal(100);
        StopCoroutine(moveCoroutine);
        StartCoroutine(move.Move(new Vector2(21.5f, 6), 2));
        Vector2 iPos = rb.position;
        while (!MoveableObject.ArrivedToPoint(iPos, rb.position, new Vector2(21.5f, 6)))
        {
            yield return new WaitForEndOfFrame();
        }
        enemyHealthBar.SetActive(true);
        player.playerHealthBar.SetActive(true);
        onDeathCutscene = "PunchingBagFight.3";
        yield return new WaitForSeconds(3);
        player.EnableInput();
        StartMusic();
        moveCoroutine = Move();
        StartCoroutine(moveCoroutine);
    }

    protected override void HandleCollision(GameObject collisionObject)
    {
        base.HandleCollision(collisionObject);
    }

    IEnumerator Move()
    {
        while (currentHealth > 0)
        {
            Vector2 newSpot = new Vector2(Mathf.Floor(Random.Range(minX, maxX)) + 0.5f, Mathf.Floor(Random.Range(minY, maxY)));
            Vector2 oldSpot = rb.position;
            StartCoroutine(move.Move(newSpot, moveSpeed));
            while (!MoveableObject.ArrivedToPoint(oldSpot, rb.position, newSpot)) {
                yield return new WaitForSeconds(0.1f);
            }
            if(Mathf.Abs(newSpot.y - oldSpot.y)/Mathf.Abs(newSpot.x - oldSpot.x) > 1)
            {
                ChangeAnimationState(arriveAnimationY, 0);
            }
            else
            {
                ChangeAnimationState(arriveAnimationX, 0);
            }
            yield return new WaitForSeconds(1 + currentHealth/maxHealth);
        }
    }

    internal override void Die()
    {
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }
        moveCoroutine = move.Move(new Vector2(rb.position.x, rb.position.y + 20), moveSpeed);
        StartCoroutine(moveCoroutine);
        base.Die();
    }
}
