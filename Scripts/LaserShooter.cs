using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserShooter : MonoBehaviour
{
    float bulbWidth = -0.4f;
    public Vector2 offset;
    public Transform startBeam;
    public Transform impact;

    public SpriteRenderer[] inner;
    public SpriteRenderer[] middle;
    public SpriteRenderer[] outer;

    public Color innerColor;
    public Color middleColor;
    public Color outerColor;

    public direction dir = direction.down;

    int layerMask;
    // Start is called before the first frame update
    void Start()
    {
        layerMask = LayerMask.GetMask("Player", "Wall");
        foreach (SpriteRenderer s in inner)
        {
            s.color = innerColor;
        }
        foreach (SpriteRenderer s in middle)
        {
            s.color = middleColor;
        }
        foreach (SpriteRenderer s in outer)
        {
            s.color = outerColor;
        }
    }

    // Update is called once per frame
    void Update()
    {
        DrawLaser(direction.down, startBeam, offset);
    }

    void DrawLaser(direction dir, Transform beam, Vector3 beamOffset)
    {
        Vector2 laserVector = transform.position + beamOffset;
        RaycastHit2D hit = new RaycastHit2D();
        
        switch (dir)
        {
            case direction.down:
                hit = Physics2D.Raycast(laserVector, Vector2.down, 1000, layerMask);
                break;
            case direction.right:
                hit = Physics2D.Raycast(laserVector, Vector2.right, 1000, layerMask);             
                break;
        }
        if (hit)
        {
           
                switch (dir)
                {
                    case direction.down:
                        beam.localScale = new Vector3(1, Vector3.Distance(laserVector, hit.point) + bulbWidth, 1);
                        impact.position = new Vector2(transform.position.x, hit.point.y + 0.1875f);
                        impact.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
                        break;
                    case direction.right:
                        beam.localScale = new Vector3(1, Vector3.Distance(laserVector, hit.point), 1);
                        impact.position = new Vector2(hit.point.x - 0.1875f, hit.point.y);
                        impact.localRotation = Quaternion.Euler(new Vector3(0, 0, 90));
                        break;
                }
              
        }
    }

  
}
