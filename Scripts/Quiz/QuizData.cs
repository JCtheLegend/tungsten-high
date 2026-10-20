using UnityEngine;

// Data model for the multiple-choice quiz minigame, deserialized straight from JSON.
//
// Unlike the scene/cutscene files there is no `Items` wrapper here: those are bare JSON *arrays*,
// which JsonUtility cannot read, while a quiz file is a single JSON *object* and needs no wrapper.
//
// One file per quiz, at Resources/Quizzes/Chapter<N>/<dayCounter>.<Stage>[.<sceneCounter>].json --
// the same coordinates cutscenes use. See QuizLoader.Resolve.
[System.Serializable]
public class QuizDefinition
{
    [Tooltip("Shown on the title card, e.g. \"Science of Superpowers\".")]
    public string title;
    [Tooltip("Second line of the title card, e.g. \"Pop Quiz\".")]
    public string subtitle;

    [Tooltip("Head sprite name for the intro/tutorial/outro dialog (Resources/Heads/Heads). " +
             "Blank means narration with no portrait.")]
    public string speaker;

    [Tooltip("Song to play for the quiz (Resources/Music/). Blank keeps whatever is already playing.")]
    public string song;

    [Tooltip("Seconds on the clock for each question. A question can override this.")]
    public float secondsPerQuestion;

    [Tooltip("Points for a correct answer, before the speed bonus.")]
    public int pointsPerQuestion;

    [Tooltip("Extra points for answering instantly, scaling down to 0 as the clock runs out.")]
    public int speedBonus;

    [Tooltip("Percentage of questions that must be answered correctly to pass. Deliberately measured " +
             "on answers, not points: the speed bonus is an arcade flourish and should not fail a " +
             "player who got everything right but took their time.")]
    public int passPercent;

    [Tooltip("Lines spoken before the Ready-Set-Go countdown.")]
    public string[] intro;

    [Tooltip("Lines spoken once the score screen is up.")]
    public string[] outro;

    [Tooltip("Scene to load when the player leaves the score screen -- normally the classroom the " +
             "quiz was taken in, so its scene JSON can start the follow-on beat.")]
    public string nextScene;

    [Tooltip("sceneCounter to set before leaving, which is how the destination scene knows to show " +
             "the beat AFTER the quiz rather than the one that started it. -1 leaves it alone.")]
    public int sceneCounterOnFinish;

    [Tooltip("Cutscene to run instead of loading nextScene. Only for a cutscene written to run in " +
             "the quiz scene itself -- it must end in loadScene or the player is stranded, since a " +
             "quiz scene has no Player object for enableMovement to hand control back to.")]
    public string onFinishCutscene;

    [Tooltip("PlayerInventory.condition flag added on a pass, so later cutscenes can checkCondition it.")]
    public string conditionOnPass;

    [Tooltip("PlayerInventory.condition flag added on a fail.")]
    public string conditionOnFail;

    [Tooltip("Inventory item awarded on a pass (e.g. \"Quarter\"). Blank for none.")]
    public string rewardItem;
    public int rewardAmount;

    [Tooltip("Play the tutorial even if the player has already seen one. For testing.")]
    public bool alwaysTutorial;

    public QuizTutorialStep[] tutorial;
    public QuizQuestion[] questions;

    // Highest score obtainable, used for the percentage and the letter grade. The speed bonus counts,
    // so a perfect paper answered slowly still lands short of 100% -- which is the point of the clock.
    public int MaxScore()
    {
        int perQuestion = Mathf.Max(pointsPerQuestion, 0) + Mathf.Max(speedBonus, 0);
        return perQuestion * (questions != null ? questions.Length : 0);
    }
}

[System.Serializable]
public class QuizQuestion
{
    public string prompt;

    [Tooltip("The four answers, in the order they appear on screen.")]
    public string[] options;

    [Tooltip("Index into options[] of the correct answer.")]
    public int answer;

    [Tooltip("Shown under the question after a correct answer. Optional.")]
    public string explainRight;

    [Tooltip("Shown under the question after a wrong answer or a timeout. Optional.")]
    public string explainWrong;

    [Tooltip("Overrides the quiz-wide clock for this question. 0 uses the quiz default.")]
    public float seconds;
}

// A pause in the quiz where the teacher explains a rule. Steps run in file order within a trigger.
[System.Serializable]
public class QuizTutorialStep
{
    [Tooltip("When this fires: \"intro\" (before the countdown), \"beforeQuestion\", " +
             "\"afterAnswer\", or \"beforeResults\".")]
    public string trigger;

    [Tooltip("For beforeQuestion/afterAnswer: which question index this attaches to.")]
    public int questionIndex;

    [Tooltip("Dialog lines, printed through the normal dialog box.")]
    public string[] lines;

    [Tooltip("Name of a QuizView highlight target to point at while this step is up: " +
             "\"options\", \"timer\", \"score\", or blank for none.")]
    public string highlight;
}

