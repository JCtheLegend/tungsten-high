using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatAnimationController : MonoBehaviour
{
    [SerializeField] PlayerCombatMasterController player;
    [SerializeField] internal Animator animator;

    [SerializeField] Sprite forwardSprite;
    [SerializeField] Sprite sideSprite;
    [SerializeField] Sprite backSprite;
    internal SpriteRenderer sprite;

    private string currentState;
    private int currentLayer;

    const int playerBaseLayer = 0;

    const string defaultCombat = "default_combat";
    const string combatPunch = "attack_forward_gloves";
    const string combatPunchAlt = "attack_forward_gloves_alt";
    const string combatMoveLeft = "combat_dodge_right_gloves";
    const string combatMoveRight = "combat_dodge_left_gloves";
    const string combatMoveUpDown = "combat_walk_forward";
    const string combatHurt = "combat_hurt";
    const string combatBlock = "combat_block";
    const string dead = "death";

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        sprite.sortingOrder = 32767 - (int)Mathf.Ceil((transform.position.y + 50) * 100);
        switch (player.combat.action)
            {
                case PlayerCombatController.combatAction.punch:
                    if (Random.Range(0, 2) == 1 && currentState != combatPunch)
                    {
                        ChangeAnimationState(combatPunchAlt, playerBaseLayer);
                    }
                    else if (currentState != combatPunchAlt)
                    {
                        ChangeAnimationState(combatPunch, playerBaseLayer);
                    }
                    break;
                case PlayerCombatController.combatAction.moveLeft:
                    ChangeAnimationState(combatMoveRight, playerBaseLayer);
                    break;
                case PlayerCombatController.combatAction.moveRight:
                    ChangeAnimationState(combatMoveLeft, playerBaseLayer);
                    break;
                case PlayerCombatController.combatAction.moveUp:
                case PlayerCombatController.combatAction.moveDown:
                    ChangeAnimationState(combatMoveUpDown, playerBaseLayer);
                    break;
                case PlayerCombatController.combatAction.block:
                    ChangeAnimationState(combatBlock, playerBaseLayer);
                    break;
                case PlayerCombatController.combatAction.hurt:
                    ChangeAnimationState(combatHurt, playerBaseLayer);
                    break;
                case PlayerCombatController.combatAction.dead:
                    sprite.sortingLayerName = "Dialog";
                    ChangeAnimationState(dead, playerBaseLayer);
                    break;
                default:
                    ChangeAnimationState(defaultCombat, playerBaseLayer);
                    break;

            }
    }

    internal void ChangeAnimationState(string newState, int layer)
    {
        if (currentState == newState && currentLayer == layer) return;
        currentState = newState;
        currentLayer = layer;
        animator.Play(newState, layer);
    }
}
