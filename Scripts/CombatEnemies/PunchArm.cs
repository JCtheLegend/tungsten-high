using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchArm : MonoBehaviour
{

    public PunchingBagBlockController bag;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Shield")
        {
            StartCoroutine(bag.Stunned());
        }
    }
}
