using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMasterController : MonoBehaviour
{
    [SerializeField] internal PlayerInputController input;
    [SerializeField] internal PlayerMovementController movement;
    [SerializeField] internal PlayerCombatController combat;
    [SerializeField] internal PlayerActionController action;
    [SerializeField] internal PlayerAnimationController anim;
    [SerializeField] internal SoundController sound;
    [SerializeField] internal CutsceneManager cutscene;

    public IEnumerator TransitionRoom(string roomName)
    {
        SpriteRenderer s = GameObject.Find("BlackFade").GetComponent<SpriteRenderer>();
        StartCoroutine(GameManager.FadeIn(s, 1));
        while (s.color.a < 1)
        {
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(1);
        GameManager.LoadScene(roomName);
    }
}
