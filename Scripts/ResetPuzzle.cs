using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPuzzle : MonoBehaviour
{

    public GameObject[] objects;
    List<Vector2> positions = new List<Vector2>();
    List<Sprite> sprites = new List<Sprite>();

    public void RecordPositions()
    {
        foreach(GameObject g in objects)
        {
            Debug.Log(g);
            positions.Add(g.transform.position);
            sprites.Add(g.GetComponent<SpriteRenderer>().sprite);
        }
    }

    public void ResetPositions()
    {
        for(int i = 0; i < objects.Length; i++)
        {
            objects[i].transform.position = positions[i];
            objects[i].GetComponent<SpriteRenderer>().sprite = sprites[i];
        }
    }
}
