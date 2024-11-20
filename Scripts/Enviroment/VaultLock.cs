using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VaultLock : MonoBehaviour
{

    [SerializeField] int[] combination;
    [SerializeField] VaultNumber[] nums;

    [SerializeField] Sprite pushedDown;
    [SerializeField] Sprite pushedUp;
    SpriteRenderer sprite;
    [SerializeField] string cutscene;

    CutsceneManager cutsceneManager;

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        cutsceneManager = GameObject.Find("Cutscene Manager").GetComponent<CutsceneManager>();
    }

    void Check()
    {
        bool valid = true;
        for (int i = 0; i < combination.Length; i++)
        {
            if(combination[i] != nums[i].val)
            {
                valid = false;
            }
        }
        if (valid)
        {
            Solved();
        }
        else
        {
            StartCoroutine(Error());
        }
    }

    void Solved()
    {
        foreach (VaultNumber v in nums)
        {
            v.enabled = false;
            v.ChangeColor(Color.green);
        }
        cutsceneManager.BeginCutscene(cutscene);
    }

    IEnumerator Error()
    {
        foreach (VaultNumber v in nums)
        {
            v.enabled = false;
            v.ChangeColor(Color.red);
        }
        yield return new WaitForSeconds(0.25f);
        foreach (VaultNumber v in nums)
        {
            v.ChangeColor(Color.white);
        }
        yield return new WaitForSeconds(0.25f);
        foreach (VaultNumber v in nums)
        {
            v.ChangeColor(Color.red);
        }
        yield return new WaitForSeconds(0.25f);
        foreach (VaultNumber v in nums)
        {
            v.ChangeColor(Color.white);
        }
        yield return new WaitForSeconds(0.25f);
        foreach (VaultNumber v in nums)
        {
            v.ChangeColor(Color.red);
        }
        yield return new WaitForSeconds(0.25f);
        foreach (VaultNumber v in nums)
        {
            v.enabled = true;
            v.ChangeColor(Color.white);
        }
        yield return new WaitForSeconds(0.25f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        sprite.sprite = pushedDown;
        Check();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        sprite.sprite = pushedUp;
    }
}
