using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicController : MonoBehaviour
{
    internal AudioSource audioSource;
    private RoomManager room;
    private void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        if (GameObject.Find("Room Manager") != null)
        {
            room = GameObject.Find("Room Manager").GetComponent<RoomManager>();
            if (room.songName != null && room.songName != "")
            {
                if (audioSource.clip == null || audioSource.clip.name != room.songName)
                {
                    ChangeSong(room.songName);
                }
            }
        }
    }

    private void Update()
    {
        if (!audioSource.isPlaying && audioSource.clip != null)
        {
            audioSource.Play();
        }
    }


public void ChangeSong(string s)
    {
        AudioClip a = Resources.Load<AudioClip>("Music/" + s);
        audioSource.clip = a;
        PlayMusic();
    }

    public void PlayMusic()
    {
        if (audioSource.isPlaying) return;
       audioSource.Play();
    }

    public void StopMusic()
    {
        audioSource.Stop();
    }


    public IEnumerator FadeOut(float FadeTime)
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;

            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
    }
}
