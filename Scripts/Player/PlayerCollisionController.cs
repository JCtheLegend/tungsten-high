using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisionController : MonoBehaviour
{
    [SerializeField] PlayerMasterController player;
    public BoxCollider2D col;
    private void Awake()
    {
        col = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        HandleCollision(collision.gameObject, true);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        HandleCollision(collision.gameObject, false);
    }

    protected virtual void HandleCollision(GameObject collisionObject, bool fromTrigger)
    {
        if (collisionObject.CompareTag("Enemy Attack"))
        {
            player.combat.Hurt(collisionObject.GetComponentInParent<EnemyController>().attackDamage);
        }
        // Only walk-through exits transition on contact. Both door prefabs are tagged "Door", but
        // Exit.prefab's collider is a trigger while Door.prefab's is solid; a solid door is meant to
        // be used with Z (PlayerActionController.Interact). Walking one out is impossible -- the
        // collider we are driving into is the one blocking us -- so the player just stalls against it
        // until the scene loads. Leave those to the interact path.
        if (collisionObject.CompareTag("Door") && fromTrigger && !player.cutscene.inCutscene)
        {
            RoomExit exit = collisionObject.GetComponent<RoomExit>();
            GameManager.RoomData.toEntranceNum = exit.toEntranceNum;
            GameManager.RoomData.toEntranceId = exit.toEntranceId;
            player.input.DisableInput();
            // Keep walking the way we were actually headed so the player strolls out of frame rather
            // than stalling the moment input is cut.
            player.movement.BeginWalkOut(player.movement.rb.velocity);
            StartCoroutine(WalkOut(exit.roomName));
        }
        if (collisionObject.CompareTag("CollideCutsceneTrigger") && !player.cutscene.inCutscene)
        {
            bool startCutscene = false;
            CutsceneTrigger cutsceneTrigger = collisionObject.GetComponent<CutsceneTrigger>();
            if (cutsceneTrigger.conditions.Contains("notDressed")) {
                if (player.anim.costume == "pjs")
                {
                    startCutscene = true;
                }
            }
            else
            {
                startCutscene = true;
            }
            if (startCutscene) {
                if (cutsceneTrigger.destroy)
                {
                    Destroy(collisionObject.GetComponent<CutsceneTrigger>());
                    collisionObject.tag = "Untagged";
                }
                player.action.StartCutscene(cutsceneTrigger.cutsceneFileName);
            }
        }
    }

    IEnumerator WalkOut(string roomName)
    {
        yield return new WaitForSeconds(1);
        StartCoroutine(player.TransitionRoom(roomName));
    }
}
