using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchingBagController : EnemyController
{
    MoveableObject move;
    // Start is called before the first frame update
    [Header("Movement")]
    int moveSpeed;
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
        if (Input.GetKeyDown(KeyCode.P))
        {
            StartCoroutine(Move());
        }
    }

    protected override void HandleCollision(GameObject collisionObject)
    {
        base.HandleCollision(collisionObject);
    }

    IEnumerator Move()
    {
        while (currentHealth > 0)
        {
            Vector2 newSpot = new Vector2(Mathf.Floor(Random.Range(11, 15.99f))+0.5f, Mathf.Floor(Random.Range(7, 10.99f)));
            StartCoroutine(move.Move(newSpot, moveSpeed));
            yield return new WaitForSeconds(3f);
        }
    }

    internal override void Die()
    {
        base.Die();
        moveVelocity = new Vector2(0, 10);
    }
}
