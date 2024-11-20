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
    public bool hasTalkMeter = false;
    public CutsceneAction.TalkMeterInfo talkMeter = null;
    public bool failedTalk = false;
    public bool successTalk = false;

    public List<CutsceneAction> CreateSimpleDialog()
    {
        List<CutsceneAction> l = new List<CutsceneAction>
        {
            new CutsceneAction("disableMovement")
        };
        if (failedTalk)
        {
            l.Add(TooAwkward());
        }
        else
        {
            if (hasTalkMeter && !successTalk)
            {
                l.Add(new CutsceneAction("talkMeter", talkMeter, gameObject.name));
            }
            for (int i = 0; i < dialogSet.Length; i++)
            {
                CutsceneAction c;
                if (speakerSprite == null) speakerSprite = "";
                string s = speaker;
                if (!isTyped[i])
                {
                    s = null;
                }
                c = new CutsceneAction(speakerSprite, s, dialogSet[i], isTyped[i]);
                l.Add(c);
            }
        }
        l.Add(new CutsceneAction("closeDialog"));
        l.Add(new CutsceneAction("enableMovement"));
        return l;
    }

    CutsceneAction TooAwkward()
    {
        return new CutsceneAction("", null, "You already messed up trying to have a conversation. It would be too awkward to try again.", true);
    }
}
