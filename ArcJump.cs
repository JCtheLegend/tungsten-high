using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcJump : MonoBehaviour
{

    public bool moveEnabled = false;

    public List<Vector2> point = new List<Vector2>();
    void Start()
    {
        
    }
    float count = 0.0f;
    // Update is called once per frame
    void Update()
    {
        if (!moveEnabled)
            return;

        if (count < 1.0f)
        {
            count += 1.0f * Time.deltaTime;

            Vector3 m1 = Vector3.Lerp(point[0], point[1], count);
            Vector3 m2 = Vector3.Lerp(point[1], point[2], count);
            transform.position = Vector3.Lerp(m1, m2, count);
        }
    }
}
