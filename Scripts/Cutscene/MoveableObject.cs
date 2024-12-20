using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveableObject : MonoBehaviour
{
    internal Rigidbody2D rb;

    Vector2 vel = Vector2.zero;
    float spin = 0;
    float spinAroundSpeed = 0;

    Vector2 rotatePoint;
    protected void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void FixedUpdate()
    {
        rb.velocity = vel;
        transform.Rotate(0, 0, spin);
        transform.RotateAround(rotatePoint, Vector3.forward, spinAroundSpeed);
    } 

    public IEnumerator Move(Vector2 coords, float speed)
    {
        Vector2 initialPos = rb.position;
        Vector2 normVec = (coords - initialPos).normalized;
        while (!ArrivedToPoint(initialPos, rb.position, coords))
        {         
            vel = 1.0f * speed * normVec;
            yield return new WaitForEndOfFrame();
        }
        rb.position = coords;
        vel = Vector2.zero;
    }

    public IEnumerator Rotate(bool clockwise, float deg, float speed)
    {
        while (transform.rotation.z < deg)
        {
            spin = speed;
            yield return new WaitForEndOfFrame();
        }
        spin = 0;
    }

    public float NormalAngle()
    {
        return transform.eulerAngles.z - 360 * Mathf.Floor(transform.eulerAngles.z / 360);
    }

    public static float ConvertSwingAngle(float angle, float cutoff)
    {
        return angle > cutoff ? angle - 360 : angle; 
    }

    public IEnumerator RotateAroundPoint(bool clockwise, Vector2 point, float deg, float speed)
    {
        rotatePoint = point;
        float cutoff = NormalAngle();
        while (ConvertSwingAngle(NormalAngle(), cutoff) > deg)
        {
            spinAroundSpeed = clockwise ? -speed : speed;
            yield return new WaitForEndOfFrame();
        }
        spinAroundSpeed = 0;
    }

    public static bool ArrivedToPoint(Vector2 initialPos, Vector2 currentPos, Vector2 newPos)
    {
        if (Mathf.Abs(currentPos.x - newPos.x) < 0.1f && Mathf.Abs(currentPos.y - newPos.y) < 0.1f) return true;
        if (initialPos.x < newPos.x)
        {
            if (initialPos.y < newPos.y)
            {
                return currentPos.x >= newPos.x && currentPos.y >= newPos.y;
            }
            else
            {
                return currentPos.x >= newPos.x && currentPos.y <= newPos.y;
            }
        }
        else
        {
            if (initialPos.y < newPos.y)
            {
                return currentPos.x <= newPos.x && currentPos.y >= newPos.y;
            }
            else
            {
                return currentPos.x <= newPos.x && currentPos.y <= newPos.y;
            }
        }
    }

}
