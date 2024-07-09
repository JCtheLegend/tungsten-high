using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DreamEntrance : MonoBehaviour
{
    public SpriteRenderer rug;
    public Vector2 scale;
    public IEnumerator AnimateEnter()
    {
        while(transform.localScale.y < scale.y)
        {
            if (rug != null)
            {
                rug.transform.Rotate(0, 0, 10);
            }
            transform.localScale = new Vector3(1, transform.localScale.y + 1, 1);
            yield return new WaitForSeconds(0.1f);
        }
        while(transform.localScale.x < scale.x)
        {
            if (rug != null)
            {
                rug.transform.Rotate(0, 0, 10);
            }
            transform.localScale = new Vector3(transform.localScale.x + 1, transform.localScale.y, 1);
            yield return new WaitForSeconds(0.1f);
        }
        if(rug != null)
        {
            rug.transform.rotation = new Quaternion(0, 0, 0, 0);
        }
    }
}
