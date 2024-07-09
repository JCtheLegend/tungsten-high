using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInOut : MonoBehaviour
{
    SpriteRenderer s;
    // Start is called before the first frame update
    void Start()
    {
        s = GetComponent<SpriteRenderer>();
        StartCoroutine(Faded());
    }

    IEnumerator Faded()
    {
        while (true)
        {
            StartCoroutine(GameManager.FadeOut(s, 1));
            yield return new WaitForSeconds(2);
            StartCoroutine(GameManager.FadeIn(s, 1));
            yield return new WaitForSeconds(2);
        }
    }
}