// Resolves and loads a quiz file. Kept apart from QuizController so a cutscene, an editor tool, or a
// test can ask "is there a quiz here?" without a scene running.
public static class QuizLoader
{
    public const float DEFAULT_SECONDS = 15f;
    public const int DEFAULT_POINTS = 100;
    public const int DEFAULT_SPEED_BONUS = 50;
    public const int DEFAULT_PASS_PERCENT = 60;

    // Where the quiz for the current point in the story lives. Tries the sceneCounter-qualified name
    // first, so one class can hold more than one quiz, then falls back to the plain day+stage name:
    //
    //   Quizzes/Chapter1/5.Sci.2.json   (Day 5, Science, sceneCounter 2)
    //   Quizzes/Chapter1/5.Sci.json     (Day 5, Science)
    public static string[] CandidatePaths()
    {
        string folder = "Quizzes/Chapter" + GameManager.chapterCounter + "/";
        string key = GameManager.dayCounter + "." + Stages.Name(GameManager.stageCounter);
        return new string[]
        {
            folder + key + "." + GameManager.sceneCounter,
            folder + key,
        };
    }

    // Load `explicitPath` if given, otherwise the quiz for the current progression counters.
    // Returns null (and logs) when there is nothing to load.
    public static QuizDefinition Load(string explicitPath)
    {
        if (!string.IsNullOrEmpty(explicitPath))
        {
            return LoadPath("Quizzes/" + explicitPath);
        }
        foreach (string path in CandidatePaths())
        {
            QuizDefinition found = LoadPath(path, false);
            if (found != null)
            {
                return found;
            }
        }
        Debug.LogError("No quiz found at any of: " + string.Join(", ", CandidatePaths()));
        return null;
    }

    static QuizDefinition LoadPath(string path, bool logMissing = true)
    {
        TextAsset t = Resources.Load<TextAsset>(path);
        if (t == null)
        {
            if (logMissing)
            {
                Debug.LogError("Quiz not found: " + path);
            }
            return null;
        }
        QuizDefinition quiz;
        try
        {
            quiz = JsonUtility.FromJson<QuizDefinition>(t.text);
        }
        catch
        {
            Debug.LogError("Malformed quiz JSON: " + path);
            return null;
        }
        if (quiz == null || quiz.questions == null || quiz.questions.Length == 0)
        {
            Debug.LogError("Quiz has no questions: " + path);
            return null;
        }
        ApplyDefaults(quiz);
        return quiz;
    }

    // Fill in the tuning values the author left out, and warn about questions that cannot be answered
    // correctly -- a bad `answer` index is the easiest mistake to make in one of these files.
    static void ApplyDefaults(QuizDefinition quiz)
    {
        if (quiz.secondsPerQuestion <= 0) quiz.secondsPerQuestion = DEFAULT_SECONDS;
        if (quiz.pointsPerQuestion <= 0) quiz.pointsPerQuestion = DEFAULT_POINTS;
        if (quiz.speedBonus < 0) quiz.speedBonus = DEFAULT_SPEED_BONUS;
        if (quiz.passPercent <= 0) quiz.passPercent = DEFAULT_PASS_PERCENT;

        for (int i = 0; i < quiz.questions.Length; i++)
        {
            QuizQuestion q = quiz.questions[i];
            int optionCount = q.options != null ? q.options.Length : 0;
            if (optionCount != QuizView.OPTION_COUNT)
            {
                Debug.LogError("Quiz question " + i + " (\"" + q.prompt + "\") has " + optionCount +
                               " options; the quiz screen shows exactly " + QuizView.OPTION_COUNT + ".");
            }
            if (q.answer < 0 || q.answer >= optionCount)
            {
                Debug.LogError("Quiz question " + i + " (\"" + q.prompt + "\") has answer index " +
                               q.answer + ", which is not one of its options.");
            }
            if (q.seconds <= 0) q.seconds = quiz.secondsPerQuestion;
        }
    }
}

// The outcome of the quiz the player just sat. Static so a follow-up cutscene, the Planner, or a
// later class can read it without the quiz scene still being loaded -- the same trick GameManager
// uses for RoomData. Cleared by GameManager.ResetGameData.
public static class QuizResults
{
    public static bool hasResult;
    public static string title = "";
    public static int score;
    public static int maxScore;
    public static int correct;
    public static int total;
    public static int bestStreak;
    public static bool passed;

    // The mark on the paper: questions right, out of questions asked. This -- not the points -- is
    // what the grade and the pass/fail are read off, so the speed bonus never costs a player who
    // answered everything correctly. `score` and `maxScore` are the arcade layer on top.
    public static int Percent()
    {
        return total > 0 ? Mathf.RoundToInt(100f * correct / total) : 0;
    }

    // Letter grade off the percentage, using the usual 90/80/70/60 banding.
    public static string Grade()
    {
        int p = Percent();
        if (p >= 97) return "A+";
        if (p >= 90) return "A";
        if (p >= 87) return "B+";
        if (p >= 80) return "B";
        if (p >= 77) return "C+";
        if (p >= 70) return "C";
        if (p >= 60) return "D";
        return "F";
    }

    public static void Clear()
    {
        hasResult = false;
        title = "";
        score = 0;
        maxScore = 0;
        correct = 0;
        total = 0;
        bestStreak = 0;
        passed = false;
    }
}
