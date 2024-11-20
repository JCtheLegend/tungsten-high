using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatController : GridMover
{
    [SerializeField] PlayerCombatMasterController player;
    [SerializeField] AnimationClip punchAnimation;

    [SerializeField] GameObject playerAttackObject;
    internal GameObject playerHealthBar;
    internal SpriteRenderer playerCurrentHealthBar;
    internal GameObject enemyHealthBar;
    SpriteRenderer blackFade;
    [SerializeField] GameObject gameOver;
    [SerializeField] GameObject shield;
    Sprite playerHealthBarFull;

    public bool lockMovement = true;
    [SerializeField] Vector2 resetPos;
    //public bool inCombat = false;
    public bool shielding = false;
    public combatAction action = combatAction.none;
    public int attackDamage;
    public int currentHealth;
    public int maxHealth;


    IEnumerator punchingRoutine;

    SoundController sound;
    MusicController music;

    Planner p;

    bool regenEnergy = true;
    int regenEnergyRate = 2;

    float time = 0;

    protected override void Awake()
    {
        base.Awake();
        playerHealthBar = GameObject.Find("Player Health Bar");
        playerCurrentHealthBar = GameObject.Find("Player Current Health Bar").GetComponent<SpriteRenderer>();
        blackFade = GameObject.Find("BlackFade").GetComponent<SpriteRenderer>();
        enemyHealthBar = GameObject.Find("Enemy Health Bar");
        playerHealthBarFull = playerCurrentHealthBar.sprite;
        p = GameObject.Find("Planner").GetComponent<Planner>();
    }

    private void Start()
    {
        sound = GameObject.Find("Sound Manager").GetComponent<SoundController>();
        music = GameObject.Find("Music Manager").GetComponent<MusicController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (moveEnabled && action != combatAction.dead)
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
        if (regenEnergy)
        {
            time += Time.deltaTime;
            if(time > 1)
            {
                RegenEnergy(regenEnergyRate);
                time = 0;
            }
        }
        if (shielding)
        {

        }
    }

    void CombatMove()
    {
        ApplyBlock();
        ApplyMovement();
        ApplyAttack();
    }

    internal void EndCombat()
    {
        moveEnabled = false;
        EnableInput();
    }

    void ApplyMovement()
    {
        if (action == combatAction.none && !isMoving)
        {
            GridMove();
        }
    }

    public void StopMovement()
    {
        if (movingRoutine != null)
        {
            StopCoroutine(movingRoutine);
        }
        isMoving = false;
        rb.velocity = Vector2.zero;
        moveVelocity = Vector2.zero;
        action = combatAction.none;
    }

    void ApplyAttack()
    {
        if (Input.GetKeyDown(KeyCode.Z) && action == combatAction.none && energy > 10)
        {
            punchingRoutine = Punch();
            StartCoroutine(punchingRoutine);
            UseEnergy(10);
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
        if(Input.GetKey(KeyCode.Space))
        {
            shielding = true;
            shield.SetActive(true);
        }
        else
        {
            shield.SetActive(false);
            shielding = false;
        }
    }




    IEnumerator DamageMoveDown()
    {
        float newPosition = transform.position.y;
        if (transform.position.y > arenaMinHeight + 1)
        {
            newPosition = Mathf.Ceil(transform.position.y - 2);
        }
        else if (transform.position.y > arenaMinHeight)
        {
            newPosition = Mathf.Ceil(transform.position.y - 1);
        }
        while (transform.position.y > newPosition)
        {

            rb.velocity = new Vector2(0, -moveSpeed);
            yield return new WaitForEndOfFrame();
        }
        rb.velocity = Vector2.zero;
        while (transform.position.y != newPosition && lockMovement)
        {
            transform.position = new Vector2(transform.position.x, newPosition);
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(0.25f);
        action = combatAction.none;
    }

   

    public void Hurt(int damage)
    {
        StopMovement();
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

    private void OnTriggerStay2D(Collider2D collision)
    {
        HandleCollision(collision.gameObject);
    }

    protected virtual void HandleCollision(GameObject collisionObject)
    {
        if (collisionObject.CompareTag("Enemy") && action != combatAction.hurt && action != combatAction.dead)
        {
            Hurt(collisionObject.GetComponent<EnemyController>().attackDamage);
        }
        if (collisionObject.CompareTag("Enemy Attack") && action != combatAction.hurt && action != combatAction.dead && !shielding)
        {
            Hurt(collisionObject.GetComponentInParent<EnemyController>().attackDamage);
        }
    }

    public enum combatAction { none, punch, moving, hurt, dead };
}
