using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupController : MonoBehaviour
{
    public GameObject parent;
    public bool holdingObject;
    public bool isObjectInRange;
    public GameObject pickupObject;
    public GameObject objectInRange;
    public Vector2 holdPoint;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void PickupObject()
    {
        pickupObject = objectInRange;
        holdingObject = true;
        pickupObject.transform.parent = this.gameObject.transform;
        pickupObject.transform.localPosition = holdPoint;
    }

    private void Update()
    {
        this.transform.position = parent.transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Pickup"))
        {
            isObjectInRange = true;
            objectInRange = collision.gameObject;          
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Pickup"))
        {
            isObjectInRange = false;
            objectInRange = null;
        }
    }
}
