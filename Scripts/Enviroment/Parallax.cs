using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    float startPos, length;
    public GameObject cam;
    public float parallaxEffect;
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        //float temp = cam.transform.position.x * (1 - parallaxEffect);
        float dist = (cam.transform.position.x * parallaxEffect);
        Debug.Log(dist);
        transform.position = new Vector3(startPos + dist, transform.position.y, transform.position.z);

        //if (temp > startPos + length) startPos += length;
        //else if (temp < startPos - length) startPos -= length;
    }
}
