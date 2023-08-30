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
    [SerializeField] int debugGameCounter = 0;
    [SerializeField] CutsceneManager cutsceneManager;

    string startCutscene = "";

    private void Awake()
    {
        Debug.Log("Game Counter: " + GameManager.gameCounter);
        Debug.Log("Scene Counter: " + GameManager.sceneCounter);
        if (GameManager.debugCounterToggle)
        {
            GameManager.gameCounter = debugGameCounter;
            GameManager.sceneCounter = debugSceneCounter;
        }
        loadObjects = ParseDataFile();
        if (loadObjects != null)
        {
            foreach (LoadedObject l in loadObjects)
            {
                if (GameManager.sceneCounter < l.maxSceneCounter && GameManager.sceneCounter >= l.minSceneCounter && GameManager.gameCounter < l.maxGameCounter && GameManager.gameCounter >= l.minGameCounter)
                {
                    LoadObject(l);
                }
            }
        }
    }

    private void Start()
    {
        if (startCutscene != "")
        {
            cutsceneManager.BeginCutscene(startCutscene);
        }
    }

    LoadedObject[] ParseDataFile()
    {
        TextAsset t = Resources.Load<TextAsset>("Scenes/"+dataFile);
        return JsonHelper.FromJson<LoadedObject>(t.text);
    }

    void LoadObject(LoadedObject obj)
    {
        if(obj.type == "cutscene")
        {
            startCutscene = obj.name;
        }
        else {
            ObjectLoader.LoadObject(obj);
        }
    }



    void ManageAsset(GameObject obj, string s, LoadedObject l)
    {       
    }
}
