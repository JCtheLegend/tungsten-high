using System.Collections;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[System.Serializable]
public class LoadedObject
{
    public int[] validScenes;
    public int[] validDays;
    public int[] validStages;

    public string type;
    public string fileName;
    public bool destroy = false;
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
    public int[][] vectors;
    public int speed;
    public int pause;
    public bool destroy = false;
    public bool facePlayer;
    public CutsceneAction.TalkMeterInfo talkMeter;
}

public class ObjectLoader : MonoBehaviour
{
    public static void LoadObject(LoadedObject obj)
    {
        if (obj.type == "DisableExit")
        {
            GameObject.Find(obj.name).GetComponent<BoxCollider2D>().enabled = false;
        }
        else if (obj.type == "EnableExit")
        {
            GameObject.Find(obj.name).GetComponent<BoxCollider2D>().enabled = true;
        }
        else if (obj.type == "Pickup")
        {
            PickupManager.LoadPickup(obj.name);
        }
        else if (obj.type == "UnlockDoor")
        {
            GameObject.Find(obj.name).GetComponent<RoomExit>().locked = false;
        }
        else
        {
            GameObject g = Resources.Load<GameObject>("LoadPrefabs/" + obj.type);
            GameObject gameObject = Instantiate(g, new Vector2(float.Parse(obj.location.Split(',')[0]), float.Parse(obj.location.Split(',')[1])), Quaternion.identity);
            gameObject.name = obj.name;
            if (obj.facing != null)
            {
                gameObject.GetComponent<Faces>().initialFacing = Direction.ParseDirection(obj.facing);
            }
            if (obj.type == "CutsceneCollider" || obj.type == "CutsceneActTrigger")
            {
                gameObject.GetComponent<CutsceneTrigger>().cutsceneFileName = obj.fileName;
                gameObject.GetComponent<CutsceneTrigger>().destroy = obj.destroy;
                gameObject.GetComponent<BoxCollider2D>().size = new Vector2(int.Parse(obj.size.Split(',')[0]), int.Parse(obj.size.Split(',')[1]));
            }
            if (obj.assets != null)
            {
                foreach (LoadObjAsset asset in obj.assets)
                {
                    if (asset.type == "SimpleDialogTrigger")
                    {
                        SimpleDialogTrigger s = gameObject.AddComponent<SimpleDialogTrigger>();
                        s.dialogSet = asset.dialog;
                        s.isTyped = asset.isTyped;
                        s.facePlayer = asset.facePlayer;
                        s.speaker = obj.name;
                        if (asset.talkMeter != null && asset.talkMeter.speed != 0 && asset.talkMeter.range != 0)
                        {
                            s.hasTalkMeter = true;
                            s.talkMeter = asset.talkMeter;
                        }
                        gameObject.layer = 7;
                        gameObject.tag = "SimpleDialogTrigger";
                    }
                    else if (asset.type == "CutsceneCollider")
                    {
                        CutsceneTrigger ct = gameObject.AddComponent<CutsceneTrigger>();
                        ct.facePlayer = asset.facePlayer;
                        ct.destroy = asset.destroy;
                        ct.cutsceneFileName = asset.fileName;
                        gameObject.layer = 7;
                        gameObject.tag = "CollideCutsceneTrigger";
                    }
                    else if (asset.type == "CutsceneActTrigger")
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

    public static void EditObject(LoadedObject obj)
    {
        GameObject gameObject = GameObject.Find(obj.name);
        if(obj.facing != null)
        {
            gameObject.GetComponent<Faces>().Face(Direction.ParseDirection(obj.facing));
        }
        if(obj.location != null)
        {
            gameObject.transform.position = new Vector2(float.Parse(obj.location.Split(',')[0]), float.Parse(obj.location.Split(',')[1]));
        }
        if (obj.type == "CutsceneCollider" || obj.type == "CutsceneActTrigger")
        {
            gameObject.GetComponent<CutsceneTrigger>().cutsceneFileName = obj.fileName;
            if (obj.size != null)
            {
                gameObject.GetComponent<BoxCollider2D>().size = new Vector2(int.Parse(obj.size.Split(',')[0]), int.Parse(obj.size.Split(',')[1]));
            }
        }
        if (obj.type == "DisableExit")
        {
            GameObject.Find(obj.name).GetComponent<BoxCollider2D>().enabled = false;
        }
        if (obj.type == "EnableExit")
        {
            GameObject.Find(obj.name).GetComponent<BoxCollider2D>().enabled = true;
        }
        if (obj.assets != null)
        {
            foreach (LoadObjAsset asset in obj.assets)
            {
                if (asset.type == "SimpleDialogTrigger")
                {
                    SimpleDialogTrigger s;
                    if(gameObject.GetComponent<SimpleDialogTrigger>() != null)
                    {
                        s = gameObject.GetComponent<SimpleDialogTrigger>();
                    }
                    else
                    {
                        s = gameObject.AddComponent<SimpleDialogTrigger>();
                    }
                    s.dialogSet = asset.dialog;
                    s.isTyped = asset.isTyped;
                    s.facePlayer = asset.facePlayer;
                    s.speaker = obj.name;
                    if (asset.talkMeter != null && asset.talkMeter.speed != 0 && asset.talkMeter.range != 0)
                    {
                        s.hasTalkMeter = true;
                        s.talkMeter = asset.talkMeter;
                    }
                    gameObject.layer = 7;
                    gameObject.tag = "SimpleDialogTrigger";
                }
                else if (asset.type == "CutsceneTrigger")
                {
                    CutsceneTrigger ct;
                    if (gameObject.GetComponent<CutsceneTrigger>() != null)
                    {
                        ct = gameObject.GetComponent<CutsceneTrigger>();
                    }
                    else
                    {
                        ct = gameObject.AddComponent<CutsceneTrigger>();
                    }
                    ct.facePlayer = asset.facePlayer;
                    ct.destroy = asset.destroy;
                    ct.cutsceneFileName = asset.fileName;
                    gameObject.layer = 7;
                    gameObject.tag = "CutsceneTrigger";
                }
                else if (asset.type == "CutsceneActTrigger")
                {
                    CutsceneTrigger ct;
                    if (gameObject.GetComponent<CutsceneTrigger>() != null)
                    {
                        ct = gameObject.GetComponent<CutsceneTrigger>();
                    }
                    else
                    {
                        ct = gameObject.AddComponent<CutsceneTrigger>();
                    }
                    ct.facePlayer = asset.facePlayer;
                    ct.destroy = asset.destroy;
                    ct.cutsceneFileName = asset.fileName;
                    gameObject.layer = 7;
                    gameObject.tag = "CutsceneTrigger";
                }
                if (obj.type == "AutoMove")
                {
                    AutoMove a;
                    if (gameObject.GetComponent<AutoMove>() != null)
                    {
                        a = gameObject.GetComponent<AutoMove>();
                    }
                    else
                    {
                        a = gameObject.AddComponent<AutoMove>();
                    }
                    a.speed = asset.speed;
                    a.pause = asset.pause;
                    List<Vector2> vs = new List<Vector2>();
                    foreach(int[] list in asset.vectors)
                    {
                        vs.Add(new Vector2(list[0], list[1]));
                    }
                    a.moveCoords = vs;
                }
            }
        }
    }
}
