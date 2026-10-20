using UnityEngine;

// The text box for the chooseHeroName cutscene action. Drawn with IMGUI so it needs no scene setup;
// swap in a styled UI later if wanted. CutsceneManager adds it, waits for done, then removes it.
public class HeroNameEntry : MonoBehaviour
{
    public const int maxLength = 16;
    internal bool done = false;
    internal string heroName = "";
    string entry = "";
    bool focused = false;

    void OnGUI()
    {
        float scale = Screen.height / 540f;
        GUI.matrix = Matrix4x4.Scale(new Vector3(scale, scale, 1));
        float width = 360;
        float height = 130;
        float x = (Screen.width / scale - width) / 2;
        float y = (Screen.height / scale - height) / 2;

        Event e = Event.current;
        if (e.type == EventType.KeyDown && (e.keyCode == KeyCode.Return || e.keyCode == KeyCode.KeypadEnter))
        {
            Confirm();
            e.Use();
        }

        GUI.Box(new Rect(x, y, width, height), "Choose your hero name");
        GUI.SetNextControlName("HeroName");
        entry = GUI.TextField(new Rect(x + 20, y + 35, width - 40, 30), entry, maxLength);
        if (!focused)
        {
            GUI.FocusControl("HeroName");
            focused = true;
        }
        GUI.enabled = entry.Trim() != "";
        if (GUI.Button(new Rect(x + width / 2 - 60, y + 80, 120, 30), "OK (Enter)"))
        {
            Confirm();
        }
        GUI.enabled = true;
    }

    void Confirm()
    {
        string chosen = entry.Trim();
        if (chosen == "")
        {
            return;
        }
        heroName = chosen;
        done = true;
    }
}
