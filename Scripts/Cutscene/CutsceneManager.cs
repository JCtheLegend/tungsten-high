using System.Collections;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class CutsceneManager : MonoBehaviour
{
    public PlayerMasterController player;
    private DialogPrinter printer;
    public PlayableDirector director;
    public Image fadeBlack;
    internal bool inChoice = false;
    internal int choiceSize = 2;
    internal int choiceNum = 0;
    internal bool inDialog = false;
    internal bool timelineDone = false;
    internal bool finishDialog = false;
    internal bool actionDone = false;
    internal bool moveDone = false;
    internal bool animateDone = false;
    [SerializeField]
    GameObject dialogBox;
    private void Awake()
    {
        printer = GetComponent<DialogPrinter>();
        dialogBox.SetActive(false);
    }

    public void BeginCutscene(string filename)
    {
        CutsceneAction[] c = CreateCutsceneFromTextFile(filename);
        List<CutsceneAction> actions = c.ToList();
        StartCoroutine(HandleCutscene(actions));
    }

    public IEnumerator HandleCutscene(List<CutsceneAction> actions)
    {
        foreach (CutsceneAction a in actions) {
            actionDone = false;
            timelineDone = false;
            switch (a.type) {
                case "disableMovement":
                    player.movement.rb.velocity = Vector2.zero;
                    player.input.DisableInput();
                    player.GetComponent<CutsceneMoveableObject>().enabled = true;
                    player.GetComponent<PlayerMovementController>().enabled = false;
                    actionDone = true;
                    break;
                case "timeline":
                    StartCoroutine(HandleTimeline(a));
                    break;
                case "animate":
                    animateDone = false;
                    StartCoroutine(HandleAnimate(a));
                    break;
                case "disable":
                    GameObject.Find(a.name).SetActive(false);
                    actionDone = true;
                    break;
                case "disableCollider":
                    GameObject.Find(a.name).GetComponent<Collider2D>().enabled = false;
                    actionDone = true;
                    break;
                case "enableCollider":
                    GameObject.Find(a.name).GetComponent<Collider2D>().enabled = true;
                    actionDone = true;
                    break;
                case "hide":
                    GameObject.Find(a.name).GetComponent<SpriteRenderer>().enabled = false;
                    actionDone = true;
                    break;
                case "unhide":
                    GameObject.Find(a.name).GetComponent<SpriteRenderer>().enabled = true;
                    actionDone = true;
                    break;
                case "move":
                    moveDone = false;
                    StartCoroutine(HandleMove(a));
                    break;
                case "rotate":
                    moveDone = false;
                    StartCoroutine(HandleRotate(a));
                    break;
                case "moveFace":
                    Faces f = GameObject.Find(a.name).GetComponent<Faces>();
                    if(f.gameObject.GetComponent<AnimatableObject>() != null)
                    {
                        f.gameObject.GetComponent<AnimatableObject>().StopAnimation();
                    }
                    f.Face(Direction.ParseDirection(a.direction));
                    actionDone = true;
                    break;
                case "dialog":
                    StartCoroutine(HandleDialog(a));
                    break;
                case "closeDialog":
                    dialogBox.SetActive(false);
                    actionDone = true;
                    break;
                case "enableMovement":
                    player.input.EnableInput();
                    player.GetComponent<CutsceneMoveableObject>().enabled = false;
                    player.GetComponent<PlayerMovementController>().enabled = true;
                    actionDone = true;
                    break;
                case "fadeIn":
                    StartCoroutine(FadeIn(GameObject.Find(a.name).GetComponent<SpriteRenderer>()));
                    break;
                case "fadeOut":
                    StartCoroutine(FadeOut(GameObject.Find(a.name).GetComponent<SpriteRenderer>()));
                    break;
                case "wait":
                    yield return new WaitForSeconds(int.Parse(a.text));
                    actionDone = true;
                    break;
                case "setSceneCounter":
                    GameManager.sceneCounter = int.Parse(a.text);
                    Debug.Log("Scene Counter: " + GameManager.sceneCounter);
                    actionDone = true;
                    break;
                case "setGameCounter":
                    GameManager.gameCounter = int.Parse(a.text);
                    Debug.Log("Game Counter: " + GameManager.gameCounter);
                    actionDone = true;
                    break;
                case "loadObjects":
                    LoadedObject[] loadObjs = a.loadObjs;
                    foreach (LoadedObject l in loadObjs)
                    {
                        ObjectLoader.LoadObject(l);
                    }
                    actionDone = true;
                    break;
                case "loadScene":
                    GameManager.LoadScene(a.text);
                    break;
                case "changeCostume":
                    if (a.text == "pjs")
                    {
                        player.anim.inPjs = true;
                    }
                    actionDone = true;
                    break;
            }
            while (!actionDone)
            {
                yield return new WaitForEndOfFrame();
            }
        }
    }

    public CutsceneAction[] CreateCutsceneFromTextFile(string fileName)
    {
        TextAsset t = Resources.Load<TextAsset>("Cutscenes/"+fileName);
        return JsonHelper.FromJson<CutsceneAction>(t.text);
    }

    List<(string, int)> ParseChoice(string[] choiceData)
    {
        var newChoiceData = new List<(string, int)>();
        if (choiceData.Length == 1) return newChoiceData;
        for (int i = 0; i < choiceData.Length; i += 2)
        {
            newChoiceData.Add((choiceData[i], int.Parse(choiceData[i + 1])));
        }
        return newChoiceData;
    }

    public void AlterChoice(bool next)
    {
        if (next)
        {
            choiceNum++;
        }
        else
        {
            choiceNum--;
        }
        choiceNum %= choiceSize;
        if (choiceNum == 0)
        {
            printer.choice1.fontStyle = TMPro.FontStyles.Underline;
            printer.choice2.fontStyle = TMPro.FontStyles.Normal;
        }
        else
        {
            printer.choice2.fontStyle = TMPro.FontStyles.Underline;
            printer.choice1.fontStyle = TMPro.FontStyles.Normal;
        }
    }

    void HandleChoice(CutsceneAction a)
    {

    }

    IEnumerator HandleAnimate(CutsceneAction a)
    {
        if (a.name == "Player")
        {
            StartCoroutine(player.anim.CutsceneAnimate(a.text, a.isContinue));
        }
        else
        {
            CutsceneAnimatableObject am = GameObject.Find(a.name).GetComponent<CutsceneAnimatableObject>();
            StartCoroutine(am.CutsceneAnimate(a.text, a.isContinue));
        }
        while (!animateDone)
        {
            yield return new WaitForEndOfFrame();
        }
        actionDone = true;
    }

    IEnumerator HandleMove(CutsceneAction a)
    {
        CutsceneMoveableObject m = GameObject.Find(a.name).GetComponent<CutsceneMoveableObject>();
        Vector2 coords = new Vector2(float.Parse(a.text.Split(',')[0]), float.Parse(a.text.Split(',')[1]));
        if(m.gameObject.GetComponent<CutsceneAnimatableObject>() != null)
        {
            m.gameObject.GetComponent<CutsceneAnimatableObject>().AnimateMove(Direction.ParseDirection(a.direction));
        }
        StartCoroutine(m.CutsceneMove(coords, a.speed, a.isContinue));
        if(!a.isContinue)
        {
            while (!moveDone)
            {
                yield return new WaitForEndOfFrame();
            }
        }
        actionDone = true;
    }


    IEnumerator HandleRotate(CutsceneAction a)
    {
        CutsceneMoveableObject m = GameObject.Find(a.name).GetComponent<CutsceneMoveableObject>();
        StartCoroutine(m.CutsceneRotate(Direction.ParseDirection(a.direction) == direction.right, float.Parse(a.text), a.speed, a.isContinue));
        while (!moveDone)
        {
            yield return new WaitForEndOfFrame();
        }
        actionDone = true;
    }


    IEnumerator HandleTimeline(CutsceneAction a)
    {
        director.playableAsset = Resources.Load<PlayableAsset>(a.text);
        director.Play();
        actionDone = true;
        while (!timelineDone)
        {
            yield return new WaitForEndOfFrame();
        }
    }

    public void setTimelineDone()
    {
        timelineDone = true;
    }

    public void setMovementDone()
    {
        moveDone = true;
    }

    public void setAnimateDone()
    {
        animateDone = true;
    }

    IEnumerator HandleDialog(CutsceneAction a)
    {
        inDialog = true;
        dialogBox.SetActive(true);
        StartCoroutine(printer.PrintText(a));
        while (!finishDialog)
        {
            yield return new WaitForSeconds(0.1f);
        }
        finishDialog = false;
        printer.ClearDialog();
        actionDone = true;
        inDialog = false;
    }

    public void AdvanceDialog()
    {
        if (printer.textDone)
        {
            finishDialog = true;
        }
        else
        {
            printer.advanceText = true;
        }
    }

    IEnumerator FadeIn(SpriteRenderer s, int fadeSpeed = 1)
    {
        StartCoroutine(GameManager.FadeIn(s, fadeSpeed));
        while (s.color.a < 1)
        {
            yield return new WaitForSeconds(0.1f);
        }
        actionDone = true;
    }

    IEnumerator FadeOut(SpriteRenderer s, int fadeSpeed = 1)
    {
        StartCoroutine(GameManager.FadeOut(s, fadeSpeed));
        while(s.color.a > 0)
        {
            yield return new WaitForSeconds(0.1f);
        }
        actionDone = true;
    }
}

public class Cutscene
{
    public string title;
    public CutsceneAction[] cutsceneActions;
}


[System.Serializable]
public class CutsceneAction
{
    public string type;
    public string name;
    public string sprite;
    public string direction;
    public string text;
    public int speed;
    public List<string> choices;
    public List<int> skips;
    public bool isTyped;
    public bool isContinue;
    public LoadedObject[] loadObjs;

    public CutsceneAction(string sprite, string speaker, string text, bool isTyped)
    {
        type = "dialog";
        this.sprite = sprite;
        this.text = text;
        this.isTyped = isTyped;
        this.name = speaker;
    }
    public CutsceneAction(string type)
    {
        this.type = type;
    }
}