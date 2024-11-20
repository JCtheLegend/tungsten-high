using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VaultReset : MonoBehaviour
{
    [SerializeField] VaultNumber[] nums;

    [SerializeField] Sprite pushedDown;
    [SerializeField] Sprite pushedUp;
    SpriteRenderer sprite;

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    void Reset()
    {
        for (int i = 0; i < nums.Length; i++)
        {
            nums[i].val = 0;
            nums[i].text.text = "0";
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        sprite.sprite = pushedDown;
        Reset();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        sprite.sprite = pushedUp;
    }
}
