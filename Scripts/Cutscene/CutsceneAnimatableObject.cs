using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CutsceneAnimatableObject : AnimatableObject
{
    protected CutsceneManager cut;
    private new void Awake()
    {
        base.Awake();
        cut = GameObject.Find("Cutscene Manager").GetComponent<CutsceneManager>();
    }
    public IEnumerator CutsceneAnimate(string animation, bool isContinue)
    {
        Animate(animation, isContinue);
        if (isContinue)
        {
            cut.setAnimateDone();
        }
        else
        {
            yield return new WaitForSeconds(anim.runtimeAnimatorController.animationClips.First(a => a.name == animation).length);
            cut.setAnimateDone();
        }
    }
}
