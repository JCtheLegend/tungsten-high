using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceController : MonoBehaviour
{
    public Hurdle[] hurdles;
    int lapCounter = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        hurdles = GetComponentsInChildren<Hurdle>();
        Debug.Log("ENABLED");
        foreach(Hurdle h in hurdles)
        {
            h.StartHurdles();
        }
    }
}
