using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    [SerializeField] RoomManager room;
    [SerializeField] PlayerMasterController player;
    [SerializeField] internal Rigidbody2D rb;
    [SerializeField] float velocity;

    private void Awake()
    {
        if (!GameManager.startedInClient) {
            rb.position = room.entrances[GameManager.RoomData.toEntranceNum];
            player.input.facing = room.entranceDirs[GameManager.RoomData.toEntranceNum];
        }
        if (GameManager.followers.Count > 0)
        {
            string nextLeader = "Player";
            foreach(string s in GameManager.followers)
            {
                LoadedObject follower = new LoadedObject
                {
                    name = s,
                    type = s,
                    facing = room.entranceDirs[GameManager.RoomData.toEntranceNum].ToString()
                };
                switch (room.entranceDirs[GameManager.RoomData.toEntranceNum])
                {
                    case direction.right:
                        follower.location = (room.entrances[GameManager.RoomData.toEntranceNum].x - 2).ToString() + "," + (room.entrances[GameManager.RoomData.toEntranceNum].y).ToString();
                        break;
                    case direction.left:
                        follower.location = (room.entrances[GameManager.RoomData.toEntranceNum].x + 2).ToString() + "," + (room.entrances[GameManager.RoomData.toEntranceNum].y).ToString();
                        break;
                    case direction.up:
                        follower.location = (room.entrances[GameManager.RoomData.toEntranceNum].x).ToString() + "," + (room.entrances[GameManager.RoomData.toEntranceNum].y - 2).ToString();
                        break;
                    case direction.down:
                        follower.location = (room.entrances[GameManager.RoomData.toEntranceNum].x).ToString() + "," + (room.entrances[GameManager.RoomData.toEntranceNum].y + 2).ToString();
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
        if (player.input.inputEnabled)
        {
            rb.velocity = player.input.dirInput * velocity;
        }
    }
}
