using System.Collections;
using UnityEngine;

// The multiple-choice quiz minigame: title card, Ready-Set-Go, timed questions, score screen.
//
// One scene runs every quiz in the game. Which quiz appears is decided by where the player is in the
// story -- QuizLoader turns the progression counters into a file under Resources/Quizzes/ -- so the
// same scene serves the PR test on Day 2, the Science pop quiz on Day 5, and anything added later,
// without a second scene or a new cutscene verb.
//
// Scene setup: this component plus a QuizView and a QuizTutorial, alongside the usual furniture a
// scene needs -- "Cutscene Manager" (for the dialog box), "BlackFade", "Music Manager", "Sound Manager".
//
// A cutscene enters the quiz with `loadScene`, and the quiz leaves through `onFinishCutscene` or
// `nextScene` in its JSON, so the story stays in charge on both sides.
public class QuizController : MonoBehaviour
{
    [SerializeField] QuizView view;
    [SerializeField] QuizTutorial tutorial;

    [Header("Quiz selection")]
    [Tooltip("Path under Resources/Quizzes/ to force a particular quiz, e.g. \"Chapter1/5.Sci\". " +
             "Only used when playing this scene directly in the editor; blank in a real playthrough, " +
             "where the quiz comes from the day/stage counters.")]
    [SerializeField] string debugQuizFile;

    [Header("Pacing")]
    [Tooltip("Seconds the title card stays up before the countdown.")]
    [SerializeField] float titleSeconds = 2f;
    [Tooltip("Seconds each of \"Ready\" / \"Set\" / \"Go!\" holds.")]
    [SerializeField] float countdownStep = 0.7f;
    [Tooltip("Seconds the correct answer stays revealed before the next question.")]
    [SerializeField] float feedbackSeconds = 1.6f;
    [Tooltip("Seconds the score screen spends counting the score up.")]
    [SerializeField] float scoreCountSeconds = 1.2f;
    [SerializeField] float fadeSpeed = 1f;

    [Header("Sounds (Resources/Sounds/)")]
    [Tooltip("Played when the selection moves between answers.")]
    [SerializeField] string moveSound = "beep";
    [SerializeField] string correctSound = "";
    [SerializeField] string wrongSound = "";
    [Tooltip("Ticks once a second while the clock is in its final stretch.")]
    [SerializeField] string tickSound = "";
    [SerializeField] string resultsSound = "Well Done CCBY3";

    QuizDefinition quiz;
    CutsceneManager cutscene;
    SoundController sound;

    int score;
    int correctCount;
    int streak;
    int bestStreak;

    void Awake()
    {
        cutscene = CutsceneManager.Instance;
        sound = GetComponent<SoundController>();
        // startedInClient is the editor-play flag: it is true until the first GameManager.LoadScene,
        // so it marks "this scene was opened directly" and is when debugQuizFile applies.
        quiz = QuizLoader.Load(GameManager.startedInClient ? debugQuizFile : null);
        if (tutorial != null)
        {
            tutorial.Setup(quiz, cutscene, view);
        }
    }

    void Start()
    {
        if (view == null || tutorial == null)
        {
            Debug.LogError("QuizController needs its QuizView and QuizTutorial wired up in the scene.");
            return;
        }
        if (quiz == null)
        {
            // QuizLoader has already logged which paths it tried. Leave the screen as it is rather
            // than dropping the player into a blank quiz they cannot finish.
            return;
        }
        StartCoroutine(RunQuiz());
    }

