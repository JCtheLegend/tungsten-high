using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController: MonoBehaviour
{
    AudioSource audioSource;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        audioSource = GetComponent<AudioSource>();
    }
    public void PlayAudio(string clip)
    {
        audioSource.clip = Resources.Load<AudioClip>(clip);
        audioSource.Play();
    }
}
