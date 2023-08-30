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
            StartCoroutine(player.combat.Hurt(collisionObject.GetComponentInParent<EnemyController>().attackDamage));
        }
        if (collisionObject.CompareTag("Door"))
        {
            GameManager.RoomData.toEntranceNum = collisionObject.GetComponent<RoomExit>().toEntranceNum;
            GameManager.RoomData.startingCutscene = collisionObject.GetComponent<RoomExit>().startingCutscene;
            GameManager.debugCounterToggle = false;
            GameManager.LoadScene(collisionObject.GetComponent<RoomExit>().roomName);
        }
        if (collisionObject.CompareTag("CollideCutsceneTrigger"))
        {
            bool startCutscene = false;
            CutsceneTrigger cutsceneTrigger = collisionObject.GetComponent<CutsceneTrigger>();
            if (cutsceneTrigger.conditions.Contains("notDressed")) {
                if (player.anim.inPjs)
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
                    Destroy(collisionObject);
                }
                player.action.StartCutscene(cutsceneTrigger.cutsceneFileName);
            }
        }
    }
}
