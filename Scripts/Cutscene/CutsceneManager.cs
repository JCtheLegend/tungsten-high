using System.Collections;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;
using Cinemachine;

public class CutsceneManager : MonoBehaviour
{
    PlayerMasterController player;
    GridMover gridPlayer;
    private DialogPrinter printer;
    AudioSource audioSource;
    public PlayableDirector director;
    public CinemachineVirtualCamera mainCamera;
    public EnemyController enemy;
    public bool inChoice = false;
    internal int choiceSize = 2;
    internal int choiceNum = 0;
    internal bool inDialog = false;
    internal bool timelineDone = false;
    internal bool finishDialog = false;
    internal bool actionDone = false;
    internal bool animateDone = false;
    public bool inCutscene;
    public GameObject talkMeterObject;
    private int i;
    [SerializeField]
    GameObject dialogBox;
    [SerializeField]
    GameObject dreamEntrance;
    public bool isPuzzle;
    IEnumerator currentCutscene;
    private void Awake()
    {
        player = GameObject.Find("Player")?.GetComponent<PlayerMasterController>();
        gridPlayer = GameObject.Find("Grid Player")?.GetComponent<GridMover>();
        printer = GetComponent<DialogPrinter>();
        audioSource = GetComponent<AudioSource>();
        dialogBox.SetActive(false);
    }

    public void StopCutscene(){
        StopAllCoroutines();
    }

    public void BeginCutscene(string filename)
    {
        Debug.Log("starting cutscene " + filename);
        CutsceneAction[] c = CreateCutsceneFromTextFile(filename);
        List<CutsceneAction> actions = c.ToList();
        StartCoroutine(HandleCutscene(actions));
    }

