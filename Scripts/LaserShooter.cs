using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserShooter : MonoBehaviour
{
    public Vector2 offset;
    public SpriteRenderer beam;
    public SpriteRenderer impact;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        DrawLaser();  
    }

    void DrawLaser()
    {
        int layerMask = LayerMask.GetMask("Player", "Wall");
        Vector2 laserVector = new Vector2(this.transform.position.x, this.transform.position.y) + offset;
        RaycastHit2D hit = Physics2D.Raycast(laserVector, Vector2.down, 10, layerMask);
        if (hit)
        {
            beam.transform.localScale = new Vector3(1, Vector3.Distance(transform.position, hit.point) - 1.5f, 1);
            impact.transform.position = new Vector2(transform.position.x, hit.point.y - 0.25f);
        }
    }
}
