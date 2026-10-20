using System.Collections;
using UnityEngine;
using TMPro;

// The little "<Name> is now your friend!" banner that pops over an NPC's head the moment a
// friendship is sealed.
//
// Deliberately not part of the dialog system. A friendship is usually sealed *inside* a running
// cutscene (the talkMeter action), and starting a nested cutscene there would stomp the outer loop's
// inCutscene/actionDone flags. This is the same self-contained popup the Dodgeball scene uses for its
// hit and catch alerts: spawn a prefab, show it briefly, destroy it.
//
// Needs a prefab at Resources/LoadPrefabs/FriendAlert with this component on it. If the prefab is
// missing the friendship is still recorded -- only the flourish is skipped.
public class FriendAlert : MonoBehaviour
{
    public const string PREFAB_PATH = "LoadPrefabs/FriendAlert";

    [SerializeField] TextMeshPro label;
    [SerializeField] SpriteRenderer portrait;

    [Tooltip("Seconds the banner stays up.")]
    [SerializeField] float lifetime = 2.5f;

    [Tooltip("How far the banner drifts upward while it is on screen.")]
    [SerializeField] float riseDistance = 0.75f;

    [Tooltip("Offset from the point it is spawned at, so it clears the NPC's head.")]
    [SerializeField] Vector3 offset = new Vector3(0, 1.5f, 0);

    // Pop the banner for `npc` at `worldPosition`. Safe to call from anywhere, including a static
    // context -- this is the only entry point.
    public static void Show(string npc, Vector3 worldPosition)
    {
        GameObject prefab = Resources.Load<GameObject>(PREFAB_PATH);
        if (prefab == null)
        {
            Debug.LogWarning("No FriendAlert prefab at Resources/" + PREFAB_PATH +
                             "; became friends with " + npc + " without the popup.");
            return;
        }
        GameObject spawned = Instantiate(prefab, worldPosition, Quaternion.identity);
        FriendAlert alert = spawned.GetComponent<FriendAlert>();
        if (alert == null)
        {
            Debug.LogWarning("The FriendAlert prefab has no FriendAlert component.");
            Destroy(spawned);
            return;
        }
        alert.Play(npc, worldPosition);
    }

    void Play(string npc, Vector3 worldPosition)
    {
        transform.position = worldPosition + offset;
        if (label != null)
        {
            label.text = npc + " is now your friend!";
        }
        if (portrait != null)
        {
            // Same portrait sheet the dialog box uses, so the face matches the one they talk with.
            portrait.sprite = DialogPrinter.FindHead(npc);
            portrait.enabled = portrait.sprite != null;
        }
        StartCoroutine(RiseAndFade());
    }

    IEnumerator RiseAndFade()
    {
        Vector3 start = transform.position;
        Vector3 end = start + Vector3.up * riseDistance;
        float elapsed = 0;
        while (elapsed < lifetime)
        {
            elapsed += Time.deltaTime;
            transform.position = Vector3.Lerp(start, end, elapsed / lifetime);
            // Fade out over the last third, so it does not simply vanish.
            float alpha = Mathf.Clamp01((lifetime - elapsed) / (lifetime / 3f));
            SetAlpha(alpha);
            yield return null;
        }
        Destroy(gameObject);
    }

    void SetAlpha(float alpha)
    {
        if (label != null)
        {
            Color c = label.color;
            label.color = new Color(c.r, c.g, c.b, alpha);
        }
        foreach (SpriteRenderer s in GetComponentsInChildren<SpriteRenderer>())
        {
            Color c = s.color;
            s.color = new Color(c.r, c.g, c.b, alpha);
        }
    }
}
