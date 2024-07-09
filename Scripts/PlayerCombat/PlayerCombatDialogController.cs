using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatDialogController : MonoBehaviour
{
    [SerializeField] internal PlayerCombatMasterController player;

    private void Update() { 
        if (player.cutscene.inChoice)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                player.cutscene.AlterChoice(false);
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                player.cutscene.AlterChoice(true);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Z) && player.cutscene.inDialog)
        {
            player.cutscene.AdvanceDialog();
        }
    }
}