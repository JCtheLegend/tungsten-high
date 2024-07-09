using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabEnemy : EnemyController
{

    [SerializeField] GameObject LeftArm;
    [SerializeField] GameObject RightArm;

    MoveableObject move;

    [SerializeField] float waitSeconds1;
    [SerializeField] float waitSeconds2;
    [SerializeField] float waitSeconds3;
    [SerializeField] int threshhold1;
    [SerializeField] int threshhold2;
    [SerializeField] int threshhold3;

    Coroutine currentAction;

    int moved = 0;
    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        move = GetComponent<MoveableObject>();
    }

    internal override IEnumerator StartFight()
    {
        StartCoroutine(base.StartFight());
        yield return new WaitForSeconds(8);
        player.GetComponent<CutsceneMoveableObject>().enabled = false;
        Heal(100);
        enemyHealthBar.SetActive(true);
        player.playerHealthBar.SetActive(true);
        StartMusic();
        player.EnableInput();
        StartCoroutine(Combat());
    }

    protected override void Update()
    {
        base.Update();
    }

    IEnumerator Combat()
    {
        int pos = 0;
        yield return new WaitForSeconds(3);
        while (currentHealth > 0)
        {
            if(currentHealth < threshhold1 && moved < 1)
            {
                StartCoroutine(ScuttleForward());
                yield return new WaitForSeconds(3);
                moved++;
            }
            if (currentHealth < threshhold2 && moved < 2)
            {
                StartCoroutine(ScuttleForward());
                yield return new WaitForSeconds(3);
                moved++;
            }
            if (currentHealth < threshhold3 && moved < 3)
            {
                StartCoroutine(ScuttleForward());
                yield return new WaitForSeconds(3);
                moved++;
            }
            int nextMove = Random.Range(0, 4);
            switch(nextMove) {
                case 0:
                    if (pos < 0)
                    {
                        animator.Play("crab_scuttle");
                        ScuttleRight();
                        pos += 1;
                    }
                    else if (pos > 0)
                    {
                        animator.Play("crab_scuttle");
                        ScuttleLeft();
                        pos -= 1;
                    }
                    else
                    {
                        if(Random.value > 0.5) {
                            animator.Play("crab_scuttle");
                            ScuttleLeft();
                            pos -= 1;
                        }
                        else
                        {
                            animator.Play("crab_scuttle");
                            ScuttleRight();
                            pos += 1;
                        }
                    }
                    yield return new WaitForSeconds(1.4f);
                    break;
                case 1:
                    currentAction = StartCoroutine(Punch(LeftArm));
                    yield return new WaitForSeconds(1.4f);
                    break;
                case 2:
                    currentAction = StartCoroutine(Punch(RightArm));
                    yield return new WaitForSeconds(1.4f);
                    break;
                case 3:
                    currentAction = StartCoroutine(Punch(LeftArm));
                    currentAction = StartCoroutine(Punch(RightArm));
                    yield return new WaitForSeconds(1.4f);
                    break;
            }
            if(currentHealth < threshhold2)
            {
                yield return new WaitForSeconds(waitSeconds3);
            }
            else if (currentHealth < threshhold1)
            {
                yield return new WaitForSeconds(waitSeconds2);
            }
            else
            {
                yield return new WaitForSeconds(waitSeconds1);
            }
        }
    }

    protected override IEnumerator Hurt(int damage)
    {
        currentAction = StartCoroutine(base.Hurt(damage));
        state = combatState.hurt;
        TakeDamage(damage);
        currentAction = StartCoroutine(GameManager.FlickerRed(sprite));
        currentAction = StartCoroutine(GameManager.FlickerRed(LeftArm.GetComponent<SpriteRenderer>()));
        currentAction = StartCoroutine(GameManager.FlickerRed(RightArm.GetComponent<SpriteRenderer>()));
        sound.PlayAudio("punch-hit");
        float p = ((float)currentHealth / maxHealth);
        CropEnemyHealthSprite(p);
        yield return new WaitForSeconds(0.25f);
        state = combatState.none;
    }

    IEnumerator Punch(GameObject arm)
    {
        
        arm.GetComponent<Animator>().Play("wind");
        yield return new WaitForSeconds(1);
        arm.GetComponent<Animator>().Play("punch");
        yield return new WaitForSeconds(0.3f);
        foreach (BoxCollider2D box in arm.GetComponents<BoxCollider2D>()) {
            box.enabled = true;
        }
        yield return new WaitForSeconds(0.1f);
        foreach (BoxCollider2D box in arm.GetComponents<BoxCollider2D>())
        {
            box.enabled = false;
        }
    }

    void ScuttleLeft()
    {
        currentAction = StartCoroutine(move.Move(new Vector2(transform.position.x - 1, transform.position.y), moveSpeed));
    }

    void ScuttleRight()
    {
        currentAction = StartCoroutine(move.Move(new Vector2(transform.position.x + 1, transform.position.y), moveSpeed));
    }

    IEnumerator ScuttleForward()
    {
        player.DisableInput();
        animator.Play("crab_scream");
        player.GetComponent<CutsceneMoveableObject>().enabled = true;
        StartCoroutine(player.GetComponent<MoveableObject>().Move(new Vector2(player.transform.position.x, player.arenaMinHeight), 3));
        yield return new WaitForSeconds(2);
        player.GetComponent<CutsceneMoveableObject>().enabled = false;
        animator.Play("crab_scuttle");
        StartCoroutine(move.Move(new Vector2(transform.position.x, transform.position.y - 1), moveSpeed));
        yield return new WaitForSeconds(0.1f);
        player.EnableInput();
    }
}
