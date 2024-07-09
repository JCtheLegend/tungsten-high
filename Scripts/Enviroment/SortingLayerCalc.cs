using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortingLayerCalc : MonoBehaviour
{
    public bool yOverride;
    public float yCoord;
    SpriteRenderer sprite;

    const int LAYER_MID = 32767 / 2;
    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        if (transform.parent && !yOverride)
        {
            yCoord += transform.parent.position.y;
        }
    }

    private void Update()
    {
        CalcLayer();
    }

    void CalcLayer()
    {
        if (yOverride)
        {
            sprite.sortingOrder = LAYER_MID - (int)Mathf.Ceil((yCoord) * 10);
        }
        else
        {
            sprite.sortingOrder = LAYER_MID - (int)Mathf.Ceil((transform.position.y) * 10);
        }
    }
}