    IEnumerator RunQuiz()
    {
        QuizResults.Clear();
        if (!string.IsNullOrEmpty(quiz.song))
        {
            if (MusicController.Exists)
            {
                MusicController.Instance.ChangeSong(quiz.song);
            }
        }

        yield return StartCoroutine(FadeFromBlack());

        // ---- Beginning: title card, then whatever the teacher has to say --------------------
        view.ShowTitle(quiz.title, quiz.subtitle);
        yield return new WaitForSeconds(titleSeconds);
        yield return StartCoroutine(tutorial.Say(quiz.intro));
        yield return StartCoroutine(tutorial.Play("intro"));
        view.HideTitle();

        // ---- Ready, set, go ----------------------------------------------------------------
        // The board is up behind the countdown so the player can see the shape of it, but the first
        // question is not revealed until "Go!" clears.
        view.ShowBoard(true);
        yield return StartCoroutine(Countdown());

        // ---- The questions -----------------------------------------------------------------
        for (int i = 0; i < quiz.questions.Length; i++)
        {
            yield return StartCoroutine(tutorial.Play("beforeQuestion", i));
            yield return StartCoroutine(AskQuestion(quiz.questions[i], i));
            yield return StartCoroutine(tutorial.Play("afterAnswer", i));
        }

        // ---- Score screen ------------------------------------------------------------------
        yield return StartCoroutine(tutorial.Play("beforeResults"));
        yield return StartCoroutine(ShowResults());

        Finish();
    }

    IEnumerator Countdown()
    {
        string[] beats = { "Ready", "Set", "Go!" };
        foreach (string beat in beats)
        {
            view.SetBanner(beat);
            PlaySound(moveSound);
            yield return new WaitForSeconds(countdownStep);
        }
        view.SetBanner("");
    }

    // Put one question up, run its clock, take the answer, then reveal. Returns once the reveal has
    // been on screen long enough to read.
    IEnumerator AskQuestion(QuizQuestion q, int index)
    {
        int selected = 0;
        float total = q.seconds;
        float remaining = total;
        int lastTickSecond = Mathf.CeilToInt(remaining);
        int chosen = -1;

        view.ShowQuestion(q, index + 1, quiz.questions.Length, score);
        view.SetSelection(selected);
        view.SetTimer(remaining, total);

        while (remaining > 0 && chosen < 0)
        {
            direction moved;
            if (TryReadDirection(out moved))
            {
                int next = view.Navigate(selected, moved);
                if (next != selected)
                {
                    selected = next;
                    view.SetSelection(selected);
                    PlaySound(moveSound);
                }
            }
            if (Input.GetKeyDown(KeyCode.Z))
            {
                chosen = selected;
                break;
            }

            remaining -= Time.deltaTime;
            view.SetTimer(remaining, total);
            // One tick per whole second, and only once the bar has gone red.
            int second = Mathf.CeilToInt(Mathf.Max(remaining, 0));
            if (second != lastTickSecond)
            {
                lastTickSecond = second;
                if (view.IsPanicking(remaining, total))
                {
                    PlaySound(tickSound);
                }
            }
            yield return null;
        }

        bool ranOut = chosen < 0;
        bool right = !ranOut && chosen == q.answer;

        if (right)
        {
            // Answering early is worth more: the bonus scales with the fraction of the clock left.
            int bonus = Mathf.RoundToInt(quiz.speedBonus * Mathf.Clamp01(remaining / total));
            score += quiz.pointsPerQuestion + bonus;
            correctCount++;
            streak++;
            bestStreak = Mathf.Max(bestStreak, streak);
        }
        else
        {
            streak = 0;
        }

        if (ranOut)
        {
            view.SetTimer(0, total);
            view.SetBanner("Time!");
        }
        view.RevealAnswer(q.answer, chosen, right ? q.explainRight : q.explainWrong);
        view.SetRunningScore(score);
        PlaySound(right ? correctSound : wrongSound);

        yield return new WaitForSeconds(feedbackSeconds);
        view.SetBanner("");
    }

    // Arrow keys, read as a single direction per frame. Vertical wins on a diagonal, matching how the
    // rest of the game resolves simultaneous presses.
    static bool TryReadDirection(out direction dir)
    {
        if (Input.GetKeyDown(KeyCode.UpArrow)) { dir = direction.up; return true; }
        if (Input.GetKeyDown(KeyCode.DownArrow)) { dir = direction.down; return true; }
        if (Input.GetKeyDown(KeyCode.LeftArrow)) { dir = direction.left; return true; }
        if (Input.GetKeyDown(KeyCode.RightArrow)) { dir = direction.right; return true; }
        dir = direction.down;
        return false;
    }

