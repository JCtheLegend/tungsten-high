using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortingLayerCalc : MonoBehaviour
{
    public float yCoord;
    SpriteRenderer sprite;

    const int LAYER_MAX = 32767;
    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        sprite.sortingOrder = LAYER_MAX - (int)Mathf.Ceil((yCoord + 50) * 100);
    }
}
