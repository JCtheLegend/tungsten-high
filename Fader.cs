using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fader : MonoBehaviour
{
    public GameObject player;
    [SerializeField] SpriteRenderer[] inFade;
    [SerializeField] SpriteRenderer[] outFade;
    [SerializeField] float speed;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        foreach(SpriteRenderer s in inFade)
        {
            StartCoroutine(GameManager.FadeIn(s, speed));
        }
        foreach (SpriteRenderer s in outFade)
        {
            StartCoroutine(GameManager.FadeOut(s, speed));
        }
    }
}
