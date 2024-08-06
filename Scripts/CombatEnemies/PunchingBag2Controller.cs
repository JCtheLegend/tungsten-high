using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PunchingBag2Controller : EnemyController
{
    MoveableObject move;
    public Animator armsAnim;
    public BoxCollider2D armsCollider;

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
        base.HandleCollision(collisionObject);
    }

    IEnumerator BattleRoutine()
    {
        while (currentHealth > 0)
        {
            int asd = Random.Range(0, 2);
            Vector2 newSpot = asd == 1 ? new Vector2(Mathf.Floor(Random.Range(minX, maxX)) + 0.5f, transform.position.y) : new Vector2(transform.position.x, Mathf.Floor(Random.Range(minY, maxY)));
            wheels.sprite = asd == 1 ? sideWheels : upWheels;
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
            StartCoroutine(Spin());
            yield return new WaitForSeconds(1);
            yield return new WaitForSeconds(1 + currentHealth/maxHealth);
        }
    }

    IEnumerator Spin()
    {
        armsAnim.Play("arms_swing");
        armsCollider.enabled = true;
        yield return new WaitForSeconds(0.3f);
        armsCollider.enabled = false;
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
