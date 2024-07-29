using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableBox : MonoBehaviour
{
    Animator animator;
    BoxCollider2D col;
    SpriteRenderer s;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        col = GetComponent<BoxCollider2D>();
        s = GetComponent<SpriteRenderer>();
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
