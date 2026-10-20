using UnityEngine;
using TMPro;

// Everything the quiz draws. QuizController owns the rules and never touches a TextMeshPro or a
// SpriteRenderer directly; it calls the methods here. Keeping the split means the board can be
// relaid out in the scene (2x2 plates, a 1x4 column, different art) without touching the game logic.
//
// Scene setup: three panels parented under this object -- the title card, the question board and the
// score screen -- wired to the fields below. All world-space TextMeshPro, matching the dialog box
// and the Planner.
public class QuizView : MonoBehaviour
{
    // Four answers per question, as the design calls for. QuizLoader validates files against this.
    public const int OPTION_COUNT = 4;

    [Header("Panels")]
    [SerializeField] GameObject titlePanel;
    [SerializeField] GameObject questionPanel;
    [SerializeField] GameObject resultsPanel;

    [Header("Title card")]
    [SerializeField] TextMeshPro titleText;
    [SerializeField] TextMeshPro subtitleText;

    [Tooltip("Big centered text used for \"Ready\" / \"Set\" / \"Go!\" and for \"Time!\".")]
    [SerializeField] TextMeshPro bannerText;

    [Header("Question board")]
    [SerializeField] TextMeshPro promptText;
    [Tooltip("Progress readout, e.g. \"Question 3 / 8\".")]
    [SerializeField] TextMeshPro counterText;
    [SerializeField] TextMeshPro runningScoreText;
    [Tooltip("Shown under the question after an answer is locked in.")]
    [SerializeField] TextMeshPro explanationText;

    [Tooltip("The four answer labels, in screen order: top-left, top-right, bottom-left, bottom-right.")]
    [SerializeField] TextMeshPro[] optionLabels = new TextMeshPro[OPTION_COUNT];
    [Tooltip("The plate behind each answer, recoloured to show selection and the reveal.")]
    [SerializeField] SpriteRenderer[] optionPlates = new SpriteRenderer[OPTION_COUNT];

    [Header("Timer")]
    [Tooltip("Squashed horizontally to show time left, the way the health and energy bars work.")]
    [SerializeField] SpriteRenderer timerBar;
    [SerializeField] TextMeshPro timerText;
    [Tooltip("Timer bar colour with plenty of time left.")]
    [SerializeField] Color timerCalmColor = Color.white;
    [Tooltip("Timer bar colour as the clock runs out.")]
    [SerializeField] Color timerPanicColor = Color.red;
    [Tooltip("Fraction of the clock below which the bar turns panicky and ticks.")]
    [SerializeField] float panicThreshold = 0.25f;

    [Header("Answer colours")]
    [SerializeField] Color plateIdleColor = Color.white;
    [SerializeField] Color plateSelectedColor = Color.yellow;
    [SerializeField] Color plateCorrectColor = Color.green;
    [SerializeField] Color plateWrongColor = Color.red;

    [Header("Score screen")]
    [SerializeField] TextMeshPro resultsTitleText;
    [SerializeField] TextMeshPro resultsScoreText;
    [SerializeField] TextMeshPro resultsBreakdownText;
    [SerializeField] TextMeshPro resultsGradeText;
    [Tooltip("\"Press Z to continue\" prompt; flashed once the score is fully counted up.")]
    [SerializeField] GameObject resultsContinuePrompt;

    [Header("Layout")]
    [Tooltip("Answers per row. 2 gives a 2x2 grid (all four arrows navigate); 1 gives a column.")]
    [SerializeField] int columns = 2;

    [Header("Tutorial")]
    [Tooltip("Pointer shown beside whatever the tutorial is talking about.")]
    [SerializeField] GameObject highlighter;
    [SerializeField] Transform highlightOptions;
    [SerializeField] Transform highlightTimer;
    [SerializeField] Transform highlightScore;

    void Awake()
    {
        HideAll();
    }

    public void HideAll()
    {
        SetActive(titlePanel, false);
        SetActive(questionPanel, false);
        SetActive(resultsPanel, false);
        SetActive(highlighter, false);
        SetBanner("");
    }

    // ---- Title card -------------------------------------------------------------------------

    public void ShowTitle(string title, string subtitle)
    {
        SetActive(titlePanel, true);
        SetText(titleText, title);
        SetText(subtitleText, subtitle);
    }

    public void HideTitle()
    {
        SetActive(titlePanel, false);
    }

    // The big centered word: "Ready", "Set", "Go!", "Time!". Blank hides it.
    public void SetBanner(string text)
    {
        SetText(bannerText, text);
        if (bannerText != null)
        {
            bannerText.gameObject.SetActive(!string.IsNullOrEmpty(text));
        }
    }

    // ---- Question board ---------------------------------------------------------------------

    public void ShowBoard(bool visible)
    {
        SetActive(questionPanel, visible);
    }

    public void ShowQuestion(QuizQuestion q, int number, int total, int runningScore)
    {
        SetActive(questionPanel, true);
        SetText(promptText, q.prompt);
        SetText(counterText, "Question " + number + " / " + total);
        SetText(runningScoreText, runningScore.ToString());
        SetText(explanationText, "");
        for (int i = 0; i < OPTION_COUNT; i++)
        {
            string label = (q.options != null && i < q.options.Length) ? q.options[i] : "";
            SetText(OptionLabel(i), label);
            SetPlateColor(i, plateIdleColor);
        }
    }

