using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableBox : ScienceObject
{
    Animator animator;
    BoxCollider2D col;
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
        col = GetComponent<BoxCollider2D>();
    }

    public override void ResetValues()
    {
        base.ResetValues();
        col.enabled = true;
        s.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        HandleCollision(collision.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        HandleCollision(collision.gameObject);
    }
    protected virtual void HandleCollision(GameObject collisionObject)
    {
        if (collisionObject.CompareTag("Player Attack"))
        {
            StartCoroutine(Die());
        }
    }

    IEnumerator Die()
    {
        animator.Play("Box Break");
        yield return new WaitForSeconds(0.833f);

        col.enabled = false;
        s.enabled = false;
    }
}
