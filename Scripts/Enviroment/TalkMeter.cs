using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkMeter : MonoBehaviour
{

    public SpriteRenderer bottomGoal;
    public SpriteRenderer topGoal;
    public SpriteRenderer goalFiller;
    public SpriteRenderer movingBar;
    public SpriteRenderer face;

    public Sprite green;
    public Sprite red;
    public Sprite gray;

    public Sprite sadFace;
    public Sprite happyFace;

    bool fillingUp = true;
    bool stopped = false;

    public float range;
    public float speed;
    bool start = false;

    public void StartTalkMeter()
    {
        float topGoalPos = Random.Range(-0.9f + range, 0.9f);
        float bottomGoalPos = topGoalPos - range;
        topGoal.gameObject.transform.localPosition = new Vector2(0, topGoalPos);
        bottomGoal.gameObject.transform.localPosition = new Vector2(0, bottomGoalPos);
        goalFiller.gameObject.transform.localPosition = new Vector2(0, (topGoalPos + bottomGoalPos) / 2);
        goalFiller.gameObject.transform.localScale = new Vector2(1, (topGoalPos - bottomGoalPos)*4);
        start = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            stopped = true;
            StartCoroutine(StopMeter());
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (start && !stopped)
        {
            if (fillingUp)
            {
                movingBar.gameObject.transform.localScale = new Vector2(1, movingBar.gameObject.transform.localScale.y + 0.01f * speed);
            }
            else
            {
                movingBar.gameObject.transform.localScale = new Vector2(1, movingBar.gameObject.transform.localScale.y - 0.01f * speed);
            }
            if (movingBar.gameObject.transform.localScale.y > 1)
            {
                fillingUp = false;
            }
            if (movingBar.gameObject.transform.localScale.y < 0)
            {
                fillingUp = true;
            }
        }
    }

    public bool InZone()
    {
        return 2 * movingBar.gameObject.transform.localScale.y - 1 > bottomGoal.gameObject.transform.localPosition.y &&
            2 * movingBar.gameObject.transform.localScale.y - 1 < topGoal.gameObject.transform.localPosition.y;
    }

    IEnumerator StopMeter()
    {
        if (InZone())
        {
            face.sprite = happyFace;
            movingBar.sprite = green;
            yield return new WaitForSeconds(0.1f);
            movingBar.sprite = gray;
            yield return new WaitForSeconds(0.1f);
            movingBar.sprite = green;
            yield return new WaitForSeconds(0.1f);
            movingBar.sprite = gray;
            yield return new WaitForSeconds(0.1f);
            movingBar.sprite = green;
            yield return new WaitForSeconds(0.1f);
            movingBar.sprite = gray;
            yield return new WaitForSeconds(0.1f);
            movingBar.sprite = green;
        }
        else
        {
            face.sprite = sadFace;
            movingBar.sprite = red;
            yield return new WaitForSeconds(0.1f);
            movingBar.sprite = gray;
            yield return new WaitForSeconds(0.1f);
            movingBar.sprite = red;
            yield return new WaitForSeconds(0.1f);
            movingBar.sprite = gray;
            yield return new WaitForSeconds(0.1f);
            movingBar.sprite = red;
            yield return new WaitForSeconds(0.1f);
            movingBar.sprite = gray;
            yield return new WaitForSeconds(0.1f);
            movingBar.sprite = red;
        }
        yield return new WaitForSeconds(0.5f);
        Destroy(this.gameObject);
    }
}
