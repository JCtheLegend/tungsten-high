using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VaultButton : MonoBehaviour
{
    public VaultNumber[] nums;
    [SerializeField] Sprite pushedDown;
    [SerializeField] Sprite pushedUp;
    SpriteRenderer sprite;
    public bool isUp;
    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        sprite.sprite = pushedDown;
        foreach (VaultNumber v in nums)
        {
            if (v.enabled)
            {
                if (isUp)
                {
                    v.Increase();
                }
                else
                {
                    v.Decrease();
                }
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        sprite.sprite = pushedUp;
    }
}
