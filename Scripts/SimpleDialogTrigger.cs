using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleDialogTrigger : MonoBehaviour
{
    public string speakerSprite;
    public string[] dialogSet;
    public bool[] isTyped;
    public string speaker;
    public bool facePlayer;

    public List<CutsceneAction> CreateSimpleDialog()
    {
        List<CutsceneAction> l = new List<CutsceneAction>();
        l.Add(new CutsceneAction("disableMovement"));
        for (int i = 0; i < dialogSet.Length; i++)
        {
            CutsceneAction c;
            if (speakerSprite == null) speakerSprite = "";
            if (speaker == null) speaker = "";
            c = new CutsceneAction(speakerSprite, speaker, dialogSet[i], isTyped[i]);
            l.Add(c);
        }
        l.Add(new CutsceneAction("closeDialog"));
        l.Add(new CutsceneAction("enableMovement"));
        return l;
    }
}
