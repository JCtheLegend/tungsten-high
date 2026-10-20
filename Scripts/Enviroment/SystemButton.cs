using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

// The Save / Reload / Save and Quit buttons on the Planner's System page.
//
// NOTE: this class used to live in PlannerPage.cs, where Unity could not attach it -- every
// MonoBehaviour reference to that file project-wide resolves to PlannerPage, so the System page
// buttons were plain PlannerPages whose Select() does nothing. Now that it has its own file it can
// be attached; the three buttons in Prefabs/Special Prefabs/Planner.prefab need their component
// swapped from PlannerPage to SystemButton for saving and quitting to actually work.
public class SystemButton : PlannerSelect
{

    public override void Select()
    {
        if (type == ButtonType.reload)
        {
            StartCoroutine(Load(SceneManager.GetActiveScene().name));
        }
        else if (type == ButtonType.save)
        {
            GameManager.SaveGameData();
        }
        else if (type == ButtonType.saveQuit)
        {
            StartCoroutine(Load("Menu"));
        }
        Close();
    }

    public void Close()
    {
        // A fight scene has no world Player -- the Planner is opened there by PlayerCombatController.
        if (PlayerMasterController.Instance != null)
        {
            PlayerMasterController.Instance.input.EnableInput();
        }
        this.transform.parent.gameObject.SetActive(false);
    }

    IEnumerator Load(string s)
    {
        GameManager.SaveGameData();
        MusicController music = MusicController.Instance;
        if (music != null)
        {
            StartCoroutine(music.FadeOut(1));
        }
        SpriteRenderer fade = GameObject.Find("BlackFade").GetComponent<SpriteRenderer>();
        fade.sortingLayerName = "Dialog";
        fade.sortingOrder = 10;
        StartCoroutine(GameManager.FadeIn(fade, 1));
        yield return new WaitForSeconds(1.5f);
        if (music != null)
        {
            music.StopMusic();
        }
        GameManager.LoadScene(s);
    }
}
