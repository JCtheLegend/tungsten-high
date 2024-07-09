using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConversationTracker : MonoBehaviour
{
    public List<string> conversations;

    public int req;

    CutsceneTrigger cut;
    CutsceneManager manager;

    bool complete = false;

    private void Start()
    {
        cut = GetComponent<CutsceneTrigger>();
        manager = GameObject.Find("Cutscene Manager").GetComponent<CutsceneManager>();
        conversations.Add("Ms Nice");
    }
    // Update is called once per frame
    void Update()
    {
        if(conversations.Count >= req && !complete)
        {
            complete = true;
            req = int.MaxValue;
            StartCoroutine(StartCutscene());            
        }
    }

    IEnumerator StartCutscene()
    {
        while (manager.inCutscene)
        {
            yield return new WaitForSeconds(0.1f);
        }
        Debug.Log("try");
        manager.BeginCutscene(cut.cutsceneFileName);
        Destroy(this.gameObject);
    }

    public void AddConvo(string name)
    {
        if (!conversations.Contains(name))
        {
            conversations.Add(name);
        }
    }
}
