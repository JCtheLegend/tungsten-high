using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    [SerializeField] RoomManager room;
    [SerializeField] PlayerMasterController player;
    [SerializeField] internal Rigidbody2D rb;
    [SerializeField] float velocity;

    // Set while the player is walking out through a door (see BeginWalkOut).
    bool walkingOut = false;
    Vector2 walkOutDir;

    private void Awake()
    {
        // Resolve the spawn point once: an Entrance marker matching RoomData.toEntranceId wins,
        // otherwise the legacy entrances[]/entranceDirs[] arrays indexed by toEntranceNum.
        // Scenes without a RoomManager (fights, puzzles) simply keep the player where they are.
        Vector2 spawnPos = Vector2.zero;
        direction spawnFacing = direction.down;
        bool haveSpawn = room != null && room.TryGetSpawn(out spawnPos, out spawnFacing, out _);

        if (!GameManager.startedInClient && haveSpawn) {
            rb.position = spawnPos;
            player.input.facing = spawnFacing;
        }
        if (haveSpawn && GameManager.followers.Count > 0)
        {
            string nextLeader = "Player";
            foreach(string s in GameManager.followers)
            {
                LoadedObject follower = new LoadedObject
                {
                    name = s,
                    type = s,
                    facing = spawnFacing.ToString()
                };
                switch (spawnFacing)
                {
                    case direction.right:
                        follower.location = (spawnPos.x - 2).ToString() + "," + (spawnPos.y).ToString();
                        break;
                    case direction.left:
                        follower.location = (spawnPos.x + 2).ToString() + "," + (spawnPos.y).ToString();
                        break;
                    case direction.up:
                        follower.location = (spawnPos.x).ToString() + "," + (spawnPos.y - 2).ToString();
                        break;
                    case direction.down:
                        follower.location = (spawnPos.x).ToString() + "," + (spawnPos.y + 2).ToString();
                        break;
                }
                ObjectLoader.LoadObject(follower);
                GameObject.Find(follower.name).GetComponent<FollowLeader>().enabled = true;
                GameObject.Find(follower.name).GetComponent<FollowLeader>().SetLeader(nextLeader);
                nextLeader = follower.name;
            }
        }


        if(player.input.facing == direction.left)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        ApplyMovement();
    }

    void ApplyMovement()
    {
        // Walking out through a door: input is disabled, so drive the player explicitly instead of
        // coasting on leftover Rigidbody velocity (which any wall contact would cancel, stranding them).
        if (walkingOut)
        {
            rb.velocity = walkOutDir * velocity;
            return;
        }
        if (player.input.inputEnabled)
        {
            rb.velocity = player.input.dirInput * velocity;
        }
    }

    // Keep moving along `heading` until the scene changes. Used by the door walk-out, where input is
    // off. `heading` is the player's live velocity, not their facing: facing is picked by priority
    // (up > down > left > right), so approaching a side exit diagonally reports "up" and would drive
    // the player along the wall instead of through the doorway. A zero heading falls back to facing.
    internal void BeginWalkOut(Vector2 heading)
    {
        walkOutDir = heading.sqrMagnitude > 0.01f
            ? heading.normalized
            : Direction.ToVector(player.input.facing).normalized;
        walkingOut = true;
        // Input is frozen from here, so pin facing/dirInput to the walk-out heading -- the animation
        // controller reads both and would otherwise hold the pre-collision animation while we move.
        direction cardinal = Direction.FromVector(walkOutDir);
        player.input.facing = cardinal;
        player.input.dirInput = Direction.ToVector(cardinal);
    }

    internal void EndWalkOut()
    {
        walkingOut = false;
        rb.velocity = Vector2.zero;
    }
}
