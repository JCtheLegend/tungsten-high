using System.Collections;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] string dataFile;
    LoadedObject[] loadObjects;
    [SerializeField] int debugSceneCounter = 0;
    [SerializeField] int debugDayCounter = 0;
    [SerializeField] stage debugStageCounter = 0;
    [SerializeField] CutsceneManager cutsceneManager;
    [SerializeField] List<string> invItems;
    [SerializeField] List<int> invCount;
    string startCutscene = "";
    public GameObject music;

    [Header("Scene fade-in")]
    [Tooltip("Fade in from white instead of black. Tints the BlackFade sprite before the fade starts.")]
    [SerializeField] bool fadeFromWhite = false;
    [Tooltip("Speed of the automatic fade-in when the scene loads.")]
    [SerializeField] float fadeSpeed = 1;

    // The automatic fade-in coroutine. A cutscene that wants to hold the screen covered while it sets
    // up (startBlack / startWhite / fillFade) cancels it via CancelAutoFade().
    Coroutine autoFade;

    private void Awake()
    {
        // Cover the screen before anything else runs, so the scene is always revealed by the fade-in
        // below rather than popping in. Done in Awake so a start cutscene sees the correct state.
        CoverScreen();
        // Debug.Log("Game Counter: " + GameManager.dayCounter);
        // Debug.Log("Scene Counter: " + GameManager.sceneCounter);
        if (GameManager.startedInClient)
        {
            GameManager.dayCounter = debugDayCounter;
            GameManager.sceneCounter = debugSceneCounter;
            GameManager.stageCounter = debugStageCounter;
            for(int i = 0; i < invItems.Count(); i++)
            {
                PlayerInventory.UpdateInventory(invItems[i], invCount[i]);
            }
            PickupManager.CreatePickup("Quarter");
        }
        Debug.Log("Stage: " + GameManager.stageCounter);
        Debug.Log("Scene: " + GameManager.sceneCounter);
        if(GameObject.Find("Music Manager") == null)
        {
            GameObject m = Instantiate(music);
            m.name = "Music Manager";
        }
        loadObjects = ParseDataFile();
        if (loadObjects != null)
        {
            foreach (LoadedObject l in loadObjects)
            {
                if (l.validDays != null && l.validDays.Length > 1)
                {
                    if(GameManager.dayCounter < l.validDays[0] || GameManager.dayCounter > l.validDays[1])
                    {
                        continue;
                    }
                }
                if(l.validStages != null && l.validStages.Length > 1)
                {
                    if ((int)GameManager.stageCounter < l.validStages[0] || (int)GameManager.stageCounter > l.validStages[1])
                    {
                        continue;
                    }
                }
                if (l.validScenes != null && l.validScenes.Length > 1)
                {
                    if (GameManager.sceneCounter < l.validScenes[0] || GameManager.sceneCounter > l.validScenes[1])
                    {
                        continue;
                    }
                }
                LoadObject(l);
            }
        }
    }

    private void Start()
    {
        // Every scene fades in automatically — cutscenes no longer need a leading fade step. The fade
        // is started before the cutscene so a cutscene opening with startBlack/startWhite/fillFade can
        // cancel it and keep the screen covered while it teleports the player, moves the camera, etc.
        SpriteRenderer fade = GetFadeSprite();
        if (fade != null)
        {
            autoFade = StartCoroutine(GameManager.FadeOut(fade, fadeSpeed));
        }
        if (startCutscene != "")
        {
            cutsceneManager.BeginCutscene(startCutscene);
        }
    }

    static SpriteRenderer GetFadeSprite()
    {
        GameObject g = GameObject.Find("BlackFade");
        return g != null ? g.GetComponent<SpriteRenderer>() : null;
    }

    // Tint the fade sprite (white or black) and make it fully opaque so the scene starts hidden.
    void CoverScreen()
    {
        SpriteRenderer fade = GetFadeSprite();
        if (fade == null)
        {
            return;
        }
        float v = fadeFromWhite ? 1 : 0;
        fade.color = new Color(v, v, v, 1);
    }

    // Stop the automatic fade-in. Called by cutscene actions that take over the fade themselves.
    public void CancelAutoFade()
    {
        if (autoFade != null)
        {
            StopCoroutine(autoFade);
            autoFade = null;
        }
    }

    LoadedObject[] ParseDataFile()
    {
        if(dataFile != null && dataFile != "")
        {
            Debug.Log("Loading data file: " + "Scenes/" + GameManager.dayCounter + "/" + dataFile);
            TextAsset t = Resources.Load<TextAsset>("Scenes/" + GameManager.dayCounter + "/" + dataFile);
            if (t != null)
            {
                return JsonHelper.FromJson<LoadedObject>(t.text);
            }
        }
        return null;
    }

    void LoadObject(LoadedObject obj)
    {
        if(obj.type == "cutscene")
        {
            startCutscene = obj.name;
        }
        else if (obj.type == "disable")
        {
            GameObject.Find(obj.name).SetActive(false);
        }
        else {
            ObjectLoader.LoadObject(obj);
        }
    }
}
