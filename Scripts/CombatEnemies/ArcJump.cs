using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcJump : MonoBehaviour
{
    public bool jumping = false;
    public List<Vector2> point = new List<Vector2>();
    public float speed;
    public float count = 0.0f;
    // Update is called once per frame
    void Update()
    {
        if (!jumping)
            return;

        if (count < 1.0f)
        {
            count += 1.0f * Time.deltaTime * speed;

            Vector3 m1 = Vector3.Lerp(point[0], point[1], count);
            Vector3 m2 = Vector3.Lerp(point[1], point[2], count);
            transform.position = Vector3.Lerp(m1, m2, count);
        }
        else
        {
            ResetJump();
            jumping = false;
        }
    }

    public void ResetJump()
    {
        count = 0;
    }
}
