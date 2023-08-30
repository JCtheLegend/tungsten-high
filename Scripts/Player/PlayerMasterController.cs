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
}
