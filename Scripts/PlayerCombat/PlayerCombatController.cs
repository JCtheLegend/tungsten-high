using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatController : CombatController
{
    [SerializeField] PlayerCombatMasterController player;
    [SerializeField] AnimationClip punchAnimation;
    [SerializeField] private float moveSpeed = 1;
    [SerializeField] GameObject playerAttackObject;
    [SerializeField] SpriteRenderer playerHealthBar;
    Sprite playerHealthBarFull;
    bool inputEnabled;

    public bool lockMovement = true;

    private Vector2 moveVelocity;

    internal Vector2 position;
    private int arenaWidth;
    private int arenaHeight;

    internal bool inCombat = false;
    internal combatAction action = combatAction.none;
    private Rigidbody2D rb;

    public int attackDamage;
    public int currentHealth;
    public int maxHealth;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        playerHealthBarFull = playerHealthBar.sprite;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K) && !inCombat)
        {
            BeginCombat(new Vector2(50, 50), 100, 100);
        }
        else if (Input.GetKeyDown(KeyCode.K) && inCombat){
            EndCombat();
        }
        if (inCombat)
        {
            CombatMove();
        }
    }

    void CombatMove()
    {
        ApplyMovement();
        ApplyBlock();
        ApplyAttack();
    }

    internal void BeginCombat(Vector2 startPosition, int arenaHeight, int arenaWidth)
    {
        inCombat = true;
        position = startPosition;
        this.arenaHeight = arenaHeight;
        this.arenaWidth = arenaWidth;
    }

    internal void EndCombat()
    {
        inCombat = false;
        EnableInput();
    }

    void ApplyMovement()
    {
        if (action == combatAction.none)
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
        if (dir == direction.down && position.y != 0)
        {
            StartCoroutine(MoveDown());
            position.y -= 1;
        }
        else if (dir == direction.left && position.x != 0)
        {
            StartCoroutine(MoveLeft());
            position.x -= 1;
        }
        else if (dir == direction.up && position.y != (arenaHeight - 1))
        {
            StartCoroutine(MoveUp());
            position.y++;
        }
        else if (dir == direction.right && position.x != (arenaWidth - 1))
        {
            StartCoroutine(MoveRight());
            position.x++;
        }

    }

    void ApplyAttack()
    {
        if (Input.GetKeyDown(KeyCode.Z) && action == combatAction.none)
        {
            StartCoroutine(Punch());
        }
    }

    IEnumerator Punch()
    {
        action = combatAction.punch;
        GameObject playerAttack = Instantiate(playerAttackObject, new Vector2(this.transform.position.x, this.transform.position.y + 0.5f), Quaternion.identity);
        playerAttack.transform.parent = gameObject.transform;
        //player.sound.PlayAudio("punch-swing");
        yield return new WaitForSeconds(punchAnimation.length);
        Destroy(playerAttack);
        action = combatAction.none;
    }

    void ApplyBlock()
    {
        if(action == combatAction.none && Input.GetKey(KeyCode.Space))
        {
            StartCoroutine(Block());
        }
    }

    IEnumerator Block()
    {
        action = combatAction.block;
        while (Input.GetKey(KeyCode.Space))
        {
            yield return null;
        }
        action = combatAction.none;
    }

    IEnumerator MoveLeft()
    {
        float newPosition = Mathf.Ceil(rb.position.x - 0.5f) - 0.5f;
        action = combatAction.moveLeft;
        Debug.Log(newPosition);
        while (rb.position.x > newPosition && action == combatAction.moveLeft)
        {
            moveVelocity = new Vector2(-moveSpeed, 0);
            yield return new WaitForEndOfFrame();
            if (action == combatAction.hurt)
            {
                yield break;
            }
        }
        moveVelocity = Vector2.zero;
        while (rb.position.x != newPosition && lockMovement)
        {
            transform.position = new Vector2(newPosition, rb.position.y);
            yield return new WaitForEndOfFrame();
        }
        action = combatAction.none;
    }
    IEnumerator MoveRight()
    {
        float newPosition = Mathf.Floor(rb.position.x + 0.5f) + 0.5f;
        action = combatAction.moveRight;
        while (rb.position.x < newPosition && action == combatAction.moveRight)
        {
            moveVelocity = new Vector2(moveSpeed, 0);
            yield return new WaitForEndOfFrame();
            if (action == combatAction.hurt)
            {
                yield break;
            }
        }
        moveVelocity = Vector2.zero;
        while(rb.position.x != newPosition && lockMovement)
        {
            transform.position = new Vector2(newPosition, rb.position.y);
            yield return new WaitForEndOfFrame();
        }
        action = combatAction.none;
    }
    IEnumerator MoveUp()
    {
        float newPosition = Mathf.Floor(rb.position.y + 1);
        action = combatAction.moveUp;
        while (rb.position.y < newPosition && action == combatAction.moveUp)
        {
            moveVelocity = new Vector2(0, moveSpeed);
            yield return new WaitForEndOfFrame();
            if (action == combatAction.hurt)
            {
                yield break;
            }
        }
        moveVelocity = Vector2.zero;
        while (rb.position.y != newPosition && lockMovement)
        {
            transform.position = new Vector2(rb.position.x, newPosition);
            yield return new WaitForEndOfFrame();
        }
        action = combatAction.none;
    }
    IEnumerator MoveDown()
    {
        float newPosition = Mathf.Ceil(rb.position.y - 1);
        action = combatAction.moveDown;
        while (rb.position.y > newPosition && action == combatAction.moveDown)
        {
           
            moveVelocity = new Vector2(0, -moveSpeed);
            yield return new WaitForEndOfFrame();
            if (action == combatAction.hurt)
            {
                yield break;
            }
        }
        moveVelocity = Vector2.zero;
        while (rb.position.y != newPosition && lockMovement)
        {
            transform.position = new Vector2(rb.position.x, newPosition);
            yield return new WaitForEndOfFrame();
        }
        action = combatAction.none;
    }

   

    public IEnumerator Hurt(int damage)
    {
        action = combatAction.hurt;
        player.sound.PlayAudio("player-hurt");
        TakeDamage(damage);
        float p = ((float)currentHealth / maxHealth);
        CropPlayerHealthSprite(p);
        float newPosition = Mathf.Floor(rb.position.y - 1);
        while (rb.position.y > newPosition)
        {
            moveVelocity = new Vector2(0, -2 * moveSpeed);
            yield return new WaitForSeconds(0.0000001f);
        }
        moveVelocity = Vector2.zero;
        rb.position = new Vector2(Mathf.Floor(rb.position.x) + 0.5f, newPosition);
        Debug.Log(rb.position);
        yield return new WaitForSeconds(0.25f);
        action = combatAction.none;
    }

    private void ChangeHealth(int amount)
    {
        currentHealth += amount;
        if (currentHealth <= 0)
        {
            Die();
        }
        if (currentHealth >= maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    internal void TakeDamage(int damage)
    {
        ChangeHealth(-damage);
    }

    internal void Heal(int heal)
    {
        ChangeHealth(heal);
    }

    internal void Die()
    {

    }

    internal void DisableInput()
    {
        inputEnabled = false;
    }

    internal void EnableInput()
    {
        inputEnabled = true;
    }
    private void CropPlayerHealthSprite(float percentage)
    {
        Rect r = new Rect(0, 0, (int)Mathf.Ceil(playerHealthBarFull.rect.width * percentage), playerHealthBarFull.rect.height);
        Sprite newSprite = Sprite.Create(playerHealthBarFull.texture, r, new Vector2(0, 0.5f), 16);
        playerHealthBar.sprite = newSprite;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        HandleCollision(collision.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        HandleCollision(collision.gameObject);
    }

    protected virtual void HandleCollision(GameObject collisionObject)
    {
        Debug.Log(this.tag);
        Debug.Log(collisionObject);
        Debug.Log(collisionObject.tag);
        if (collisionObject.CompareTag("Enemy"))
        {
            StartCoroutine(Hurt(collisionObject.GetComponent<EnemyController>().attackDamage));
        }
        if (collisionObject.CompareTag("Enemy Attack"))
        {
            StartCoroutine(Hurt(collisionObject.GetComponentInParent<EnemyController>().attackDamage));
        }
    }

    public enum combatAction { none, punch, moveLeft, moveRight, moveUp, moveDown, block, hurt };
}
