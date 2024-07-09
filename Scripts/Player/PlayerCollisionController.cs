using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisionController : MonoBehaviour
{
    [SerializeField] PlayerMasterController player;

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
        if (collisionObject.CompareTag("Enemy Attack"))
        {
            player.combat.Hurt(collisionObject.GetComponentInParent<EnemyController>().attackDamage);
        }
        if (collisionObject.CompareTag("Door") && !player.cutscene.inCutscene)
        {
            GameManager.RoomData.toEntranceNum = collisionObject.GetComponent<RoomExit>().toEntranceNum;
            player.input.DisableInput();
            StartCoroutine(WalkOut(collisionObject.GetComponent<RoomExit>().roomName));
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