    IEnumerator ShowResults()
    {
        int maxScore = quiz.MaxScore();

        QuizResults.hasResult = true;
        QuizResults.title = quiz.title;
        QuizResults.score = score;
        QuizResults.maxScore = maxScore;
        QuizResults.correct = correctCount;
        QuizResults.total = quiz.questions.Length;
        QuizResults.bestStreak = bestStreak;
        QuizResults.passed = QuizResults.Percent() >= quiz.passPercent;

        view.ShowResults(quiz.title, score, maxScore, correctCount, quiz.questions.Length,
                         bestStreak, QuizResults.Grade(), QuizResults.passed);
        PlaySound(resultsSound);

        // Roll the score up rather than snapping to it.
        float elapsed = 0;
        while (elapsed < scoreCountSeconds)
        {
            elapsed += Time.deltaTime;
            view.SetResultsScore(Mathf.RoundToInt(Mathf.Lerp(0, score, elapsed / scoreCountSeconds)));
            yield return null;
        }
        view.SetResultsScore(score);

        yield return StartCoroutine(tutorial.Say(quiz.outro));

        view.ShowContinuePrompt();
        // Ignore a Z that is still held from the last answer or from advancing the outro dialog.
        while (Input.GetKey(KeyCode.Z))
        {
            yield return null;
        }
        while (!Input.GetKeyDown(KeyCode.Z))
        {
            yield return null;
        }
    }

    // Record the outcome where the rest of the game can branch on it, then hand back to the story.
    void Finish()
    {
        string flag = QuizResults.passed ? quiz.conditionOnPass : quiz.conditionOnFail;
        if (!string.IsNullOrEmpty(flag) && !PlayerInventory.condition.Contains(flag))
        {
            PlayerInventory.condition.Add(flag);
        }
        if (QuizResults.passed && !string.IsNullOrEmpty(quiz.rewardItem) && quiz.rewardAmount != 0)
        {
            PlayerInventory.UpdateInventory(quiz.rewardItem, quiz.rewardAmount);
        }
        tutorial.MarkSeen();

        // Move the story on before leaving, so the scene we return to starts the beat that follows
        // the quiz rather than the one that sent us here.
        if (quiz.sceneCounterOnFinish >= 0)
        {
            GameManager.sceneCounter = quiz.sceneCounterOnFinish;
        }
        GameManager.SaveGameData();

        if (!string.IsNullOrEmpty(quiz.onFinishCutscene) && cutscene != null)
        {
            view.HideAll();
            cutscene.BeginCutscene(quiz.onFinishCutscene);
        }
        else if (!string.IsNullOrEmpty(quiz.nextScene))
        {
            StartCoroutine(LeaveTo(quiz.nextScene));
        }
        else
        {
            Debug.LogWarning("Quiz \"" + quiz.title + "\" has neither nextScene nor onFinishCutscene; " +
                             "the player is stuck on the score screen.");
        }
    }

    IEnumerator LeaveTo(string sceneName)
    {
        SpriteRenderer fade = FindFade();
        if (fade != null)
        {
            StartCoroutine(GameManager.FadeIn(fade, fadeSpeed));
            while (fade.color.a < 1)
            {
                yield return null;
            }
        }
        GameManager.LoadScene(sceneName);
    }

    // The quiz owns its own fade-in, the way a fight does; there is no SceneLoader in this scene.
    IEnumerator FadeFromBlack()
    {
        SpriteRenderer fade = FindFade();
        if (fade == null)
        {
            yield break;
        }
        fade.color = new Color(0, 0, 0, 1);
        yield return StartCoroutine(GameManager.FadeOut(fade, fadeSpeed));
    }

    static SpriteRenderer FindFade()
    {
        GameObject g = GameObject.Find("BlackFade");
        return g != null ? g.GetComponent<SpriteRenderer>() : null;
    }

    void PlaySound(string clip)
    {
        if (sound != null && !string.IsNullOrEmpty(clip))
        {
            sound.PlayAudio(clip);
        }
    }
}
