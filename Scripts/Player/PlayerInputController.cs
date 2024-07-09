using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputController : MonoBehaviour
{
    [SerializeField] PlayerMasterController player;
    [SerializeField] float rayLength = 1;

    internal bool inputEnabled = true;

    internal Vector2 dirInput;

    public direction facing;
    internal Vector2 interactRay;
    internal BoxCollider2D col;
    int layerMask;

    Planner p;
    public Planner old_p;
    private void Start()
    {
        col = GetComponent<BoxCollider2D>();
        p = GameObject.Find("Planner").GetComponent<Planner>();
        layerMask = LayerMask.GetMask("Interact");
    }
    void Update()
    {
        if (inputEnabled)
        {
            DetermineDirection();
            int v = Input.GetKey(KeyCode.UpArrow) ? 1 : Input.GetKey(KeyCode.DownArrow) ? -1 : 0;
            int h = Input.GetKey(KeyCode.RightArrow) ? 1 : Input.GetKey(KeyCode.LeftArrow) ? -1 : 0;
            dirInput = new Vector2(h, v);
            if (Input.GetKey(KeyCode.LeftShift))
            {
                dirInput *= 1.5f;
            }
            switch (facing) {
                case direction.right:
                    interactRay = new Vector2(rayLength, 0);
                    break;
                case direction.up:
                    interactRay = new Vector2(0, rayLength);
                    break;
                case direction.down:
                    interactRay = new Vector2(0, -rayLength);
                    break;
                case direction.left:
                    interactRay = new Vector2(-rayLength, 0);
                    break;
            }
            Vector2 boxColVector = new Vector2(this.transform.position.x, this.transform.position.y) + col.offset;
            Debug.DrawRay(boxColVector, interactRay, Color.blue);
            if (Input.GetKeyDown(KeyCode.Z) && Physics2D.Raycast(boxColVector, interactRay, rayLength, layerMask))
            {
                RaycastHit2D hit = Physics2D.Raycast(boxColVector, interactRay, rayLength, layerMask);
                player.action.Interact(hit);
            } 
        }
      
        if (Input.GetKeyDown(KeyCode.Escape) && !player.cutscene.inCutscene)
        {
            if (GameManager.flags.hasPlanner)
            {
                if (!p.isActive)
                {
                    player.movement.rb.velocity = Vector2.zero;
                    DisableInput();
                    p.SetActive();
                }
                else
                {
                    p.SetInactive();
                    EnableInput();
                }
            }
            else
            {
                if (!old_p.gameObject.activeInHierarchy)
                {
                    player.movement.rb.velocity = Vector2.zero;
                    DisableInput();
                    old_p.gameObject.SetActive(true);
                }
                else
                {
                    old_p.gameObject.SetActive(false);
                    EnableInput();
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            GameManager.SaveGameData();
        }
    }   

    internal void DisableInput()
    {
        inputEnabled = false;
    }

    internal void EnableInput()
    {
        inputEnabled = true;
    }

    void DetermineDirection()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            facing = direction.up;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            facing = direction.down;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            facing = direction.left;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            facing = direction.right;
        }
    }

    void SetDirection(direction d)
    {
        facing = d;
    }
}
