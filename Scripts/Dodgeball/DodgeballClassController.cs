using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeballClassController : MonoBehaviour
{
    CutsceneManager cutscene;
    public string[] cutscenes;
    public DodgeballTarget[] targets;

    public DodgeballTarget[] targets2;
    public DodgeballTarget[] targets3;
    public int stage = 0;

    public GameObject dodgeballPrefab;
    // Start is called before the first frame update
    void Start()
    {
        cutscene = GameObject.Find("Cutscene Manager").GetComponent<CutsceneManager>();       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    bool CheckTargets()
    {
        if (stage == 0)
        {
            foreach (DodgeballTarget d in targets)
            {
                if (!d.hit) return false;
            }
            return true;
        }
        else if (stage == 1)
        {
            foreach (DodgeballTarget d in targets2)
            {
                if (!d.hit) return false;
            }
            return true;
        }
        else if (stage == 2)
        {
            foreach (DodgeballTarget d in targets3)
            {
                if (!d.hit) return false;
            }
            return true;
        }
        return false;
    }

    void SpawnBall()
    {
        GameObject d = Instantiate(dodgeballPrefab, new Vector2(10, 0), Quaternion.identity);
        d.GetComponent<ArcJump>().point[0] = new Vector2(11.5f, 0);
        d.GetComponent<ArcJump>().point[1] = new Vector2(15, 5);
        d.GetComponent<ArcJump>().point[2] = new Vector2(20, 0);
        d.GetComponent<ArcJump>().jumping = true;
        d.name = "Dodgeball";
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Ally Dodgeball")
        {
            if (CheckTargets())
            {               
                cutscene.BeginCutscene(cutscenes[stage]);
                stage++;
            }
            if (stage < 2)
            {
                SpawnBall();
            }
            if (stage == 3)
            {
                SpawnBall();
            }
        }
    }
}
