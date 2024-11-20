using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActionController : MonoBehaviour
{
    [SerializeField] PlayerMasterController player;

    internal void Interact(RaycastHit2D ray)
    {
        GameObject collider = ray.collider.gameObject;
        if (collider.CompareTag("CutsceneTrigger"))
        {
            if (collider.GetComponent<CutsceneTrigger>() != null)
            {
                if (collider.GetComponent<CutsceneTrigger>().facePlayer)
                {
                    collider.GetComponent<Faces>().FacePlayer(player.input.facing);
                }             
                StartCutscene(ray.collider.gameObject.GetComponent<CutsceneTrigger>().cutsceneFileName);
                if (collider.GetComponent<CutsceneTrigger>().destroy)
                {
                    Destroy(collider.gameObject.GetComponent<CutsceneTrigger>(), 0.5f);
                }
            }
        }
        else if (collider.CompareTag("Door"))
        {
            if (collider.GetComponent<RoomExit>().locked)
            {
                StartCutscene("LockedDoor");
            }
            else
            {
                GameManager.RoomData.toEntranceNum = collider.GetComponent<RoomExit>().toEntranceNum;
                // player.sound.PlayAudio("door-open");
                player.input.DisableInput();
                StartCoroutine(player.TransitionRoom(collider.GetComponent<RoomExit>().roomName));
            }
        }
        else if (collider.CompareTag("OutfitChanger"))
        {
            collider.GetComponent<OutfitChanger>().ChangeOutfit();
        }
        else if (collider.CompareTag("SimpleDialogTrigger"))
        {
            if (collider.GetComponent<SimpleDialogTrigger>().facePlayer)
            {
                collider.GetComponent<Faces>().FacePlayer(player.input.facing);
            }
            GameObject g = GameObject.Find("ConversationTracker");
            if (g != null)
            {
                g.GetComponent<ConversationTracker>().AddConvo(collider.name);
            }
            StartCoroutine(player.cutscene.HandleCutscene(collider.GetComponent<SimpleDialogTrigger>().CreateSimpleDialog()));
        }
        else if (collider.CompareTag("Pickup"))
        {
            collider.GetComponent<PickupObject>().Pickup();
        }
    }

    

    internal void StartCutscene(string fileName)
    {
        player.cutscene.BeginCutscene(fileName);
    }
}
