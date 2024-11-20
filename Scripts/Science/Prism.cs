using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prism : MonoBehaviour
{
    Vector2 offset= new Vector2(0.3f, 0.3f);
    public bool lasered = false;
    public List<direction> inputDirs;
    public List<direction> outputDirs;


    Dictionary<GameObject, GameObject> newBeams = new Dictionary<GameObject, GameObject>();
    
    public void TurnOnPrism(direction inputDir, GameObject laser)
    {
        direction shootDir;
        if (inputDir == inputDirs[0])
        {
            shootDir = outputDirs[0];
        }
        else
        {
            shootDir = outputDirs[1];
        }
        if (!newBeams.ContainsKey(laser))
        {
            newBeams[laser] = CreateNewBeam(laser);
        }
        LaserBeam l = newBeams[laser].GetComponent<LaserBeam>();
        switch (shootDir)
        {
            case direction.up:
                l.DrawLaser(shootDir, newBeams[laser].transform, new Vector2(0, offset.y));
                break;
            case direction.right:
                l.DrawLaser(shootDir, l.transform, new Vector2(offset.x, 0));
                break;
            case direction.left:
                l.DrawLaser(shootDir, l.transform, new Vector2(-offset.x, 0));
                break;
            case direction.down:
                l.DrawLaser(shootDir, l.transform, new Vector2(0, -offset.y));
                break;
        }

    }

    public void ResetPrism(GameObject g)
    {
        Destroy(newBeams[g]);
        newBeams.Remove(g);
        lasered = false;
    }

    GameObject CreateNewBeam(GameObject g)
    {
        GameObject newBeam = Instantiate(g, this.transform.position, Quaternion.identity, this.transform);
        newBeam.name = g.name;
        return newBeam;
    }
}