    private void Update()
    {
        if (inChoice)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                AlterChoice(false);
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                AlterChoice(true);
            }
        }
        if (Input.GetKeyDown(KeyCode.Z) && inDialog)
        {
            AdvanceDialog();
        }
        
    }

    public IEnumerator HandleCutscene(List<CutsceneAction> inActions)
    {
        List<CutsceneAction> actions = new List<CutsceneAction>(inActions);
        inCutscene = true;
        for(i = 0; i < actions.Count; i++) {
            CutsceneAction a = actions[i];
            actionDone = false;
            timelineDone = false;
            animateDone = false;
            Debug.Log(a.type);
            switch (a.type) {
                case "disableMovement":
                    if (player != null && player.isActiveAndEnabled)
                    {
                        player.movement.rb.velocity = Vector2.zero;
                        player.anim.inCutscene = true;
                        player.anim.FaceDir();
                        yield return new WaitForSeconds(0.1f);
                        player.input.DisableInput();
                        player.GetComponent<CutsceneMoveableObject>().enabled = true;
                        player.GetComponent<PlayerMovementController>().enabled = false;
                        //player.GetComponent<BoxCollider2D>().enabled = false;
                    }
                    else if (gridPlayer != null && gridPlayer.isActiveAndEnabled)
                    {  
                        gridPlayer.moveVelocity = Vector2.zero;
                        gridPlayer.isMoving = false;
                        gridPlayer.DisableInput();
                        gridPlayer.GetComponent<CutsceneMoveableObject>().enabled = true;
                    }
                    else if (isPuzzle)
                    {
                        GameObject.Find("Hero Cell").GetComponent<HeroCell>().DisableMovement();
                    }                 
                    actionDone = true;
                    break;
                case "timeline":
                    StartCoroutine(HandleTimeline(a));
                    break;
                case "dreamExit":
                    StartCoroutine(HandleDreamExit());
                    break;
                case "fillFade":
                    SpriteRenderer st = GameObject.Find("BlackFade").GetComponent<SpriteRenderer>();
                    st.color = new Color(st.color.r, st.color.b, st.color.g, 1);
                    actionDone = true;
                    break;
                case "clearFade":
                    SpriteRenderer sp = GameObject.Find("BlackFade").GetComponent<SpriteRenderer>();
                    sp.color = new Color(sp.color.r, sp.color.b, sp.color.g, 0);
                    actionDone = true;
                    break;
                case "setBlack":
                    SpriteRenderer sr = GameObject.Find("BlackFade").GetComponent<SpriteRenderer>();
                    sr.color = new Color(0, 0, 0, sr.color.a);
                    actionDone = true;
                    break;
                case "setWhite":
                    SpriteRenderer si = GameObject.Find("BlackFade").GetComponent<SpriteRenderer>();
                    si.color = new Color(1, 1, 1, si.color.a);
                    actionDone = true;
                    break;
                case "addCondition":
                    PlayerInventory.condition.Add(a.name);
                    actionDone = true;
                    break;
                case "removeCondition":
                    PlayerInventory.condition.Remove(a.name);
                    actionDone = true;
                    break;
                case "checkCondition":
                    if (PlayerInventory.condition.Contains(a.name))
                    {
                        i += a.skips[0];
                    }
                    else
                    {
                        i += a.skips[1];
                    }
                    actionDone = true;
                    break;
                case "checkInventory":
                    if(PlayerInventory.CheckInventory(a.name) >= int.Parse(a.text))
                    {
                        i += a.skips[0];
                    }
                    else
                    {
                        i += a.skips[1];
                    }
                    actionDone = true;
                    break;
                case "cameraFollow":
                    mainCamera.m_Follow = GameObject.Find(a.name).transform;
                    actionDone = true;
                    break;
                case "switchCombat":
                    player.gameObject.SetActive(false);
                    gridPlayer.gameObject.SetActive(true);
                    actionDone = true;
                    break;
                case "switchNormal":
                    player.gameObject.SetActive(true);
                    gridPlayer.gameObject.SetActive(false);
                    actionDone = true;
                    break;
                case "destroy":
                    foreach (GameObject g in GameObject.Find(a.name).GetComponents<GameObject>())
                    {
                        Destroy(g);
                    }
                    break;
                case "color":
                    GameObject.Find(a.name).GetComponent<SpriteRenderer>().color = new Color(float.Parse(a.text.Split(',')[0]), float.Parse(a.text.Split(',')[1]), float.Parse(a.text.Split(',')[2]), float.Parse(a.text.Split(',')[3]));
                    actionDone = true;
                    break;
                case "moveCamera":
                    a.name = "CM vcam1";
                    mainCamera.m_Follow = null;
                    StartCoroutine(HandleMove(a));
                    break;
                case "setCameraBounds":
                    mainCamera.GetComponent<CinemachineConfiner>().m_BoundingShape2D = GameObject.Find(a.name).GetComponent<Collider2D>();
                    actionDone = true;
                    break;
                case "animate":
                    animateDone = false;
                    StartCoroutine(HandleAnimate(a));
                    break;
                case "disable":
                    Debug.Log(a.name);
                    GameObject.Find(a.name).SetActive(false);
                    actionDone = true;
                    break;
                case "enableComponents":
                    foreach (MonoBehaviour child in GameObject.Find(a.name).GetComponents<MonoBehaviour>())
                    {
                        child.enabled = true;
                    }
                    actionDone = true;
                    break;
                case "disableComponents":
                    foreach (MonoBehaviour child in GameObject.Find(a.name).GetComponents<MonoBehaviour>())
                    {
                        child.enabled = false;
                    }
                    actionDone = true;
                    break;
                case "enableChildren":
                    foreach(Transform child in GameObject.Find(a.name).transform)
                    {
                        child.gameObject.SetActive(true);
                    }
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
                case "startFight":
                    foreach (SpriteRenderer s in enemy.enemyHealthBar.GetComponentsInChildren<SpriteRenderer>())
                    {
                        StartCoroutine(GameManager.FadeIn(s, 10));
                    }
                    foreach (SpriteRenderer s in gridPlayer.GetComponent<PlayerCombatController>().playerHealthBar.GetComponentsInChildren<SpriteRenderer>())
                    {
                        StartCoroutine(GameManager.FadeIn(s, 10));
                    }
                    StartCoroutine(enemy.StartFight());
                    actionDone = true;
                    break;
                case "teleport":
                    Teleport(a);
                    actionDone = true;
                    break;
                case "move":
                    StartCoroutine(HandleMove(a));
                    break;
                case "pickup":
                    StartCoroutine(Pickup(a));
                   
                    actionDone = true;
                    break;
                case "addFollow":
                    AddFollow(a);
                    actionDone = true;
                    break;
                case "removeFollow":
                    RemoveFollow(a);
                    actionDone = true;
                    break;
                case "moveSet":
                    StartCoroutine(HandleMoveSet(a));
                    break;
                case "rotate":
                    StartCoroutine(HandleRotate(a));
                    break;
                case "moveFace":
                    MoveFace(a);
                    actionDone = true;
                    break;
                case "dialogAndChoice":
                case "choice":
                    inChoice = true;
                    StartCoroutine(HandleDialog(a));
                    break;
                case "dialog":
                    StartCoroutine(HandleDialog(a));
                    break;
                case "closeDialog":
                    inDialog = false;
                    dialogBox.SetActive(false);
                    actionDone = true;
                    break;
                case "goto":
                    i = int.Parse(a.text) - 1;
                    actionDone = true;
                    break;
                case "enableMovement":
                    if (player)
                    {
                        player.input.EnableInput();
                        player.anim.inCutscene = false;
                        player.GetComponent<CutsceneMoveableObject>().enabled = false;
                        player.GetComponent<PlayerMovementController>().enabled = true;
                        player.GetComponent<BoxCollider2D>().enabled = true;
                    }
                    else if (gridPlayer)
                    {
                        gridPlayer.GetComponent<CutsceneMoveableObject>().enabled = false;
                        gridPlayer.EnableInput();
                    }
                    else if (isPuzzle)
                    {
                        GameObject.Find("Hero Cell").GetComponent<HeroCell>().inputEnabled = true;
                    }
                  
                    actionDone = true;
                    break;
                case "fadeIn":
                    SpriteRenderer[] fadeInSprites = GameObject.Find(a.name).GetComponentsInChildren<SpriteRenderer>();
                    foreach(SpriteRenderer spritey in fadeInSprites)
                    {
                        StartCoroutine(FadeIn(spritey));
                    }
                    break;
                case "fadeOut":
                    SpriteRenderer[] fadeOutSprites = GameObject.Find(a.name).GetComponentsInChildren<SpriteRenderer>();
                    foreach (SpriteRenderer spritey in fadeOutSprites)
                    {
                        StartCoroutine(FadeOut(spritey));
                    }
                    break;
                case "wait":
                    yield return new WaitForSeconds(float.Parse(a.text));
                    actionDone = true;
                    break;
                case "setSceneCounter":
                    GameManager.sceneCounter = int.Parse(a.text);
                    Debug.Log("Scene Counter: " + a.text);
                    actionDone = true;
                    break;
                case "setStageCounter":
                    SetStageCounter(a.text);
                    Debug.Log("Stage Counter: " + GameManager.stageCounter);
                    actionDone = true;
                    break;
                case "setDayCounter":
                    GameManager.dayCounter = int.Parse(a.text);
                    Debug.Log("Game Counter: " + GameManager.dayCounter);
                    actionDone = true;
                    break;
                case "talkMeter":
                    if (a.talkMeter != null)
                    {
                        bool success = false;
                        GameObject g = Instantiate(talkMeterObject, player.transform.position + new Vector3(-1, 1, 0), Quaternion.identity);
                        TalkMeter talkMeter = g.GetComponent<TalkMeter>();
                        talkMeter.speed = a.talkMeter.speed;
                        talkMeter.range = a.talkMeter.range;
                        talkMeter.StartTalkMeter();
                        while (g != null)
                        {
                            success = talkMeter.InZone();
                            yield return new WaitForSeconds(0.1f);
                        }
                        if (!success)
                        {
                            GameObject.Find(a.name).GetComponent<SimpleDialogTrigger>().failedTalk = !success;
                            i = actions.Count;
                            player.input.EnableInput();
                            player.anim.inCutscene = false;
                            player.GetComponent<CutsceneMoveableObject>().enabled = false;
                            player.GetComponent<PlayerMovementController>().enabled = true;
                            player.GetComponent<BoxCollider2D>().enabled = true;
                            inCutscene = false;
                        }
                        else
                        {
                            Debug.Log(a.name);
                            GameObject.Find(a.name).GetComponent<SimpleDialogTrigger>().successTalk = success;
                        }
                        actionDone = true;
                    }
                    break;
                case "loadObjects":
                    LoadedObject[] loadObjs = a.loadObjs;
                    foreach (LoadedObject l in loadObjs)
                    {
                        ObjectLoader.LoadObject(l);
                    }
                    actionDone = true;
                    break;
                case "editObjects":
                    LoadedObject[] editObjs = a.loadObjs;
                    foreach (LoadedObject l in editObjs)
                    {
                        ObjectLoader.EditObject(l);
                    }
                    actionDone = true;
                    break;
                case "loadScene":
                    GameManager.RoomData.toEntranceNum = 0;
                    GameManager.LoadScene(a.name);
                    break;
                case "changeCostume":
                    player.anim.costume = a.text;
                    actionDone = true;
                    break;
                case "changeSong":
                    GameObject.Find("Music Manager").GetComponent<MusicController>().ChangeSong(a.name);
                    actionDone = true;
                    break;
                case "stopMusic":
                    StartCoroutine(GameObject.Find("Music Manager").GetComponent<MusicController>().FadeOut(1));
                    actionDone = true;
                    break;
                case "playSound":
                    audioSource.Stop();
                    audioSource.pitch = 1;
                    audioSource.clip = Resources.Load<AudioClip>("Sounds/" + a.name);
                    audioSource.Play();
                    actionDone = true;
                    break;
                case "unlockDoor":
                    GameObject.Find(a.name).GetComponent<RoomExit>().locked = false;
                    actionDone = true;
                    break;
                case "updateInventory":
                    PlayerInventory.UpdateInventory(a.name, int.Parse(a.text));
                    actionDone = true;
                    break;
                default:
                    Debug.Log("INVALID CUTSCENE ACTION");
                    actionDone = true;
                    break;
            }
            while (!actionDone)
            {
                yield return new WaitForEndOfFrame();
            }
            inChoice = false;
            if (a.skip > 0)
            {
                i += a.skip;
            }
        }
        inCutscene = false;
    }

    public void MoveFace(CutsceneAction a)
    {
        if (a.name == "Player")
        {
            switch (a.direction)
            {
                case "down":
                    player.anim.ChangeAnimationState("default_forward", 0);
                    break;
                case "up":
                    player.anim.ChangeAnimationState("default_back", 0);
                    break;
                case "right":
                    player.anim.ChangeAnimationState("default_side", 0);
                    break;
            }
        }
        else
        {
            Faces f = GameObject.Find(a.name).GetComponent<Faces>();
            if (f.gameObject.GetComponent<AnimatableObject>() != null)
            {
                f.gameObject.GetComponent<AnimatableObject>().StopAnimation();
            }
            f.Face(Direction.ParseDirection(a.direction));
        }
    }

    void SetStageCounter(string s)
    {
        switch (s)
        {
            case "Pre":
                GameManager.stageCounter = stage.pre;
                break;
            case "PR":
                GameManager.stageCounter = stage.pr;
                break;
            case "Gym":
                GameManager.stageCounter = stage.gym;
                break;
            case "Lunch":
                GameManager.stageCounter = stage.lunch;
                break;
            case "Psych":
                GameManager.stageCounter = stage.psych;
                break;
            case "Sci":
                GameManager.stageCounter = stage.sci;
                break;
            case "Post":
                GameManager.stageCounter = stage.post;
                break;
            case "Dream":
                GameManager.stageCounter = stage.dream;
                break;
            default:
                Debug.LogError("invalid stage");
                break;
        }
        GameManager.sceneCounter = 0;
    }

    public void Teleport(CutsceneAction a)
    {
        Vector2 coords = new Vector2(float.Parse(a.text.Split(',')[0]), float.Parse(a.text.Split(',')[1]));
        GameObject.Find(a.name).transform.position = coords;
        if(a.name == "Hero Cell")
        {
            GameObject.Find(a.name).GetComponent<HeroCell>().spawnPoint = GameObject.Find(a.name).transform.position;
        }
    }

    public CutsceneAction[] CreateCutsceneFromTextFile(string fileName)
    {
        TextAsset t = Resources.Load<TextAsset>("Cutscenes/"+GameManager.dayCounter+"/"+fileName);
        return JsonHelper.FromJson<CutsceneAction>(t.text);
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
        i += a.skips[choiceNum];
    }

    void AddFollow(CutsceneAction a)
    {
        GameObject.Find(a.text).GetComponent<FollowLeader>().enabled = true;
        FollowLeader f = GameObject.Find(a.text).GetComponent<FollowLeader>();
        f.SetLeader(a.name);
        f.NewFollower();
    }

    void RemoveFollow(CutsceneAction a)
    {
        FollowLeader f = GameObject.Find(a.text).GetComponent<FollowLeader>();
        f.RemoveFollower();
        GameObject.Find(a.text).GetComponent<FollowLeader>().enabled = false;
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

    IEnumerator HandleDreamExit()
    {
        dreamEntrance.SetActive(true);
        StartCoroutine(dreamEntrance.GetComponent<DreamEntrance>().AnimateEnter());
        yield return new WaitForSeconds(5);
        actionDone = true;
    }

    IEnumerator HandleMoveSet(CutsceneAction a)
    {
        if (a.isContinue) {
            actionDone = true;
        }
        Ref<bool> shouldContinue = new Ref<bool>(false);
        for(int p = 0; p < a.moveSet.Length; p++)
        {
            CutsceneAction move = a.moveSet[p];
            switch (move.type)
            {
            case "animate":
                animateDone = false;
                StartCoroutine(HandleAnimate(move));
                break;
            case "disable":
                GameObject.Find(move.name).SetActive(false);
                actionDone = true;
                break;
            case "disableCollider":
                GameObject.Find(move.name).GetComponent<Collider2D>().enabled = false;
                break;
            case "enableCollider":
                GameObject.Find(move.name).GetComponent<Collider2D>().enabled = true;
                break;
            case "hide":
                GameObject.Find(move.name).GetComponent<SpriteRenderer>().enabled = false;
                break;
            case "unhide":
                GameObject.Find(move.name).GetComponent<SpriteRenderer>().enabled = true;
                break;
            case "teleport":
                Teleport(move);
                break;
            case "wait":
                yield return new WaitForSeconds(float.Parse(move.text));
                break;
            case "moveFace":
                MoveFace(move);
                break;
            case "goto":
                p = int.Parse(move.text) - 1;
                break;
            default:
                StartCoroutine(HandleMoveSetMove(move, shouldContinue));
                while (!shouldContinue.Value)
                {
                    yield return new WaitForEndOfFrame();
                }
                break;
            } 
            shouldContinue.Value = false;
        }
        if (!a.isContinue)
        {
            actionDone = true;
        }
    }

    IEnumerator HandleMoveSetMove(CutsceneAction a, Ref<bool> shouldContinue)
    {
        CutsceneMoveableObject m = GameObject.Find(a.name).GetComponent<CutsceneMoveableObject>();
        Vector2 coords = new Vector2(float.Parse(a.text.Split(',')[0]), float.Parse(a.text.Split(',')[1]));
        Ref<bool> isDone = new Ref<bool>(false);
        if (m.gameObject.GetComponent<CutsceneAnimatableObject>() != null && !a.animateOverride)
        {
            m.gameObject.GetComponent<CutsceneAnimatableObject>().AnimateMove(Direction.ParseDirection(a.direction));
        }
        if (m.gameObject.name == "Player")
        {
            m.gameObject.GetComponent<PlayerAnimationController>().CutsceneAnimateMove(Direction.ParseDirection(a.direction), a.animateOverride);
        }
        StartCoroutine(m.CutsceneMove(coords, a.speed, false, isDone, Direction.ParseDirection(a.direction), a.isTyped));
        while (!isDone.Value)
        {
            yield return new WaitForEndOfFrame();
        }
        shouldContinue.Value = true;
    }

    IEnumerator HandleMove(CutsceneAction a)
    {
        CutsceneMoveableObject m = GameObject.Find(a.name).GetComponent<CutsceneMoveableObject>();
        CutsceneAnimatableObject anim = GameObject.Find(a.name).GetComponent<CutsceneAnimatableObject>();
        Vector2 coords = new Vector2(float.Parse(a.text.Split(',')[0]), float.Parse(a.text.Split(',')[1]));
        Ref<bool> isDone = new Ref<bool>(false);
        if(anim != null && !a.animateOverride)
        {
           anim.AnimateMove(Direction.ParseDirection(a.direction));
        }
        if(m.gameObject.name == "Player")
        {
            m.gameObject.GetComponent<PlayerAnimationController>().CutsceneAnimateMove(Direction.ParseDirection(a.direction), a.animateOverride);
        }
        StartCoroutine(m.CutsceneMove(coords, a.speed, a.isContinue, isDone, Direction.ParseDirection(a.direction), a.isTyped));
        if(!a.isContinue)
        {
            while (!isDone.Value)
            {
                yield return new WaitForEndOfFrame();
            }
        }
        actionDone = true;
    }

    IEnumerator HandleRotate(CutsceneAction a)
    {
        CutsceneMoveableObject m = GameObject.Find(a.name).GetComponent<CutsceneMoveableObject>();
        Ref<bool> isDone = new Ref<bool>(false);
        StartCoroutine(m.CutsceneRotate(Direction.ParseDirection(a.direction) == direction.right, float.Parse(a.text), a.speed, a.isContinue, isDone));
        while (!isDone.Value)
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

    public void setAnimateDone()
    {
        animateDone = true;
    }

    IEnumerator HandleDialog(CutsceneAction a)
    {
        inDialog = true;
        if (a.name != null && a.name != "" && GameObject.Find(a.name) != null)
        {
            if (GameObject.Find(a.name).transform.position.y > Camera.main.transform.position.y)
            {
                dialogBox.transform.localPosition = new Vector2(0, -7);
            }
            else
            {
                dialogBox.transform.localPosition = new Vector2(0, 0);
            }
        }
        else if (player != null && player.transform.position.y > Camera.main.transform.position.y)
        {
            dialogBox.transform.localPosition = new Vector2(0, -7);
        }
        else {
            //dialogBox.transform.localPosition = new Vector2(0, 0);
        }
        dialogBox.SetActive(true);
        StartCoroutine(printer.PrintText(a));
        while (!finishDialog)
        {
            yield return new WaitForSeconds(0.1f);
        }
        finishDialog = false;
        if (inChoice)
        {
            HandleChoice(a);
        }
        printer.ClearDialog();
        actionDone = true;
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

    public IEnumerator FadeIn(SpriteRenderer s, int fadeSpeed = 1)
    {
        StartCoroutine(GameManager.FadeIn(s, fadeSpeed));
        while (s.color.a < 1)
        {
            yield return new WaitForSeconds(0.1f);
        }
        actionDone = true;
    }

    public IEnumerator FadeOut(SpriteRenderer s, int fadeSpeed = 1)
    {
        StartCoroutine(GameManager.FadeOut(s, fadeSpeed));
        while(s.color.a > 0)
        {
            yield return new WaitForSeconds(0.1f);
        }
        actionDone = true;
    }

    IEnumerator Pickup(CutsceneAction a)
    {
        StartCoroutine(player.anim.CutsceneAnimate("get_item", true));
        Vector2 playerCoords = GameObject.Find("Player").transform.position;
        GameObject.Find(a.name).transform.position = new Vector2(playerCoords.x, playerCoords.y + 1);
        GameObject.Find(a.name).GetComponent<SpriteRenderer>().sortingLayerName = "Midground";
        GameObject.Find(a.name).GetComponent<SpriteRenderer>().sortingOrder = 30000;
        yield return new WaitForSeconds(2);
        GameObject.Find(a.name).GetComponent<SpriteRenderer>().enabled = false;
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
    public float speed;
    public bool animateOverride = false;
    public List<string> choices;
    public int skip = 0;
    public List<int> skips;
    public bool isTyped;
    public bool isContinue = false;
    public LoadedObject[] loadObjs;
    public CutsceneAction[] moveSet;
    public TalkMeterInfo talkMeter;

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
    public CutsceneAction(string type, TalkMeterInfo talkMeter, string name)
    {
        this.type = type;
        this.talkMeter = talkMeter;
        this.name = name;
    }

    [System.Serializable]
    public class TalkMeterInfo
    {
        public float range;
        public float speed;
    }
}