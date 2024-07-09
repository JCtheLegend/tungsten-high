using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thanks : MonoBehaviour
{

    public SpriteRenderer s;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ThanksALot());
    }

    IEnumerator ThanksALot()
    {
        StartCoroutine(GameManager.FadeOut(s, 1));
        yield return new WaitForSeconds(10);
        StartCoroutine(GameManager.FadeIn(s, 1));
        yield return new WaitForSeconds(1);
        GameManager.ResetGameData();
        GameManager.LoadScene("Menu");
    }
}
