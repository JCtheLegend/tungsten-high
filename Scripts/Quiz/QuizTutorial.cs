using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The teaching pass on the player's first quiz.
//
// It does not draw anything of its own: each step is turned into an inline cutscene and handed to
// CutsceneManager.HandleCutscene, exactly the way SimpleDialogTrigger and PickupObject build their
// dialog. That means quiz tutorial text prints in the same box, with the same typing sound, the same
// speaker portrait and the same Z-to-advance as the rest of the game.
//
// Only dialog actions are emitted -- deliberately no disableMovement/enableMovement, since there is
// no Player or Grid Player object in a quiz scene for those verbs to act on.
public class QuizTutorial : MonoBehaviour
{
    // Set once the player has been walked through a quiz, so it only ever happens on the first one.
    // Lives in PlayerInventory.condition with the game's other global flags, so a cutscene can read
    // it with checkCondition. Note that PlayerInventory is NOT written to gameData.json today, so
    // this resets on a reload -- see the note on save coverage in CLAUDE.md.
    public const string SEEN_CONDITION = "quizTutorialDone";

    [Tooltip("Type the tutorial lines out character by character, like normal dialog.")]
    [SerializeField] bool typeLines = true;

    QuizDefinition quiz;
    CutsceneManager cutscene;
    QuizView view;
    bool active;

    public void Setup(QuizDefinition definition, CutsceneManager cutsceneManager, QuizView quizView)
    {
        quiz = definition;
        cutscene = cutsceneManager;
        view = quizView;
        // The tutorial runs when this quiz has steps and the player has not been taught yet. A quiz
        // can force it on with alwaysTutorial while it is being written.
        active = quiz != null
                 && quiz.tutorial != null
                 && quiz.tutorial.Length > 0
                 && (quiz.alwaysTutorial || !PlayerInventory.condition.Contains(SEEN_CONDITION));
    }

    public bool IsActive
    {
        get { return active; }
    }

    // Remember that the player has been taught. Called once the quiz is over, not when the first step
    // plays, so quitting halfway through still leaves the tutorial armed for next time.
    public void MarkSeen()
    {
        if (active && !PlayerInventory.condition.Contains(SEEN_CONDITION))
        {
            PlayerInventory.condition.Add(SEEN_CONDITION);
        }
    }

    // Run every step matching `trigger` (and `questionIndex`, where the trigger uses one) and return
    // when the player has clicked through all of them. Yields nothing when the tutorial is off, so
    // callers can await it unconditionally.
    public IEnumerator Play(string trigger, int questionIndex = -1)
    {
        if (!active || cutscene == null)
        {
            yield break;
        }
        foreach (QuizTutorialStep step in quiz.tutorial)
        {
            if (step.trigger != trigger)
            {
                continue;
            }
            if (questionIndex >= 0 && step.questionIndex != questionIndex)
            {
                continue;
            }
            yield return StartCoroutine(PlayStep(step));
        }
    }

    IEnumerator PlayStep(QuizTutorialStep step)
    {
        if (step.lines == null || step.lines.Length == 0)
        {
            yield break;
        }
        if (view != null)
        {
            view.SetHighlight(step.highlight);
        }
        // StartCoroutine runs the body up to its first yield, and HandleCutscene raises inCutscene
        // before it gets there -- so the flag is already up when this returns. Wait for it to come
        // back down rather than for a fixed time, so the player reads at their own pace.
        StartCoroutine(cutscene.HandleCutscene(BuildDialog(step.lines)));
        while (cutscene.inCutscene)
        {
            yield return null;
        }
        if (view != null)
        {
            view.SetHighlight("");
        }
    }

    List<CutsceneAction> BuildDialog(string[] lines)
    {
        string speaker = quiz != null && !string.IsNullOrEmpty(quiz.speaker) ? quiz.speaker : null;
        List<CutsceneAction> actions = new List<CutsceneAction>();
        foreach (string line in lines)
        {
            actions.Add(new CutsceneAction("", speaker, line, typeLines));
        }
        actions.Add(new CutsceneAction("closeDialog"));
        return actions;
    }

    // Same inline-dialog trick, for the quiz's own intro and outro lines. Static-ish helper kept here
    // so there is one place that knows how quiz text reaches the dialog box.
    public IEnumerator Say(string[] lines)
    {
        if (lines == null || lines.Length == 0 || cutscene == null)
        {
            yield break;
        }
        StartCoroutine(cutscene.HandleCutscene(BuildDialog(lines)));
        while (cutscene.inCutscene)
        {
            yield return null;
        }
    }
}
