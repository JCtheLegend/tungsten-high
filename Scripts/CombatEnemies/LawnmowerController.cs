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
    public int minX;
    public int minY;
    public int maxX;
    public int maxY;
    [Header("AdjustSwing")]
    public float number1;
    public float number2;
    [Header("Movement")]
    public int mowSpeed;
    public int sideSpeed;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        move = GetComponent<MoveableObject>();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        Debug.DrawLine(new Vector2(transform.position.x, 4), new Vector2(number1, 4));
        Debug.DrawLine(new Vector2(0, transform.position.y), new Vector2(0, number2));
        if (Input.GetKeyDown(KeyCode.P))
        {
            StartCoroutine(Combat());
        }
    }

    protected override void HandleCollision(GameObject collisionObject)
    {
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
                        StartCoroutine(TheBigSuck());
                        break;
                    case 2:
                        StartCoroutine(SwingyThingy());
                        break;
                    case 3:
                        StartCoroutine(MowDown());
                        break;
                }
            }
            yield return new WaitForSeconds(5);
        }
    }

    IEnumerator TheBigSuck()
    {
        Debug.Log("suck");
        ChangeAnimationState(bigSuckTiltDownAnim, 0);
        yield return new WaitForSeconds(animator.runtimeAnimatorController.animationClips[0].length);
        ChangeAnimationState(bigSuckSpinAnim, 0);
        transform.GetChild(1).GetComponent<PolygonCollider2D>().enabled = true;
        yield return new WaitForSeconds(5);
        transform.GetChild(1).GetComponent<PolygonCollider2D>().enabled = false;
        ChangeAnimationState(bigSuckTiltUpAnim, 0);
        yield return new WaitForSeconds(animator.runtimeAnimatorController.animationClips[0].length);
        ChangeAnimationState(idleAnim, 0);
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
        StartCoroutine(cord.GetComponent<MoveableObject>().RotateAroundPoint(true, new Vector2(number1, number2), -45, 0.25f));
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
        isAttacking = false;
    } 

    IEnumerator MowDown()
    {
        Debug.Log("mow");
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
        int rushPos = Random.Range(0, maxX - minX);
        StartCoroutine(move.Move(new Vector2(minX + rushPos, rb.position.y), sideSpeed));
        while (rb.position.x > minX + rushPos)
        {
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(move.Move(new Vector2(minX + rushPos, minY), mowSpeed));
        while(rb.position.y > minY)
        {
            yield return new WaitForSeconds(0.1f);
        }
        rb.position = new Vector2(0, 4);
        isAttacking = false;
    }

    internal override void Die()
    {
        base.Die();
    }


}
