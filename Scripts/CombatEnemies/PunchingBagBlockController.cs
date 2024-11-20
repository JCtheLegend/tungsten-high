using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PunchingBagBlockController : EnemyController
{
    public BoxCollider2D attackCollider;

    public SpriteRenderer wheels;
    public Sprite upWheels;
    public Sprite sideWheels;
    // Start is called before the first frame updated
    [Header("Bounds")]
    public float minX;
    public float minY;
    public float maxX;
    public float maxY;

    [SerializeField] string arriveAnimationX;
    [SerializeField] string arriveAnimationY;

    IEnumerator moveCoroutine;

    bool stunned;
    protected override void Start()
    {
        base.Start();
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
        player.GetComponent<CutsceneMoveableObject>().enabled = false;
        player.Heal(100);
        Heal(100);
        enemyHealthBar.SetActive(true);
        player.playerHealthBar.SetActive(true);
        yield return new WaitForSeconds(0);
        player.EnableInput();
        StartMusic();
        moveCoroutine = BattleRoutine();
        StartCoroutine(moveCoroutine);
    }

    protected override void HandleCollision(GameObject collisionObject)
    {
        if (stunned)
        {
            base.HandleCollision(collisionObject);
        }
        else if (collisionObject.CompareTag("Player Attack"))
        {
            StartCoroutine(Block());
        }
    }

    public IEnumerator Stunned()
    {
        StopCoroutine(currentAction);
        attackCollider.enabled = false;
        ChangeAnimationState("stunned");
        StartCoroutine(move.Move(new Vector2(this.transform.position.x, Mathf.Min(player.arenaMaxHeight, transform.position.y + 2)), 3));
        stunned = true;
        yield return new WaitForSeconds(5);
        stunned = false;
    }

    IEnumerator BattleRoutine()
    {
        while (currentHealth > 0)
        {
            actionDone = false;
            currentAction = StartCoroutine(Move());
            while (actionDone == false)
            {
                yield return new WaitForEndOfFrame();
            }
            currentAction = StartCoroutine(Punch());
            yield return new WaitForSeconds(1);
            yield return new WaitForSeconds(1 + currentHealth/maxHealth);
        }
    }

    IEnumerator Move()
    {
        //int asd = Random.Range(0, 2);
        //Vector2 newSpot = asd == 1 ? new Vector2(Mathf.Floor(Random.Range(minX, maxX)) + 0.5f, transform.position.y) : new Vector2(transform.position.x, Mathf.Floor(Random.Range(minY, maxY)));

        Vector2 oldSpot = rb.position;
        Vector2 newSpot = player.transform.position + Vector3.up * 1f;
        StartCoroutine(move.Move(newSpot, moveSpeed));

        while (!MoveableObject.ArrivedToPoint(oldSpot, rb.position, newSpot))
        {
            yield return new WaitForSeconds(0.1f);
            wheels.sprite = Mathf.Abs(rb.position.y - newSpot.y) < Mathf.Abs(rb.position.x - newSpot.x) ? sideWheels : upWheels;
        }
        //if(Mathf.Abs(newSpot.y - oldSpot.y)/Mathf.Abs(newSpot.x - oldSpot.x) > 1)
        //{
        //    ChangeAnimationState(arriveAnimationY, 0);
        //}
        //else
        //{
        //    ChangeAnimationState(arriveAnimationX, 0);
        //}
        actionDone = true;
    }

    IEnumerator Block()
    {
        StopCoroutine(currentAction);
        ChangeAnimationState("block");
        StartCoroutine(move.Move(new Vector2(this.transform.position.x, Mathf.Min(player.arenaMaxHeight, transform.position.y + 2)), 3));
        yield return new WaitForSeconds(2);
        ChangeAnimationState("Default");
        actionDone = true;
    }

    IEnumerator Punch()
    {
        if (Random.Range(0, 2) == 1)
        {
            ChangeAnimationState("right_punch");
        }
        else
        {
            ChangeAnimationState("left_punch");
        }
        yield return new WaitForSeconds(0.75f);
        attackCollider.enabled = true;
        yield return new WaitForSeconds(0.5f);
        attackCollider.enabled = false;
        ChangeAnimationState("Default");
    }

    internal override IEnumerator Die()
    {
        yield return new WaitForSeconds(0);
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }
        moveCoroutine = move.Move(new Vector2(58.5f, 13.5f), moveSpeed);
        StartCoroutine(moveCoroutine);
        StartCoroutine(base.Die());
    }
}
