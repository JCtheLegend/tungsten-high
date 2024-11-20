using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeballMatchController : MonoBehaviour
{
    CutsceneManager cutscene;
    public string[] endCutscenes;
    public DodgeballOpponent[] opps;
    public DodgeballPlayer[] allies;

    // Start is called before the first frame update
    void Start()
    {
        cutscene = GameObject.Find("Cutscene Manager").GetComponent<CutsceneManager>();       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CheckGame()
    {
        if(CheckOpps())
        {
            cutscene.BeginCutscene(endCutscenes[0]);
        }
        else if (CheckAllies())
        {
            cutscene.BeginCutscene(endCutscenes[1]);
        }
    }

    bool CheckOpps()
    {
        foreach(DodgeballOpponent o in opps)
        {
            if (o.lives > 0) return false;
        }
        return true;
    }

    bool CheckAllies()
    {
        foreach (DodgeballPlayer p in allies)
        {
            if (p.lives > 0) return false;
        }
        return true;
    }
}
