using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController: MonoBehaviour
{
    AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void PlayAudio(string clip)
    {
        audioSource.clip = Resources.Load<AudioClip>("Sounds/" + clip);
        audioSource.Play();
    }

    public void StopAudio()
    {
        audioSource.Stop();
    }
}
