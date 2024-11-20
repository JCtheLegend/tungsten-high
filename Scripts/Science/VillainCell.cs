using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillainCell : MonoBehaviour
{
    internal Animator animator;
    private string currentState;
    private int currentLayer;

    public string onDeathScene;

    public ResetPuzzle nextPuzzle;
    bool dead = false;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    internal void ChangeAnimationState(string newState, int layer)
    {
        if (currentState == newState && currentLayer == layer) return;
        currentState = newState;
        currentLayer = layer;
        animator.Play(newState, layer);
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
        if (collisionObject.CompareTag("Player Attack") && !dead)
        {
            dead = true;
            StartCoroutine(Die());
        }
    }

    IEnumerator Die()
    {
        ChangeAnimationState("Villain Cell Die", 0);
        yield return new WaitForSeconds(0.833f);
        if (onDeathScene != null)
        {
            GameObject.Find("Cutscene Manager").GetComponent<CutsceneManager>().BeginCutscene(onDeathScene);
        }
        GameObject.Find("Hero Cell").GetComponent<HeroCell>().RevertToNormal();
        GameObject.Find("Hero Cell").GetComponent<HeroCell>().currentPuzzle = nextPuzzle;
        yield return new WaitForSeconds(5);
        Destroy(this.gameObject);
    }
}
