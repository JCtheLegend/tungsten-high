using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatMasterController : MonoBehaviour
{
    [SerializeField] internal PlayerCombatController combat;
    [SerializeField] internal PlayerCombatAnimationController anim;
    [SerializeField] internal CutsceneManager cutscene;
    [SerializeField] internal PlayerCombatDialogController dialog;
}
