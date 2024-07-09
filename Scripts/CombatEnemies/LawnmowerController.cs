using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LawnmowerController : EnemyController
{
    public GameObject cord;
    MoveableObject move;
    bool isAttacking = false;
    [Header("Animations")]
    public string bigSuckTiltDownAnim;
    public string bigSuckTiltUpAnim;
    public string bigSuckSpinAnim;
    public string idleAnim;
    public string swingAnim;
    public string preSwingAnim;
    public string postSwingAnim;
    [Header("Bounds")]
    public float minX;
    public float minY;
    public float maxX;
    public float maxY;
    [Header("AdjustSwing")]
    public float number1;
    public float number2;
    [Header("Movement")]
    public int mowSpeed;
    public int sideSpeed;
    bool stoppingSuck;

    IEnumerator attackRoutine;
    bool isSucking = false;

    public GameObject explosion;
    // Start is called before the first frame update
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
        base.Update();
        if (Input.GetKeyDown(KeyCode.P))
        {
            StartCoroutine(Combat());
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            StopCoroutine(attackRoutine);
            StartCoroutine(StopSuck());
        }
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

    protected override void HandleCollision(GameObject collisionObject)
    {
        if (collisionObject.CompareTag("Player") && isSucking)
        {
            StopCoroutine(attackRoutine);
            StartCoroutine(StopSuck());
        }
        base.HandleCollision(collisionObject);
    }

    IEnumerator Combat()
    {
        while (currentHealth > 0)
        {
            if (!isAttacking)
            {
                isAttacking = true;
                int attackNum = Random.Range(1, 4);
                switch (attackNum)
                {
                    case 1:
                        attackRoutine = TheBigSuck();
                        StartCoroutine(attackRoutine);
                        break;
                    case 2:
                        attackRoutine = SwingyThingy();
                        StartCoroutine(attackRoutine);
                        break;
                    case 3:
                        attackRoutine = MowDown();
                        StartCoroutine(attackRoutine);
                        break;
                }
            }
            while (isAttacking)
            {
                yield return new WaitForSeconds(1);
            }
        }
    }

    IEnumerator TheBigSuck()
    {
        isSucking = true;
        ChangeAnimationState(bigSuckTiltDownAnim, 0);
        yield return new WaitForSeconds(animator.runtimeAnimatorController.animationClips[0].length);
        ChangeAnimationState(bigSuckSpinAnim, 0);
        yield return new WaitForSeconds(0.5f);
        sound.PlayAudio("mower-long");
        transform.GetChild(1).GetComponent<PolygonCollider2D>().enabled = true;
        transform.GetChild(1).GetComponent<SpriteRenderer>().enabled = true;
        yield return new WaitForSeconds(5);
        isSucking = false;
        StartCoroutine(StopSuck());
    }

    IEnumerator StopSuck()
    {
        sound.StopAudio();
        transform.GetChild(1).GetComponent<PolygonCollider2D>().enabled = false;
        yield return new WaitForEndOfFrame();
        ChangeAnimationState(bigSuckTiltUpAnim, 0);
        yield return new WaitForSeconds(animator.runtimeAnimatorController.animationClips[0].length);
        transform.GetChild(1).GetComponent<SpriteRenderer>().enabled = false;
        ChangeAnimationState(idleAnim, 0);
        attackRoutine = null;
        isAttacking = false;
    }

    IEnumerator SwingyThingy()
    {
        ChangeAnimationState(swingAnim, 0);
        yield return new WaitForSeconds(3);
        ChangeAnimationState(preSwingAnim, 0);
        yield return new WaitForSeconds(animator.runtimeAnimatorController.animationClips[0].length);
        cord.GetComponent<BoxCollider2D>().enabled = true;
        cord.GetComponent<SpriteRenderer>().enabled = true;
        ChangeAnimationState(idleAnim, 0);
        StartCoroutine(cord.GetComponent<MoveableObject>().RotateAroundPoint(true, new Vector2(transform.position.x + number1, transform.position.y + number2), -45, 2.5f));
        while (MoveableObject.ConvertSwingAngle(cord.GetComponent<MoveableObject>().NormalAngle(), 180) > -45)
        {
            yield return new WaitForEndOfFrame();
        }
        cord.GetComponent<BoxCollider2D>().enabled = false;
        cord.GetComponent<SpriteRenderer>().enabled = false;
        cord.transform.position = new Vector3(this.transform.position.x + 3.3f, this.transform.position.y + 0.45f, 0);
        cord.transform.rotation = Quaternion.Euler(cord.transform.rotation.x, cord.transform.rotation.y, 45);
        ChangeAnimationState(postSwingAnim, 0);
        yield return new WaitForSeconds(animator.runtimeAnimatorController.animationClips[0].length);
        ChangeAnimationState(idleAnim, 0);
        attackRoutine = null;
        isAttacking = false;
    } 

    IEnumerator MowDown()
    {
        StartCoroutine(move.Move(new Vector2(minX, rb.position.y), sideSpeed));
        while (rb.position.x > minX)
        {
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(move.Move(new Vector2(maxX, rb.position.y), sideSpeed));
        while (rb.position.x < maxX)
        {
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(0.5f);
        float rushPos = Random.Range(0, maxX - minX);
        StartCoroutine(move.Move(new Vector2(minX + rushPos, rb.position.y), sideSpeed));
        while (rb.position.x > minX + rushPos)
        {
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(0.5f);
        sound.PlayAudio("mower");
        StartCoroutine(move.Move(new Vector2(minX + rushPos, minY), mowSpeed));
        while(rb.position.y > minY)
        {
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(2);
        rb.position = new Vector2(rb.position.x, 10);
        while (rb.position.y < 10)
        {
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForEndOfFrame();
        StartCoroutine(move.Move(new Vector2(rb.position.x, maxY), mowSpeed));
        while (rb.position.y > maxY)
        {
            yield return new WaitForSeconds(0.1f);
        }
        attackRoutine = null;
        isAttacking = false;
    }

    internal override void Die()
    {
        music.ChangeSong("Victory");
        state = combatState.dead;
        if (attackRoutine != null)
        {
            StopCoroutine(attackRoutine);
        }
        rb.velocity = Vector2.zero;
        base.Die();
        StartCoroutine(Explode());
    }

    IEnumerator Explode()
    {
        for(int i = 0; i < 50; i++)
        {
            Vector2 randomCoord = new Vector2(Random.Range(sprite.bounds.min.x, sprite.bounds.max.x), Random.Range(sprite.bounds.min.y, sprite.bounds.max.y));
            float randomSize = Random.Range(0.2f, 1f);
            GameObject explode = Instantiate(explosion, randomCoord, Quaternion.identity);
            explode.transform.localScale *= randomSize;
            yield return new WaitForSeconds(0.1f);
            StartCoroutine(GameManager.FlickerRed(sprite));
        }
    }
}
