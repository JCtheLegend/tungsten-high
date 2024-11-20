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

    private void Awake()
    {
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
        if (startCutscene != "")
        {
            cutsceneManager.BeginCutscene(startCutscene);
        }
        else
        {
            SpriteRenderer s = GameObject.Find("BlackFade").GetComponent<SpriteRenderer>();
            StartCoroutine(GameManager.FadeOut(s, 1));
        }
    }

    LoadedObject[] ParseDataFile()
    {
        if(dataFile != null && dataFile != "")
        {
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
