using UnityEngine;

// A spawn marker placed in a scene. Drop this on an empty GameObject where you want the player to
// appear when they come through a door; the marker's transform.position IS the spawn coordinate, so
// there is no need to type x/y by hand. A RoomExit targets one of these by matching `id`.
//
// See RoomManager.TryGetSpawn for how a marker is resolved at scene load, and RoomExit.toEntranceId
// for the door side. If no marker matches, the room falls back to the legacy entrances[] arrays.
public class Entrance : MonoBehaviour
{
    [Tooltip("Stable name a door points at, e.g. \"FromHallway\" or \"Main\". Must be unique within a scene.")]
    public string id;

    [Tooltip("Which way the player faces when they arrive here. Also selects which spawn offset below applies.")]
    public direction facing = direction.down;

    // Per-facing spawn offset. The player's collider is centered on the body (above the transform
    // origin), so the marker is placed at the doorway and the player is pushed off the door in the
    // direction they face. Each direction is tuned independently because the body-centered collider
    // makes the up/down magnitudes asymmetric. Defaults push one unit in the facing direction.
    [Header("Spawn offset by facing")]
    [Tooltip("Applied when facing up (door is below → push the hitbox up).")]
    public Vector2 offsetUp = new Vector2(0, 1);
    [Tooltip("Applied when facing down (door is above → push the hitbox down).")]
    public Vector2 offsetDown = new Vector2(0, -1);
    [Tooltip("Applied when facing left (door is to the right → push the hitbox left).")]
    public Vector2 offsetLeft = new Vector2(-1, 0);
    [Tooltip("Applied when facing right (door is to the left → push the hitbox right).")]
    public Vector2 offsetRight = new Vector2(1, 0);

    [Tooltip("Optional arrival sound played when the player spawns here (Resources/Sounds/<name>). Leave blank for none.")]
    public string sound = "";

    // The offset for the current facing direction.
    public Vector2 SpawnOffset()
    {
        switch (facing)
        {
            case direction.up: return offsetUp;
            case direction.down: return offsetDown;
            case direction.left: return offsetLeft;
            case direction.right: return offsetRight;
            default: return offsetDown;
        }
    }

    // The actual point the player transform is placed at: the marker's position plus the facing offset.
    public Vector2 SpawnPosition()
    {
        return (Vector2)transform.position + SpawnOffset();
    }

    // Unit vector for `facing`, used by the gizmo (and available if other code wants the direction).
    public Vector2 FacingVector()
    {
        return Direction.ToVector(facing);
    }

    // Scene-view visualization: a small dot where the marker sits (the doorway), a green dot where the
    // player transform actually spawns (marker + offset), a line between them, and an arrow for facing.
    private void OnDrawGizmos()
    {
        Vector3 markerPos = transform.position;
        Vector3 spawnPos = (Vector3)SpawnPosition();

        // The doorway anchor (where you placed the marker).
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(markerPos, 0.15f);

        // The spawn offset link + the resolved spawn point (where the player lands).
        Gizmos.color = Color.green;
        Gizmos.DrawLine(markerPos, spawnPos);
        Gizmos.DrawWireSphere(spawnPos, 0.3f);

        // Facing arrow, drawn from the spawn point.
        Gizmos.color = Color.cyan;
        Vector3 dir = (Vector3)FacingVector();
        Vector3 tip = spawnPos + dir;
        Gizmos.DrawLine(spawnPos, tip);
        Vector3 perp = new Vector3(-dir.y, dir.x, 0) * 0.15f;
        Gizmos.DrawLine(tip, tip - dir * 0.3f + perp);
        Gizmos.DrawLine(tip, tip - dir * 0.3f - perp);
    }
}
