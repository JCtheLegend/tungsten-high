using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActionController : MonoBehaviour
{
    [SerializeField] PlayerMasterController player;
    [SerializeField] CutsceneManager cutsceneManager;

    internal void Interact(RaycastHit2D ray)
    {
        GameObject collider = ray.collider.gameObject;
        if(collider.CompareTag("CutsceneTrigger"))
        {
            if (collider.GetComponent<CutsceneTrigger>().facePlayer)
            {
                collider.GetComponent<Faces>().FacePlayer(player.input.facing);
            }
            StartCutscene(ray.collider.gameObject.GetComponent<CutsceneTrigger>().cutsceneFileName);
        }
        else if(collider.CompareTag("Door"))
        {
            GameManager.RoomData.toEntranceNum = collider.GetComponent<RoomExit>().toEntranceNum;
            GameManager.RoomData.startingCutscene = collider.GetComponent<RoomExit>().startingCutscene;
            // player.sound.PlayAudio("door-open");
            GameManager.debugCounterToggle = false;
            GameManager.LoadScene(collider.GetComponent<RoomExit>().roomName);
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
            StartCoroutine(cutsceneManager.HandleCutscene(collider.GetComponent<SimpleDialogTrigger>().CreateSimpleDialog()));
        }
    }

    internal void StartCutscene(string fileName)
    {
        cutsceneManager.BeginCutscene(fileName);
    }
}
