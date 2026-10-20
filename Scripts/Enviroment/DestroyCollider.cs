using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Destroys anything that touches this collider -- used to clean up projectiles that leave the arena.
public class DestroyCollider : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(collision.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(collision.gameObject);
    }
}