    public void SetSelection(int index)
    {
        for (int i = 0; i < OPTION_COUNT; i++)
        {
            SetPlateColor(i, i == index ? plateSelectedColor : plateIdleColor);
        }
    }

    // Colour the correct plate green and, if the player got it wrong, their plate red. `chosen` of
    // -1 means the clock ran out, so only the correct answer lights up.
    public void RevealAnswer(int correctIndex, int chosen, string explanation)
    {
        for (int i = 0; i < OPTION_COUNT; i++)
        {
            SetPlateColor(i, plateIdleColor);
        }
        SetPlateColor(correctIndex, plateCorrectColor);
        if (chosen >= 0 && chosen != correctIndex)
        {
            SetPlateColor(chosen, plateWrongColor);
        }
        SetText(explanationText, explanation);
    }

    public void SetRunningScore(int score)
    {
        SetText(runningScoreText, score.ToString());
    }

    // ---- Timer ------------------------------------------------------------------------------

    // `secondsLeft` / `secondsTotal`; drives the bar scale, the readout and the panic colour.
    public void SetTimer(float secondsLeft, float secondsTotal)
    {
        float fraction = secondsTotal > 0 ? Mathf.Clamp01(secondsLeft / secondsTotal) : 0;
        if (timerBar != null)
        {
            Vector3 scale = timerBar.transform.localScale;
            timerBar.transform.localScale = new Vector3(fraction, scale.y, scale.z);
            timerBar.color = fraction <= panicThreshold ? timerPanicColor : timerCalmColor;
        }
        SetText(timerText, Mathf.CeilToInt(Mathf.Max(secondsLeft, 0)).ToString());
    }

    // True once the clock is low enough to warrant the ticking sound.
    public bool IsPanicking(float secondsLeft, float secondsTotal)
    {
        return secondsTotal > 0 && secondsLeft / secondsTotal <= panicThreshold;
    }

    // ---- Navigation -------------------------------------------------------------------------

    // Move the selection one step. Laid out row-major over `columns`, so with the default 2 columns
    // up/down move between rows and left/right between columns; movement off an edge is ignored.
    public int Navigate(int index, direction dir)
    {
        int cols = Mathf.Max(columns, 1);
        int rows = Mathf.CeilToInt((float)OPTION_COUNT / cols);
        int row = index / cols;
        int col = index % cols;
        switch (dir)
        {
            case direction.up: row--; break;
            case direction.down: row++; break;
            case direction.left: col--; break;
            case direction.right: col++; break;
            default: return index;
        }
        if (row < 0 || row >= rows || col < 0 || col >= cols)
        {
            return index;
        }
        int moved = row * cols + col;
        return moved >= 0 && moved < OPTION_COUNT ? moved : index;
    }

    // ---- Score screen -----------------------------------------------------------------------

    public void ShowResults(string title, int score, int maxScore, int correct, int total,
                            int bestStreak, string grade, bool passed)
    {
        SetActive(questionPanel, false);
        SetActive(resultsPanel, true);
        SetActive(resultsContinuePrompt, false);
        SetText(resultsTitleText, title);
        SetText(resultsScoreText, "0");
        SetText(resultsBreakdownText,
            correct + " / " + total + " correct\n" +
            "Best streak  " + bestStreak + "\n" +
            "Out of  " + maxScore);
        SetText(resultsGradeText, grade);
        if (resultsGradeText != null)
        {
            resultsGradeText.color = passed ? plateCorrectColor : plateWrongColor;
        }
    }

    // Called each frame by the controller while it counts the score up, so the number rolls.
    public void SetResultsScore(int score)
    {
        SetText(resultsScoreText, score.ToString());
    }

    public void ShowContinuePrompt()
    {
        SetActive(resultsContinuePrompt, true);
    }

    // ---- Tutorial highlight -----------------------------------------------------------------

    // Park the pointer next to the thing the tutorial is describing. An unknown or blank name hides it.
    public void SetHighlight(string target)
    {
        if (highlighter == null)
        {
            return;
        }
        Transform anchor = null;
        switch (target)
        {
            case "options": anchor = highlightOptions; break;
            case "timer": anchor = highlightTimer; break;
            case "score": anchor = highlightScore; break;
        }
        if (anchor == null)
        {
            highlighter.SetActive(false);
            return;
        }
        highlighter.transform.position = anchor.position;
        highlighter.SetActive(true);
    }

    // ---- Helpers ----------------------------------------------------------------------------
    // Every field above is optional so a scene can be dressed a piece at a time; these guards mean a
    // half-wired board still runs instead of throwing on the first question.

    TextMeshPro OptionLabel(int i)
    {
        return optionLabels != null && i < optionLabels.Length ? optionLabels[i] : null;
    }

    void SetPlateColor(int i, Color c)
    {
        if (optionPlates != null && i < optionPlates.Length && optionPlates[i] != null)
        {
            optionPlates[i].color = c;
        }
    }

    static void SetText(TextMeshPro field, string value)
    {
        if (field != null)
        {
            field.text = value;
        }
    }

    static void SetActive(GameObject go, bool active)
    {
        if (go != null)
        {
            go.SetActive(active);
        }
    }
}
