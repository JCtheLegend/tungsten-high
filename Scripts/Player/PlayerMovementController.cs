using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    [SerializeField] RoomManager room;
    [SerializeField] PlayerMasterController player;
    [SerializeField] internal Rigidbody2D rb;
    [SerializeField] float velocity;

    private void Start()
    {
        rb.position = room.entrances[GameManager.RoomData.toEntranceNum];
        player.input.facing = room.entranceDirs[GameManager.RoomData.toEntranceNum];
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
