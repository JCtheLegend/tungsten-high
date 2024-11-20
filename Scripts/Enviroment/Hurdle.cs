using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hurdle : MonoBehaviour
{
    Animator anim;
    BoxCollider2D col;

    public float startDelay;
    public float downPeriod;
    public float upPeriod;
    public bool isActive = false;

    // Start is called before the first frame update
    void Start()
    {
        startDelay = 1 + Random.Range(0, 4) / 2.0f;
        downPeriod = 2 + Random.Range(0, 4) / 2.0f;
        upPeriod = 1 + Random.Range(0, 4) / 2.0f;

        anim = GetComponent<Animator>();
        col = GetComponent<BoxCollider2D>();
    }

    public void StartHurdles()
    {
        isActive = true;
        StartCoroutine(HurdleProcess());
    }

    public void StopHurdles()
    {
        isActive = false;
    }

    public IEnumerator HurdleProcess()
    {
        yield return new WaitForSeconds(startDelay);
        while (isActive)
        {
            col.enabled = true;
            GoUp();
            yield return new WaitForSeconds(upPeriod);
            GoDown();
            col.enabled = false;
            yield return new WaitForSeconds(downPeriod);
        }
    }

    void GoUp()
    {
        anim.Play("hurdle_up");
    }

    void GoDown()
    {
        anim.Play("hurdle_down");
    }
}
