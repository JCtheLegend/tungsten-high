using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PlannerSelect : MonoBehaviour
{
    public PlannerSelect up;
    public PlannerSelect down;
    public PlannerSelect left;
    public PlannerSelect right;
    public TextMeshPro text;

    public Sprite hoveredSprite;
    public Sprite unhoveredSprite;

    public Color hoveredTextColor;
    public Color unhoveredTextColor;

    public SpriteRenderer sprite;
    public virtual void Hover()
    {
        if (hoveredSprite != null)
        {
            sprite.sprite = hoveredSprite;
        }
        if (text)
        {
            text.color = hoveredTextColor;
        }
    }
    public enum ButtonType { none, save, reload, saveQuit, close };

    public ButtonType type;

    public virtual void Dehover()
    {
        if (unhoveredSprite != null)
        {
            sprite.sprite = hoveredSprite;
        }
        if (text)
        {
            text.color = unhoveredTextColor;
        }
    }
    public virtual void Select()
    {
    }
}

public class PlannerPage : PlannerSelect
{
    public GameObject[] pageObjects;

    public override void Hover()
    {
        base.Hover();
        foreach (GameObject g in pageObjects)
        {
            g.SetActive(true);
        }
        if (left)
        {
            foreach (GameObject g in ((PlannerPage)left).pageObjects)
            {
                g.SetActive(false);
            }
        }
        if (right)
        {
            foreach (GameObject g in ((PlannerPage)right).pageObjects)
            {
                g.SetActive(false);
            }
        }

    }
}
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
        GameObject.Find("Player").GetComponent<PlayerInputController>().EnableInput();
        this.transform.parent.gameObject.SetActive(false);
    }

    IEnumerator Load(string s)
    {
        GameManager.SaveGameData();
        StartCoroutine(GameObject.Find("Music Manager(Clone)").GetComponent<MusicController>().FadeOut(1));
        GameObject.Find("BlackFade").GetComponent<SpriteRenderer>().sortingLayerName = "Dialog";
        GameObject.Find("BlackFade").GetComponent<SpriteRenderer>().sortingOrder = 10;
        StartCoroutine(GameManager.FadeIn(GameObject.Find("BlackFade").GetComponent<SpriteRenderer>(), 1));
        yield return new WaitForSeconds(1.5f);
        GameObject.Find("Music Manager(Clone)").GetComponent<MusicController>().StopMusic();
        GameManager.LoadScene(s);
    }
}

