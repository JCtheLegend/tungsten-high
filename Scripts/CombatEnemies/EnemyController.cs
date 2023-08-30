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
    [SerializeField] string defaultAnimation;
    [SerializeField] string arriveAnimationX;
    [SerializeField] string arriveAnimationY;
    [SerializeField] SpriteRenderer enemyHealthBar;
    [SerializeField] SoundController sound;

    [SerializeField] private float moveSpeed = 1;
    internal Vector2 moveVelocity;
    public int currentHealth;
    public int maxHealth;
    public int attackDamage;
    public int collisionDamage;
    Sprite enemyHealthBarFull;

    combatState state;

    protected Rigidbody2D rb;

    protected virtual void Start()
    {
        enemyHealthBarFull = enemyHealthBar.sprite;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        sprite.sortingOrder = 32767 - (int)Mathf.Ceil((rb.position.y + 50) * 100);
        if (state == combatState.hurt)
        {
            ChangeAnimationState(hurtAnimation, 0);
        }
        else if (state == combatState.arrivedX)
        {
            ChangeAnimationState(arriveAnimationX, 0);
        }
        else if (state == combatState.arrivedY)
        {
            ChangeAnimationState(arriveAnimationY, 0);
        }
        rb.velocity = moveVelocity;
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
        if (collisionObject.tag == "Player Attack" && state == combatState.none)
        {
            StartCoroutine(Hurt(collisionObject.gameObject.GetComponentInParent<PlayerCombatController>().attackDamage));
        }
    }
    protected void ChangeAnimationState(string newState, int layer)
    {
        if (currentState == newState && currentLayer == layer) return;
        currentState = newState;
        currentLayer = layer;
        animator.Play(newState, layer);
    }

    protected enum combatState { none, attacking, hurt, arrivedX, arrivedY };

    IEnumerator Hurt(int damage)
    {
        state = combatState.hurt;
        TakeDamage(damage);
        Debug.Log(currentHealth);
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

    internal virtual void Die()
    {

    }

    private void CropEnemyHealthSprite(float percentage)
    {
        Rect r = new Rect(0, 0, (int)Mathf.Ceil(enemyHealthBarFull.rect.width * percentage), enemyHealthBarFull.rect.height);
        Sprite newSprite = Sprite.Create(enemyHealthBarFull.texture, r, new Vector2(0, 0.5f), 16);
        enemyHealthBar.sprite = newSprite;
    }
}
