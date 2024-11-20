using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMover : MonoBehaviour
{
    protected bool moveEnabled = true;
    [SerializeField] protected float moveSpeed = 1;
    internal Vector2 moveVelocity;
    protected Coroutine movingRoutine;

    public float arenaMinWidth;
    public float arenaMinHeight;
    public float arenaMaxWidth;
    public float arenaMaxHeight;
    protected Rigidbody2D rb;

    public bool isMoving;

    protected Animator animator;

    protected int energy = 100;
    public int fullEnergy = 100;

    protected SpriteRenderer energyBar;
    protected SpriteRenderer energyLevel;
    Sprite fullEnergySprite;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        energyBar = GameObject.Find("Energy Bar").GetComponent<SpriteRenderer>();
        energyLevel = GameObject.Find("Energy Level").GetComponent<SpriteRenderer>();
        fullEnergySprite = energyLevel.sprite;
    }

    public void GridMove()
    {
        if (!isMoving)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                Move(direction.up);
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                Move(direction.down);
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                Move(direction.left);
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                Move(direction.right);
            }
        }   
        rb.velocity = moveVelocity;
    }

    private void Move(direction dir)
    {
        if (dir == direction.down && transform.position.y > arenaMinHeight)
        {
            movingRoutine = StartCoroutine(MoveDown());
        }
        else if (dir == direction.left && transform.position.x > arenaMinWidth)
        {
            movingRoutine = StartCoroutine(MoveLeft());
        }
        else if (dir == direction.up && transform.position.y < arenaMaxHeight)
        {
            movingRoutine = StartCoroutine(MoveUp());
        }
        else if (dir == direction.right && transform.position.x < arenaMaxWidth)
        {
            movingRoutine = StartCoroutine(MoveRight());
        }

    }

    IEnumerator MoveLeft()
    {
        float newPosition = Mathf.Ceil(transform.position.x - 0.5f) - 0.5f;
        isMoving = true;
        while (transform.position.x > newPosition && isMoving)
        {
            moveVelocity = new Vector2(-moveSpeed, 0);
            yield return new WaitForSeconds(0.001f);
        }
        moveVelocity = Vector2.zero;
        transform.position = new Vector2(newPosition, transform.position.y);
        isMoving = false;
    }
    IEnumerator MoveRight()
    {
        float newPosition = Mathf.Floor(transform.position.x + 0.5f) + 0.5f;
        isMoving = true;
        while (transform.position.x < newPosition && isMoving)
        {
            moveVelocity = new Vector2(moveSpeed, 0);
            yield return new WaitForSeconds(0.001f);
        }
        moveVelocity = Vector2.zero;
        transform.position = new Vector2(newPosition, transform.position.y);
        isMoving = false;
    }
    IEnumerator MoveUp()
    {
        float newPosition = Mathf.Floor(transform.position.y + 1);
        isMoving = true;
        while (transform.position.y < newPosition && isMoving)
        {
            moveVelocity = new Vector2(0, moveSpeed);
            yield return new WaitForSeconds(0.001f);
        }
        moveVelocity = Vector2.zero;
        transform.position = new Vector2(transform.position.x, newPosition);
        isMoving = false;
    }
    IEnumerator MoveDown()
    {
        float newPosition = Mathf.Ceil(transform.position.y - 1);
        isMoving = true;
        while (transform.position.y > newPosition && isMoving)
        {

            moveVelocity = new Vector2(0, -moveSpeed);
            yield return new WaitForSeconds(0.001f);
        }
        moveVelocity = Vector2.zero;
        transform.position = new Vector2(transform.position.x, newPosition);
        isMoving = false;
    }

    internal void DisableInput()
    {
        moveEnabled = false;
    }

    internal void EnableInput()
    {
        moveEnabled = true;
    }

    protected void UseEnergy(int amount)
    {
        energy -= amount;      
        CropEnergyBarSprite();
    }

    private void CropEnergyBarSprite()
    {
        float p = energy * 1.0f / fullEnergy * 1.0f;
        if (p < 0)
        {
            p = 0;
        }
        //Rect r = new Rect(0, 0, fullEnergySprite.rect.width, (int)Mathf.Ceil(fullEnergySprite.rect.height * p));
        //Sprite newSprite = Sprite.Create(fullEnergySprite.texture, r, new Vector2(0.5f, 0), 16);
        //energyLevel.sprite = newSprite;
        energyLevel.transform.localScale = new Vector3(1, p, 1);
    }

    protected void RegenEnergy(int amount)
    {
        energy += amount;
        if(energy > fullEnergy)
        {
            energy = fullEnergy;
        }
        CropEnergyBarSprite();
    }
}
