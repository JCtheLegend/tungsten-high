using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prism : MonoBehaviour
{
    public Vector2 offset;
    public bool lasered = false;
    public direction[] inputDirs;
    public direction[] outputDirs;
    direction shootDir;
    GameObject newBeam;
    int layerMask;
    // Update is called once per frame
    private void Start()
    {
        layerMask = LayerMask.GetMask("Player", "Wall");
    }
    private void Update()
    {
        if (lasered)
        {
            ShootLaser(offset);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
       
        if (collision.gameObject.name == "Laser Beam")
        {
            if (collision.gameObject.GetComponent<LaserBeam>().dir == inputDirs[0])
            {
                shootDir = outputDirs[0];
            }
            else
            {
                shootDir = outputDirs[1];
            }
            if (newBeam == null)
            {
                newBeam = CreateNewBeam(collision.gameObject);             
            }
            lasered = true; 
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Laser Beam")
        {
            Destroy(newBeam);
            newBeam = null;
            lasered = false;
        }
    }

    public void ShootLaser(Vector3 beamOffset)
    {
        if (newBeam)
        {
            newBeam.transform.position = transform.position;
        }
        Vector2 laserVector = transform.position + beamOffset;
        RaycastHit2D hit = new RaycastHit2D();
        switch (shootDir)
        {
            case direction.down:
                hit = Physics2D.Raycast(laserVector, Vector2.down, 1000, layerMask);
                break;
            case direction.right:
                hit = Physics2D.Raycast(laserVector, Vector2.right, 1000, layerMask);
                break;
        }
        Debug.DrawRay(laserVector, Vector3.right, Color.green);
        if (hit)
        {
            if (hit.collider.gameObject.name == "Prism" && hit.collider.gameObject.GetComponent<Prism>().lasered == false)
            {
                newBeam.transform.localScale = new Vector3(1, 1, 1);
            }
            else
            {
                switch (shootDir)
                {
                    case direction.down:
                        newBeam.transform.localScale = new Vector3(1, 1, 1);
                        newBeam.transform.Find("Laser Impact").position = new Vector2(transform.position.x, hit.point.y + 0.1875f);
                        newBeam.transform.Find("Laser Impact").localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
                        break;
                    case direction.right:
                        newBeam.transform.localScale = new Vector3(1, Vector3.Distance(laserVector, hit.point), 1);
                        //newBeam.transform.Find("Laser Impact").gameObject.SetActive(true);
                        //newBeam.transform.Find("Laser Impact").position = new Vector2(hit.point.x - 0.1875f, hit.point.y);
                        //newBeam.transform.Find("Laser Impact").localRotation = Quaternion.Euler(new Vector3(0, 0, 90));
                        break;
                }
            }
        }
    }
    GameObject CreateNewBeam(GameObject g)
    {
        newBeam = Instantiate(g, this.transform.position, Quaternion.identity, this.transform);
        
        newBeam.transform.localScale = new Vector3(1, 1, 1);
        switch (shootDir)
        {
            case direction.right:
                newBeam.transform.Rotate(0, 0, 90);
                break;
        }
        return newBeam;
    }
}
