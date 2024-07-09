using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightCard : MonoBehaviour
{
    [SerializeField] GameObject redCard;
    [SerializeField] GameObject blueCard;
    [SerializeField] SpriteRenderer lightning;
    [SerializeField] SpriteRenderer playerPic;
    [SerializeField] SpriteRenderer enemyPic;
    [SerializeField] SpriteRenderer blackFade;

    Animator anim;

    AudioSource a;
    public AudioClip clip;

    public Vector2 redCoords1;
    public Vector2 blueCoords1;
    public Vector2 redCoords2;
    public Vector2 blueCoords2;

    public int moveSpeed = 5;
    public int fadeSpeed = 2;

    private void Start()
    {
        a = GetComponent<AudioSource>();
    }

    public IEnumerator DisplayTitleCard()
    {
        StartCoroutine(redCard.GetComponent<MoveableObject>().Move(redCoords1, moveSpeed));
        StartCoroutine(blueCard.GetComponent<MoveableObject>().Move(blueCoords1, moveSpeed));
        yield return new WaitForSeconds(3);
        anim = GetComponent<Animator>();
        anim.Play("fight_card_lightning");
        a.PlayOneShot(clip);
        yield return new WaitForSeconds(3);
        StartCoroutine(redCard.GetComponent<MoveableObject>().Move(redCoords2, moveSpeed));
        StartCoroutine(blueCard.GetComponent<MoveableObject>().Move(blueCoords2, moveSpeed));
        StartCoroutine(GameManager.FadeOut(playerPic, fadeSpeed));
        StartCoroutine(GameManager.FadeOut(enemyPic, fadeSpeed));
        StartCoroutine(GameManager.FadeOut(lightning, fadeSpeed));
        StartCoroutine(GameManager.FadeOut(blackFade, fadeSpeed));
    }
}
