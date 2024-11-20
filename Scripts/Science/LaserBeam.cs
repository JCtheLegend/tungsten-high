using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeam : MonoBehaviour
{
    public string type;
    public Color innerColor;
    public Color middleColor;
    public Color outerColor;

    public direction dir;

    int layerMask;
    public GameObject impactPrefab;

    public GameObject impact;

    GameObject prism;

    private void Start()
    {
        impact = Instantiate(impactPrefab, this.transform.parent);
        impact.name = "Laser Impact";
        impact.transform.Find("Lab Impact Inner").GetComponent<SpriteRenderer>().color = innerColor;
        impact.transform.Find("Lab Imapt Middle").GetComponent<SpriteRenderer>().color = middleColor;
        impact.transform.Find("Lab Impact Outer").GetComponent<SpriteRenderer>().color = outerColor;
        layerMask = type == "block" ? LayerMask.GetMask("Player", "Wall") : LayerMask.GetMask("Player", "Wall", "Laser Interact");
        transform.Find("Lab Beam Inner").GetComponent<SpriteRenderer>().color = innerColor;
        transform.Find("Lab Beam Middle").GetComponent<SpriteRenderer>().color = middleColor;
        transform.Find("Lab Beam Outer").GetComponent<SpriteRenderer>().color = outerColor;
        if(type == "block")
        {
            Debug.Log("BOCK");
            gameObject.layer = 14;
        }
    }

    private void OnDestroy()
    {
        Destroy(impact);
    }

    public void DrawLaser(direction dir, Transform beam, Vector3 offset)
    {
        Vector2 laserVector = transform.position + offset;
        RaycastHit2D hit = new RaycastHit2D();

        switch (dir)
        {
            case direction.down:
                hit = Physics2D.Raycast(laserVector, Vector2.down, 1000, layerMask);
                break;
            case direction.right:
                hit = Physics2D.Raycast(laserVector, Vector2.right, 1000, layerMask);
                transform.rotation = Quaternion.Euler(0, 0, 90);
                break;
            case direction.up:
                hit = Physics2D.Raycast(laserVector, Vector2.up, 1000, layerMask);
                transform.rotation = Quaternion.Euler(0, 0, 180);
                break;
            case direction.left:
                hit = Physics2D.Raycast(laserVector, Vector2.left, 1000, layerMask);
                transform.rotation = Quaternion.Euler(0, 0, 270);
                break;
        }
        if (hit)
        {
            //Debug.DrawLine(laserVector, hit.point, Color.red);
            if (hit.collider.name != "Prism" || (hit.collider.name == "Prism" && !hit.transform.gameObject.GetComponent<Prism>().inputDirs.Contains(dir)))
            {
                if (prism != null)
                {
                    prism.GetComponent<Prism>().ResetPrism(gameObject);
                    prism = null;
                }
                impact.SetActive(true);
                switch (dir)
                {
                    case direction.down:
                        impact.transform.position = new Vector2(transform.position.x, hit.point.y + 0.1875f);
                        break;
                    case direction.right:
                        impact.transform.position = new Vector2(hit.point.x - 0.1875f, hit.point.y);
                        break;
                    case direction.up:
                        impact.transform.position = new Vector2(transform.position.x, hit.point.y - 0.1875f);
                        break;
                    case direction.left:
                        impact.transform.position = new Vector2(hit.point.x + 0.1875f, hit.point.y);
                        break;
                }
            }
            else
            {
                prism = hit.transform.gameObject;
                prism.GetComponent<Prism>().TurnOnPrism(dir, this.gameObject);
                impact.SetActive(false);
            }
            switch (dir)
            {
                case direction.down:
                    beam.localScale = new Vector3(1, Vector3.Distance(this.transform.position, hit.point), 1);
                    impact.transform.position = new Vector2(transform.position.x, hit.point.y + 0.1875f);
                    impact.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
                    break;
                case direction.right:
                    beam.localScale = new Vector3(1, Vector3.Distance(this.transform.position, hit.point), 1);
                    impact.transform.position = new Vector2(hit.point.x - 0.1875f, hit.point.y);
                    impact.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 90));
                    break;
                case direction.left:
                    beam.localScale = new Vector3(1, Vector3.Distance(this.transform.position, hit.point), 1);
                    impact.transform.position = new Vector2(hit.point.x + 0.1875f, hit.point.y);
                    impact.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 270));
                    break;
                case direction.up:
                    beam.localScale = new Vector3(1, Vector3.Distance(this.transform.position, hit.point), 1);
                    impact.transform.position = new Vector2(transform.position.x, hit.point.y - 0.1875f);
                    impact.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 180));
                    break;
            }
        }    
    }
}
