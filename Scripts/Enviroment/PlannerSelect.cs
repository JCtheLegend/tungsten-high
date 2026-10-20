using UnityEngine;
using TMPro;

// One selectable entry in the Planner: a tab, a page, or a button. Entries are wired into a grid by
// their up/down/left/right neighbours in the inspector, and Planner walks that graph.
//
// Split into its own file because Unity only makes the class whose name matches the file attachable
// to a GameObject -- classes sharing a file with another are invisible to the inspector.
public class PlannerSelect : MonoBehaviour
{
    public PlannerSelect up;
    public PlannerSelect down;
    public PlannerSelect left;
    public PlannerSelect right;
    public TextMeshPro text;

    public Sprite hoveredSprite;
    public Sprite unhoveredSprite;

    public Color hoveredTextColor;
    public Color unhoveredTextColor;

    public SpriteRenderer sprite;
    public virtual void Hover()
    {
        if (hoveredSprite != null)
        {
            sprite.sprite = hoveredSprite;
        }
        if (text)
        {
            text.color = hoveredTextColor;
        }
    }
    public enum ButtonType { none, save, reload, saveQuit, close };

    public ButtonType type;

    public virtual void Dehover()
    {
        if (unhoveredSprite != null)
        {
            sprite.sprite = unhoveredSprite;
        }
        if (text)
        {
            text.color = unhoveredTextColor;
        }
    }
    public virtual void Select()
    {
    }

    // Let a page take an arrow key before the Planner uses it to move the highlight. Return true to
    // say "I used that key". Default is to consume nothing, so existing pages navigate as before.
    // Used by FriendsPage, which needs up/down to walk its roster.
    public virtual bool HandleInput()
    {
        return false;
    }

    // Called on whichever entry is highlighted when the Planner is opened, so a page showing live
    // data can refresh. Hover() covers turning *to* a page; this covers opening onto one.
    public virtual void OnPlannerOpened()
    {
    }
}
