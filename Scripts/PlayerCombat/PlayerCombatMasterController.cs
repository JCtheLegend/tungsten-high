using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatMasterController : MonoBehaviour
{
    [SerializeField] internal PlayerCombatController combat;
    [SerializeField] internal PlayerCombatAnimationController anim;
    internal CutsceneManager cutscene;
    [SerializeField] internal PlayerCombatDialogController dialog;

    private void Awake()
    {
        cutscene = GameObject.Find("Main Camera").GetComponentInChildren<CutsceneManager>();
    }
}

