using UnityEngine;
using TMPro;

// One line in the roster: portrait, name, and how far along the friendship is. Every field is
// optional so the page still runs while the layout is being dressed in the scene.
public class FriendRow : MonoBehaviour
{
    [SerializeField] SpriteRenderer portrait;
    [SerializeField] TextMeshPro nameText;
    [SerializeField] TextMeshPro statusText;

    [Tooltip("Shown only for a full friend -- a heart, a star, whatever the art calls for.")]
    [SerializeField] GameObject friendIcon;

    [Tooltip("Highlight behind the row the cursor is on.")]
    [SerializeField] GameObject cursorHighlight;

    [SerializeField] Color friendColor = Color.white;
    [SerializeField] Color acquaintanceColor = Color.grey;

    public void Show(string npc, int points, bool highlighted)
    {
        gameObject.SetActive(true);
        bool isFriend = points >= Friendship.FRIEND_THRESHOLD;

        if (portrait != null)
        {
            portrait.sprite = DialogPrinter.FindHead(npc);
            portrait.enabled = portrait.sprite != null;
        }
        if (nameText != null)
        {
            nameText.text = npc;
            nameText.color = isFriend ? friendColor : acquaintanceColor;
        }
        if (statusText != null)
        {
            statusText.text = isFriend
                ? "Friend"
                : points + " / " + Friendship.FRIEND_THRESHOLD;
            statusText.color = isFriend ? friendColor : acquaintanceColor;
        }
        if (friendIcon != null)
        {
            friendIcon.SetActive(isFriend);
        }
        if (cursorHighlight != null)
        {
            cursorHighlight.SetActive(highlighted);
        }
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
