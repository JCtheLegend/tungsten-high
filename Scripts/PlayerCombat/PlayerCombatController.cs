using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatController : MonoBehaviour
{
    [SerializeField] PlayerCombatMasterController player;
    [SerializeField] AnimationClip punchAnimation;
    [SerializeField] private float moveSpeed = 1;
    [SerializeField] GameObject playerAttackObject;
    [SerializeField] internal GameObject playerHealthBar;
    [SerializeField] internal SpriteRenderer playerCurrentHealthBar;
    [SerializeField] GameObject enemyHealthBar;
    [SerializeField] SpriteRenderer blackFade;
    [SerializeField] GameObject gameOver;
    Sprite playerHealthBarFull;

    public bool lockMovement = true;

    internal Vector2 moveVelocity;

    [SerializeField] Vector2 resetPos;

    public float arenaMinWidth;
    public float arenaMinHeight;
    public float arenaMaxWidth;
    public float arenaMaxHeight;

    internal bool inCombat = false;
    public combatAction action = combatAction.none;
    private Rigidbody2D rb;

    public int attackDamage;
    public int currentHealth;
    public int maxHealth;

    IEnumerator movingRoutine;
    IEnumerator punchingRoutine;

    SoundController sound;
    MusicController music;

    public Planner p;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerHealthBarFull = playerCurrentHealthBar.sprite;
        sound = GameObject.Find("Sound Manager").GetComponent<SoundController>();
        Debug.Log(sound);
        music = GameObject.Find("Music Manager").GetComponent<MusicController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (inCombat && action != combatAction.dead)
        {
            CombatMove();
        }
        if (Input.GetKeyDown(KeyCode.Escape) && !player.cutscene.inCutscene)
        {
            if (GameManager.sceneCounter >= 7) //FIXME
            {
                if (!p.gameObject.activeInHierarchy)
                {
                    Time.timeScale = 0;
                    DisableInput();
                    p.gameObject.SetActive(true);
                }
                else
                {
                    Time.timeScale = 1;
                    p.gameObject.SetActive(false);
                    EnableInput();
                }
            }
        }
    }

    void CombatMove()
    {
        ApplyMovement();
        ApplyBlock();
        ApplyAttack();
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

    public void StopMovement()
    {
        if (movingRoutine != null)
        {
            StopCoroutine(movingRoutine);
        }
        rb.velocity = Vector2.zero;
        action = combatAction.none;
    }


    private void Move(direction dir)
    {
        if (dir == direction.down && rb.position.y > arenaMinHeight)
        {
            movingRoutine = MoveDown();
            StartCoroutine(movingRoutine);
        }
        else if (dir == direction.left && rb.position.x > arenaMinWidth)
        {
            movingRoutine = MoveLeft();
            StartCoroutine(movingRoutine);
        }
        else if (dir == direction.up && rb.position.y < arenaMaxHeight)
        {
            movingRoutine = MoveUp();
            StartCoroutine(movingRoutine);
        }
        else if (dir == direction.right && rb.position.x < arenaMaxWidth)
        {
            movingRoutine = MoveRight();
            StartCoroutine(movingRoutine);
        }

    }

    void ApplyAttack()
    {
        if (Input.GetKeyDown(KeyCode.Z) && action == combatAction.none)
        {
            punchingRoutine = Punch();
            StartCoroutine(punchingRoutine);
        }
    }

    IEnumerator Punch()
    {
        if (action != combatAction.dead)
        {
            action = combatAction.punch;
            GameObject playerAttack = Instantiate(playerAttackObject, new Vector2(this.transform.position.x, this.transform.position.y + 0.5f), Quaternion.identity);
            playerAttack.transform.parent = gameObject.transform;
            sound.PlayAudio("punch-swing");
            yield return new WaitForSeconds(punchAnimation.length);
            Destroy(playerAttack);
            if (currentHealth > 0)
            {
                action = combatAction.none;
            }
        }
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

    IEnumerator DamageMoveDown()
    {
        float newPosition = rb.position.y;
        action = combatAction.hurt;
        if (rb.position.y > arenaMinHeight + 1)
        {
            newPosition = Mathf.Ceil(rb.position.y - 2);
        }
        else if (rb.position.y > arenaMinHeight)
        {
            newPosition = Mathf.Ceil(rb.position.y - 1);
        }
        while (rb.position.y > newPosition)
        {

            moveVelocity = new Vector2(0, -moveSpeed);
            yield return new WaitForEndOfFrame();
        }
        moveVelocity = Vector2.zero;
        while (rb.position.y != newPosition && lockMovement)
        {
            transform.position = new Vector2(rb.position.x, newPosition);
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(0.5f);
        action = combatAction.none;
    }

   

    public void Hurt(int damage)
    {
        action = combatAction.hurt;
        TakeDamage(damage);
        StartCoroutine(GameManager.FlickerRed(player.anim.sprite));
        float p = ((float)currentHealth / maxHealth);
        CropPlayerHealthSprite(p);
        sound.PlayAudio("player-hurt");
        if (currentHealth <= 0)
        {
            StartCoroutine(Die());
            return;
        }
        StartCoroutine(DamageMoveDown());
    }

    private void ChangeHealth(int amount)
    {
        currentHealth += amount;
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

    IEnumerator Die()
    {

        music.StopMusic();
        sound.PlayAudio("Death");
        enemyHealthBar.SetActive(false);
        playerHealthBar.SetActive(false);
        DisableInput();
        if (movingRoutine != null)
        {
            StopCoroutine(movingRoutine);
        }
        if(punchingRoutine != null)
        {
            StopCoroutine(punchingRoutine);
        }
        rb.velocity = Vector2.zero;
        action = combatAction.dead;
        StartCoroutine(GameManager.FadeIn(blackFade, 0.5f));
        yield return new WaitForSeconds(3);
        StartCoroutine(GameManager.FadeOut(player.anim.sprite, 0.5f));
        player.anim.sprite.sprite = null;
        yield return new WaitForSeconds(3);
        music.ChangeSong("Game Over");
        gameOver.SetActive(true);
        Destroy(gameObject);
    }

    internal void DisableInput()
    {
        inCombat = false;
    }

    internal void EnableInput()
    {
        inCombat = true;
    }
    private void CropPlayerHealthSprite(float percentage)
    {
        if(percentage < 0)
        {
            percentage = 0;
        }
        Rect r = new Rect(0, 0, (int)Mathf.Ceil(playerHealthBarFull.rect.width * percentage), playerHealthBarFull.rect.height);
        Sprite newSprite = Sprite.Create(playerHealthBarFull.texture, r, new Vector2(0, 0.5f), 16);
        playerCurrentHealthBar.sprite = newSprite;
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
        if (collisionObject.CompareTag("Enemy") && action != combatAction.hurt && action != combatAction.dead)
        {
            Hurt(collisionObject.GetComponent<EnemyController>().attackDamage);
        }
        if (collisionObject.CompareTag("Enemy Attack") && action != combatAction.hurt && action != combatAction.dead)
        {
            Hurt(collisionObject.GetComponentInParent<EnemyController>().attackDamage);
        }
    }

    public enum combatAction { none, punch, moveLeft, moveRight, moveUp, moveDown, block, hurt, dead };
}
