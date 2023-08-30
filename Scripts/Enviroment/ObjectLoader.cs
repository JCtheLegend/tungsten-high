using System.Collections;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

[System.Serializable]
public class LoadedObject
{
    public int minSceneCounter;
    public int maxSceneCounter;
    public int minGameCounter;
    public int maxGameCounter;

    public string type;
    public string name;
    public string location;
    public string size;
    public LoadObjAsset[] assets;
    public string facing;
}

[System.Serializable]
public class LoadObjAsset
{
    public string type;
    public string fileName;
    public string[] dialog;
    public bool[] isTyped;
    public bool destroy;
    public bool facePlayer;
}

public class ObjectLoader : MonoBehaviour
{
    public static void LoadObject(LoadedObject obj)
    {
        GameObject g = Resources.Load<GameObject>("LoadPrefabs/" + obj.type);
        GameObject gameObject = Instantiate(g, new Vector2(float.Parse(obj.location.Split(',')[0]), float.Parse(obj.location.Split(',')[1])), Quaternion.identity);
        gameObject.name = obj.name;
        if (obj.facing != null)
        {
            gameObject.GetComponent<Faces>().initialFacing = Direction.ParseDirection(obj.facing);
        }
        if (obj.type == "CutsceneCollider")
        {
            gameObject.GetComponent<CutsceneTrigger>().cutsceneFileName = obj.name;
            gameObject.GetComponent<BoxCollider2D>().size = new Vector2(int.Parse(obj.size.Split(',')[0]), int.Parse(obj.size.Split(',')[1]));
        }
        if (obj.assets != null) { 
            foreach (LoadObjAsset asset in obj.assets)
            {
                if (asset.type == "SimpleDialogTrigger")
                {
                    SimpleDialogTrigger s = gameObject.AddComponent<SimpleDialogTrigger>();
                    s.dialogSet = asset.dialog;
                    s.isTyped = asset.isTyped;
                    s.facePlayer = asset.facePlayer;
                    gameObject.layer = 7;
                    gameObject.tag = "SimpleDialogTrigger";
                }
                else if (asset.type == "CutsceneTrigger")
                {
                    CutsceneTrigger ct = gameObject.AddComponent<CutsceneTrigger>();
                    ct.facePlayer = asset.facePlayer;
                    ct.destroy = asset.destroy;
                    ct.cutsceneFileName = asset.fileName;
                    gameObject.layer = 7;
                    gameObject.tag = "CutsceneTrigger";
                }
            }
        }
    }
}
