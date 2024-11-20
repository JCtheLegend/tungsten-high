using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeballTarget : MonoBehaviour
{
    SpriteRenderer s;
    public Sprite hitSprite;
    public bool hit;
    public bool spin;
    public GameObject hitAlert;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        s = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name == "Ally Dodgeball" && !hit)
        {
            s.sprite = hitSprite;
            hit = true;
            this.gameObject.GetComponent<Collider2D>().enabled = false;
            StartCoroutine(Die());
            if (spin)
            {
                StartCoroutine(Spin());
            }
            else
            {
                StartCoroutine(FallDown());
            }
        }
    }

    IEnumerator FallDown()
    {
        while (transform.eulerAngles.x < 60)
        {
            transform.eulerAngles = new Vector3(transform.localEulerAngles.x + 1, 0, 0);
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator Die()
    {
        GameObject g = Instantiate(hitAlert, this.transform.position + new Vector3(1, 1, 0), Quaternion.identity);
        yield return new WaitForSeconds(1);
        Destroy(g);
    }

    IEnumerator Spin()
    {
        int i = 0;
        while (i < 100)
        {
            transform.eulerAngles = new Vector3(transform.localEulerAngles.y + (100 - i), 0, 0);
            i++;
            yield return new WaitForEndOfFrame();
        }
    }
}
