using UnityEngine;
using System.Collections.Generic;
using TMPro;

// The Friends tab in the Planner: everyone the player has met, who is already a friend, and how
// close the rest are.
//
// It is a PlannerPage like Classes/Inventory/Map/System, so it slots into the existing left/right tab
// chain with no changes to how the book is wired. What it adds is a cursor: up/down walk the roster
// (scrolling when the cursor reaches an edge) and the detail panel follows. Those keys are only
// consumed while there is roster left to move through, so once the player hits the end the press
// falls through to normal Planner navigation and they are not trapped on the page.
public class FriendsPage : PlannerPage
{
    [Header("Roster rows")]
    [Tooltip("The fixed row widgets this page draws into, top to bottom. The roster scrolls through them.")]
    [SerializeField] FriendRow[] rows;

    [Tooltip("Header line, e.g. \"Friends  3 / 11\".")]
    [SerializeField] TextMeshPro headerText;

    [Tooltip("Shown instead of the list before the player has met anybody.")]
    [SerializeField] TextMeshPro emptyText;

    [Tooltip("Arrows shown when there is more roster above or below the visible rows.")]
    [SerializeField] GameObject scrollUpArrow;
    [SerializeField] GameObject scrollDownArrow;

    [Header("Detail panel")]
    [SerializeField] SpriteRenderer detailPortrait;
    [SerializeField] TextMeshPro detailNameText;
    [SerializeField] TextMeshPro detailStatusText;

    [Tooltip("Explains what being friends actually does. Shown for a friend.")]
    [SerializeField] string friendBlurb = "You can talk to them any time. No need to break the ice.";

    [Tooltip("Shown for someone the player knows but is not yet friends with.")]
    [SerializeField] string acquaintanceBlurb = "Talk to them again to get to know them better.";

    List<string> roster = new List<string>();

    // Index into `roster` of the highlighted person, and the roster index drawn in the first row.
    int cursor;
    int scroll;

    public override void Hover()
    {
        base.Hover();
        Rebuild();
    }

    public override void OnPlannerOpened()
    {
        Rebuild();
    }

    // Re-read the friendship store and redraw. Cheap enough to do on every open, and it means the
    // page is never showing a stale roster after a conversation.
    public void Rebuild()
    {
        roster = Friendship.Roster();
        cursor = Mathf.Clamp(cursor, 0, Mathf.Max(roster.Count - 1, 0));
        ClampScroll();
        Redraw();
    }

    public override bool HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) && cursor > 0)
        {
            cursor--;
            ClampScroll();
            Redraw();
            return true;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow) && cursor < roster.Count - 1)
        {
            cursor++;
            ClampScroll();
            Redraw();
            return true;
        }
        // Nothing left to scroll to in that direction: let the Planner have the key.
        return false;
    }

    // Keep the cursor inside the visible window, scrolling by the minimum needed.
    void ClampScroll()
    {
        int visible = VisibleRows();
        if (visible <= 0)
        {
            scroll = 0;
            return;
        }
        scroll = Mathf.Clamp(scroll, cursor - visible + 1, cursor);
        scroll = Mathf.Clamp(scroll, 0, Mathf.Max(roster.Count - visible, 0));
    }

    int VisibleRows()
    {
        return rows != null ? rows.Length : 0;
    }

    void Redraw()
    {
        int visible = VisibleRows();
        bool anyone = roster.Count > 0;

        if (headerText != null)
        {
            headerText.text = "Friends  " + Friendship.FriendCount() + " / " + roster.Count;
        }
        if (emptyText != null)
        {
            emptyText.gameObject.SetActive(!anyone);
            emptyText.text = "You haven't got to know anyone yet.";
        }

        for (int i = 0; i < visible; i++)
        {
            FriendRow row = rows[i];
            if (row == null)
            {
                continue;
            }
            int rosterIndex = scroll + i;
            if (rosterIndex < roster.Count)
            {
                string npc = roster[rosterIndex];
                row.Show(npc, Friendship.Points(npc), rosterIndex == cursor);
            }
            else
            {
                row.Hide();
            }
        }

        SetActive(scrollUpArrow, scroll > 0);
        SetActive(scrollDownArrow, scroll + visible < roster.Count);

        RedrawDetail(anyone ? roster[cursor] : null);
    }

    void RedrawDetail(string npc)
    {
        bool has = !string.IsNullOrEmpty(npc);
        if (detailPortrait != null)
        {
            // The same portrait sheet the dialog box uses, so the face matches who they talk to.
            detailPortrait.sprite = has ? DialogPrinter.FindHead(npc) : null;
            detailPortrait.enabled = detailPortrait.sprite != null;
        }
        if (detailNameText != null)
        {
            detailNameText.text = has ? npc : "";
        }
        if (detailStatusText != null)
        {
            if (!has)
            {
                detailStatusText.text = "";
            }
            else if (Friendship.IsFriend(npc))
            {
                detailStatusText.text = "Friend\n" + friendBlurb;
            }
            else
            {
                detailStatusText.text = "Getting to know them  " +
                    Friendship.Points(npc) + " / " + Friendship.FRIEND_THRESHOLD + "\n" + acquaintanceBlurb;
            }
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
