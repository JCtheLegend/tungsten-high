using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class RaceController : MonoBehaviour
{
    public Hurdle[] hurdles;
    int lapCounter = 0;
    bool counting = false;
    public float timer = 0.0f;
    public int lapCount = 0;
    public TextMeshProUGUI timerText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (counting)
        {
            timer += Time.deltaTime;
            timerText.text = TimeSpan.FromSeconds((double)timer).ToString(@"mm\:ss");
        }
    }

    private void OnEnable()
    {
        counting = true;
        timerText.enabled = true;
        hurdles = GetComponentsInChildren<Hurdle>();
        foreach(Hurdle h in hurdles)
        {
            h.StartHurdles();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        lapCount++;
        if (lapCount >= 3)
        {
            counting = false;
            GameObject.Find("Cutscene Manager").GetComponent<CutsceneManager>().StopAllCoroutines();
            GameObject.Find("Cutscene Manager").GetComponent<CutsceneManager>().BeginCutscene("2.Gym.FinishRace");
        }
    }
}
