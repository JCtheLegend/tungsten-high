using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dodgeball : MonoBehaviour
{
    public Vector2 vel;
    public bool flying;
    Rigidbody2D rb;
    Coroutine c;
    ArcJump arcJump;
    private void Start()
    {
        arcJump = GetComponent<ArcJump>();
        rb = GetComponent<Rigidbody2D>();
        //c = StartCoroutine(Decay());
    }
    // Update is called once per frame
    void Update()
    {
        rb.velocity = vel;
    }

    public void Pickup()
    {
        if (c != null)
        {
            StopCoroutine(c);
        }
    }

    public void Throw(Vector2 vel)
    {
        this.vel = vel;
        if(c != null)
        {
            StopCoroutine(c);
        }
        //c = StartCoroutine(Decay());
    }

    IEnumerator Decay()
    {
        yield return new WaitForSeconds(10);
        Destroy(this.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.name == "Dodgeball Wall")
        {
            bool goingUp = vel.y > 0;
            vel = Vector2.zero;
            arcJump.point[0] = transform.position;
            arcJump.point[1] = new Vector2(transform.position.x + Random.Range(-1, 2),transform.position.y + 2);
            arcJump.point[2] = new Vector2(transform.position.x + Random.Range(-1, 2), goingUp ? transform.position.y - 1 : transform.position.y + 1);
            arcJump.jumping = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("Dodgeball Wall"))
        {
            bool goingUp = vel.y > 0;
            bool goingRight = vel.x > 0;
            vel = Vector2.zero;
            arcJump.point[0] = transform.position;
            arcJump.point[1] = new Vector2(goingRight ? transform.position.x + 1 : transform.position.x - 1, transform.position.y + 2);
            arcJump.point[2] = new Vector2(goingRight ? transform.position.x + 2 : transform.position.x - 2, goingUp ? transform.position.y - 1 : transform.position.y + 1);
            arcJump.jumping = true;
            this.gameObject.name = "Dodgeball";
        }
    }
}
