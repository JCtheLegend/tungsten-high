using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hole : EnemyController
{
    public GameObject quarter;
    int shootNum = 10;
    int shootSpeed = 7;
    float shootWait = 0.2f;
    bool actionDone;
    public ArcJump arcJump;
    bool fellDown = false;
    internal override IEnumerator StartFight()
    {
        StartCoroutine(base.StartFight());
        player.GetComponent<CutsceneMoveableObject>().enabled = false;
        enemyHealthBar.SetActive(true);
        player.playerHealthBar.SetActive(true);
        yield return new WaitForSeconds(0);
        StartMusic();
        player.EnableInput();
        StartCoroutine(Combat());
    }

    protected override void Start()
    {
        base.Start();
        arcJump = GetComponent<ArcJump>();
        arcJump.speed = 1;
    }

    IEnumerator Block()
    {
        ChangeAnimationState("Hole Block", 0);
        StartCoroutine(move.Move(new Vector2(this.transform.position.x, Mathf.Min(player.arenaMaxHeight, transform.position.y + 2)), 3));
        yield return new WaitForSeconds(2);
        ChangeAnimationState("Hole Default", 0);
        actionDone = true;
    }

    IEnumerator Fall()
    {
        Debug.Log("Fall");
        fellDown = true;
        ChangeAnimationState("Hole Fall", 0);
        Vector2 midPoint = new Vector2(transform.position.x, transform.position.y + 1);
        arcJump.point[0] = (transform.position);
        arcJump.point[1] = (midPoint);
        arcJump.point[2] = (transform.position);
        arcJump.jumping = true;
        yield return new WaitForSeconds(5);
        actionDone = true;
        fellDown = false;
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name == "Slip Quarter")
        {
            StopCoroutine(currentAction);
            arcJump.jumping = false;
            StartCoroutine(Fall());
        }
        if (fellDown)
        {
            base.OnTriggerEnter2D(collision);
        }
        else
        {
            StopCoroutine(currentAction);
            arcJump.jumping = false;
            StartCoroutine(Block());
        }
    }

    IEnumerator Combat()
    {
        while(currentHealth > 0)
        {
            actionDone = false;
            int nextMove = Random.Range(1, 2);
            switch (nextMove)
            {
                case 0:                
                    break;
                case 1:
                    currentAction = StartCoroutine(Jump());
                    break;
                case 2:
                    currentAction = StartCoroutine(AttackPlayer());
                    break;
                case 3:
                    currentAction = StartCoroutine(Fade());
                    break;
            }
            while (actionDone == false)
            {
                yield return new WaitForEndOfFrame();
            }

        }
    }

    IEnumerator Jump()
    {
        ChangeAnimationState("Hole Jump", 0);
        Vector2 jumpPoint = new Vector2(Random.Range(player.arenaMinWidth, player.arenaMaxWidth), Random.Range(player.arenaMinHeight, player.arenaMaxHeight));
        Vector2 midPoint = new Vector2((jumpPoint.x - transform.position.x) / 2, 4);
        arcJump.point[0] = (transform.position);
        arcJump.point[1] = (midPoint);
        arcJump.point[2] = (jumpPoint);
        arcJump.jumping = true;
        ChangeAnimationState("Default", 0);
        yield return new WaitForSeconds(1);
        arcJump.ResetJump();
        actionDone = true;
    }

    IEnumerator AttackPlayer()
    {
        ChangeAnimationState("Hole Jump", 0);
        Vector2 jumpPoint = player.transform.position + new Vector3(0, 1.25f);
        Vector2 midPoint = new Vector2((jumpPoint.x - transform.position.x) / 2, transform.position.y + 1);
        arcJump.point[0] = (transform.position);
        arcJump.point[1] = (midPoint);
        arcJump.point[2] = (jumpPoint);
        arcJump.jumping = true;
        ChangeAnimationState("Default", 0);
        yield return new WaitForSeconds(1);
        ChangeAnimationState("Hole Punch", 0);
        yield return new WaitForSeconds(1);
        arcJump.ResetJump();
        actionDone = true;
    }

    IEnumerator Fade()
    {
        GameObject moneyBag = GameObject.Find("Money Bag");
        ChangeAnimationState("Hole Jump", 0);
        StartCoroutine(move.Move(moneyBag.transform.position + new Vector3(0, -0.3f, 0), 5));
        ChangeAnimationState("Default", 0);
        yield return new WaitForSeconds(1);
        ChangeAnimationState("Hole Fade", 0);
        for(int i = 0; i < shootNum; i++)
        {
            GameObject newQuarter = Instantiate(quarter, moneyBag.transform.position, Quaternion.identity, this.transform);
            Vector3 addition = (player.transform.position - moneyBag.transform.position).normalized * 100;
            Vector2 shootPos = player.transform.position + addition;
            StartCoroutine(newQuarter.GetComponent<MoveableObject>().Move(shootPos, shootSpeed));
            yield return new WaitForSeconds(shootWait);
        }
        yield return new WaitForSeconds(1);
        ChangeAnimationState("Hole X", 0);
        yield return new WaitForSeconds(2);
        actionDone = true;
    }
}
