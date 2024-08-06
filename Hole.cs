using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hole : EnemyController
{
    public GameObject quarter;
    Coroutine currentAction;
    int shootNum = 10;
    int shootSpeed = 7;
    float shootWait = 0.2f;
    bool actionDone;
    public ArcJump arcJump;
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

    IEnumerator Combat()
    {
        while(currentHealth > 0)
        {
            actionDone = false;
            int nextMove = Random.Range(3, 4);
            switch (nextMove)
            {
                case 0:
                   
                    break;
                case 1:
                    currentAction = StartCoroutine(Jump());
                    break;
                case 2:
                    currentAction = StartCoroutine(Punch());
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

        yield return new WaitForSeconds(5);
    }

    IEnumerator Punch()
    {
        ChangeAnimationState("Hole Punch", 0);
        yield return new WaitForSeconds(5);
    }

    IEnumerator Fade()
    {
        GameObject moneyBag = GameObject.Find("Money Bag");
        ChangeAnimationState("Hole Jump", 0);
        StartCoroutine(move.Move(moneyBag.transform.position + new Vector3(0, -0.3f, 0), 5));
        yield return new WaitForSeconds(1);
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
