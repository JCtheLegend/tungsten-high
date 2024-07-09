using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuckZone : MonoBehaviour
{
    bool suckThePlayer;
    public int suckSpeed;
    GameObject player;
    Rigidbody2D rb;
    SpriteRenderer sprite;
    // Update is called once per frame
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
    }
    void LateUpdate()
    {
        transform.position = transform.position + new Vector3(-0.00001f, 0);
        if (suckThePlayer)
        {
            if (player.GetComponent<PlayerCombatController>().action == PlayerCombatController.combatAction.hurt)
            {
                player.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -5);
            }
            else
            {
                Vector2 v2 = (transform.position - player.transform.position).normalized * suckSpeed;
                player.GetComponent<PlayerCombatController>().lockMovement = false;
                player.GetComponent<Rigidbody2D>().velocity = player.GetComponent<Rigidbody2D>().velocity + v2;
            }
        }
        transform.position = transform.position + new Vector3(0.00001f, 0);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            player = collision.gameObject;
            suckThePlayer = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            player.GetComponent<PlayerCombatController>().lockMovement = true;
            suckThePlayer = false;
        }
    }
}
