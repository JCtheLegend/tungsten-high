using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingPerson : MonoBehaviour
{

    AnimatableObject anim;
    // Start is called before the first frame update
    MoveableObject move;
    SpriteRenderer sprite;

    [SerializeField] float walkSpeed;


    void Awake()
    {
        anim = GetComponent<AnimatableObject>();
        sprite = GetComponent<SpriteRenderer>();
        move = GetComponent<MoveableObject>();
    }

    public IEnumerator StartWalking(float endYcoord)
    {
        string animNum = Random.Range(1, 16).ToString();
        Debug.Log(anim);
        anim.Animate(animNum, true);
        StartCoroutine(move.Move(new Vector2(transform.position.x, endYcoord), walkSpeed));
        while (transform.position.y > endYcoord)
        {
            yield return new WaitForEndOfFrame();
        }
        StartCoroutine(GameManager.FadeOut(sprite, 1));
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
}
