using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserShooter : MonoBehaviour
{
    public GameObject beamPrefab;
    LaserBeam startBeam;
    Color innerColor = Color.white;
    public Color middleColor;
    public Color outerColor;
    public string type;

    public direction dir = direction.down;
    public Vector3 offset;
    // Start is called before the first frame update
    void Awake()
    {

        startBeam = Instantiate(beamPrefab, transform.position + offset, Quaternion.identity, transform.parent).GetComponent<LaserBeam>();
        transform.Find("Laser Bulb/Laser Bulb Inner").GetComponent<SpriteRenderer>().color = innerColor;
        transform.Find("Laser Bulb/Laser Bulb Middle").GetComponent<SpriteRenderer>().color = middleColor;
        transform.Find("Laser Bulb/Laser Bulb Outer").GetComponent<SpriteRenderer>().color = outerColor;
        startBeam.innerColor = innerColor;
        startBeam.middleColor = middleColor;
        startBeam.outerColor = outerColor;
        startBeam.type = type;
    }

    // Update is called once per frame
    void Update()
    {
        startBeam.DrawLaser(dir, startBeam.transform, Vector2.zero);
    }
}
