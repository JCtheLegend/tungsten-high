using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Menu : MonoBehaviour
{
    public Vector2[] selectorPos;
    public enum MenuState { pressZ, newGameSelect, continueSelect };

    public GameObject pressZ;
    public GameObject newGameSelect;
    public GameObject continueSelect;
    public GameObject selector;

    public GameObject chalk;
    public GameObject eraser;
    public GameObject music;

    MusicController m;

    public MenuState state;

    public bool inputDisabled = true;

    public PlayableDirector director;
    [SerializeField] PlayableAsset[] p;
    [SerializeField] SpriteRenderer blackFade;

    AudioSource a;

    public AudioClip chalkNoise;
    public AudioClip eraserNoise;

    // Update is called once per frame

    private void Start()
    {
        a = GetComponent<AudioSource>();
        if (GameObject.Find("Music Manager(Clone)") == null)
        {
            m = Instantiate(music).GetComponent<MusicController>();
            m.ChangeSong("School");
        }
        inputDisabled = true;
        StartCoroutine(MenuIntro());
    }

    IEnumerator MenuIntro()
    {
        yield return new WaitForSeconds(1);
        director.playableAsset = p[0];
        director.Play();
        yield return new WaitForSeconds(3);
        director.playableAsset = p[1];
        director.Play();
        yield return new WaitForSeconds(3);
        inputDisabled = false;
    }

    void Update()
    {
        if (!inputDisabled)
        {
            if (state == MenuState.pressZ)
            {
                if (Input.GetKeyDown(KeyCode.Z))
                {
                    StartCoroutine(ProgressMenuPart1());
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.Z))
                {
                    StartCoroutine(ProgressMenuPart2());
                }
                if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
                {
                    ChangeSelection();
                }
            }
        }
    }

    void ChangeSelection()
    {
        if(state == MenuState.continueSelect)
        {
            state = MenuState.newGameSelect;
            selector.transform.position = selectorPos[0];
        }
        else
        {
            state = MenuState.continueSelect;
            selector.transform.position = selectorPos[1];
        }
    }

    IEnumerator ProgressMenuPart1()
    {
        inputDisabled = true;
        director.playableAsset = p[2];
        director.Play();
        yield return new WaitForSeconds(3);
        director.playableAsset = p[3];
        director.Play();
        yield return new WaitForSeconds(7);
        pressZ.SetActive(false);
        newGameSelect.SetActive(true);
        continueSelect.SetActive(true);
        selector.SetActive(true);
        selector.transform.position = selectorPos[0];
        state = MenuState.newGameSelect;
        inputDisabled = false;
    }

    IEnumerator ProgressMenuPart2()
    {
        inputDisabled = true;
        selector.SetActive(false);
        director.playableAsset = p[4];
        director.Play();
        yield return new WaitForSeconds(3);
        director.playableAsset = p[5];
        director.Play();
        yield return new WaitForSeconds(3);
        pressZ.SetActive(false);
        newGameSelect.SetActive(false);
        continueSelect.SetActive(false);
        StartCoroutine(GameManager.FadeIn(blackFade, 3));
        yield return new WaitForSeconds(3);
        GameObject.Find("Music Manager(Clone)").GetComponent<MusicController>().StopMusic();
        if (state == MenuState.newGameSelect)
        {
            GameManager.LoadScene("80s 303");
        }
        else
        {
            GameManager.LoadGameData();
            GameManager.LoadScene(GameManager.gameData.room);
        }

    }
}
