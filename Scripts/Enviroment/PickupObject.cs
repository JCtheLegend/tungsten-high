using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupObject : MonoBehaviour
{
    public int number;
    public string inventoryName;
    public bool destroy;
    public bool obtained;
    public string[] foundDialog;
    public string[] alreadyFoundDialog;

    public void Pickup()
    {
        PlayerMasterController player = GameObject.Find("Player").GetComponent<PlayerMasterController>();
        PlayerInventory.UpdateInventory(inventoryName, number);
        List<CutsceneAction> list = new List<CutsceneAction>();
        if (!obtained)
        {
            PickupManager.UpdatePickup(gameObject.name, gameObject.transform.position.x.ToString() + ',' + gameObject.transform.position.y.ToString());
            list = CreatePickupDialog(foundDialog);
        }
        else
        {
            list = CreatePickupDialog(alreadyFoundDialog);
        }
        StartCoroutine(player.cutscene.HandleCutscene(list));
        if (destroy)
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            this.enabled = false;
        }
    }

    public List<CutsceneAction> CreatePickupDialog(string[] dialog)
    {
        List<CutsceneAction> l = new List<CutsceneAction>
        {
            new CutsceneAction("disableMovement")
        };
        for (int i = 0; i < dialog.Length; i++)
        {
            CutsceneAction c;
            c = new CutsceneAction(null, null, dialog[i], false);
            l.Add(c);
        }
        l.Add(new CutsceneAction("closeDialog"));
        l.Add(new CutsceneAction("enableMovement"));
        return l;
    }
}
