using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    protected Animator animator;
    protected string currentState;
    protected int currentLayer;
    protected SpriteRenderer sprite;

    [SerializeField] string hurtAnimation;
    [SerializeField] string deadAnimation;
    [SerializeField] string defaultAnimation;

    [SerializeField] internal GameObject enemyHealthBar;
    [SerializeField] internal SpriteRenderer enemyCurrentHealthBar;
    [SerializeField] CutsceneManager cutscene;
    [SerializeField] FightCard fightCard;
    public PlayerCombatController player;

    [SerializeField] protected float moveSpeed = 1;

    [SerializeField] internal string onDeathCutscene;

    internal Vector2 moveVelocity;
    public int currentHealth;
    public int maxHealth;
    public int attackDamage;
    public int collisionDamage;
    Sprite enemyHealthBarFull;


    protected combatState state;

    protected Rigidbody2D rb;

    internal MusicController music;
    internal SoundController sound;
    internal MoveableObject move;

    protected virtual void Start()
    {
        enemyHealthBarFull = enemyCurrentHealthBar.sprite;
        music = GameObject.Find("Music Manager").GetComponent<MusicController>();
        sound = GetComponent<SoundController>();
    }

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        move = GetComponent<MoveableObject>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (state == combatState.dead)
        {
            ChangeAnimationState(deadAnimation, 0);
        }
        else if (state == combatState.hurt)
        {
            ChangeAnimationState(hurtAnimation, 0);
        }
        else
        {
            ChangeAnimationState(defaultAnimation, 0);
        }
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
        if (collisionObject.CompareTag("Player Attack") && state == combatState.none)
        {
            StartCoroutine(Hurt(collisionObject.GetComponentInParent<PlayerCombatController>().attackDamage));
        }
    }
    protected void ChangeAnimationState(string newState, int layer)
    {
        if (newState == null || newState == "") return;
        if (currentState == newState && currentLayer == layer) return;
        currentState = newState;
        currentLayer = layer;
        animator.Play(newState, layer);
    }

    protected enum combatState { none, attacking, hurt, arrivedX, arrivedY, dead };

    protected virtual IEnumerator Hurt(int damage)
    {
        state = combatState.hurt;
        TakeDamage(damage);
        sound.PlayAudio("punch-hit");
        float p = ((float)currentHealth / maxHealth);
        CropEnemyHealthSprite(p);
        yield return new WaitForSeconds(0.25f);
        state = combatState.none;
    }

    private void ChangeHealth(int amount)
    {
        currentHealth += amount;
        if (currentHealth <= 0)
        {
            foreach(SpriteRenderer s in enemyHealthBar.GetComponentsInChildren<SpriteRenderer>())
            {
                StartCoroutine(GameManager.FadeOut(s, 1));
            }
            foreach(SpriteRenderer s in player.playerHealthBar.GetComponentsInChildren<SpriteRenderer>())
            {
                StartCoroutine(GameManager.FadeOut(s, 1));
            }
            StartCoroutine(Die());
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

    internal virtual IEnumerator Die()
    {
        yield return new WaitForSeconds(1);
        if (onDeathCutscene != "")
        {
            cutscene.BeginCutscene(onDeathCutscene);
        }
    }

    internal virtual void StartMusic()
    {
        //music.ChangeSong("Fight Music 1");
    }

    internal virtual IEnumerator StartFight()
    {
        //StartCoroutine(fightCard.DisplayTitleCard());
        CropEnemyHealthSprite(1);
        yield return null;
    }

    internal void CropEnemyHealthSprite(float percentage)
    {
        if(percentage < 0)
        {
            percentage = 0;
        }
        Rect r = new Rect(0, 0, (int)Mathf.Ceil(enemyHealthBarFull.rect.width * percentage), enemyHealthBarFull.rect.height);
        Sprite newSprite = Sprite.Create(enemyHealthBarFull.texture, r, new Vector2(0, 0.5f), 16);
        enemyCurrentHealthBar.sprite = newSprite;
    }
}
