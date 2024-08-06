using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyBag : MonoBehaviour
{
    int health = 4;
    public Sprite[] healthSprites;
    SpriteRenderer sprite;

    public int maxHeight;
    public int minHeight;
    public int maxWidth;
    public int minWidth;

    public GameObject quarter;
    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player Attack") && health > 0)
        {
            health--;
            sprite.sprite = healthSprites[health];
            if (health == 0)
            {
                StartCoroutine(Die());
            }
        }
    }

    IEnumerator Die()
    {
        GameManager.FadeOut(sprite, 3);
        for(int i = 0; i < 20; i++)
        {
            ThrowQuarters();
        }
        yield return new WaitForEndOfFrame();
        Destroy(this.gameObject);
    }

    void ThrowQuarters()
    {
        GameObject thrownQuarter = Instantiate(quarter, transform.position, Quaternion.identity);
        Vector2 endPoint = new Vector2(Random.Range(minWidth, maxWidth), Random.Range(minHeight, maxHeight));
        Vector2 controlPoint = new Vector2(endPoint.x / 2, 4);
        thrownQuarter.GetComponent<ArcJump>().point.Add(transform.position);
        thrownQuarter.GetComponent<ArcJump>().point.Add(controlPoint);
        thrownQuarter.GetComponent<ArcJump>().point.Add(endPoint);
        thrownQuarter.GetComponent<ArcJump>().moveEnabled = true; 
    }
}
