using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutfitChanger : MonoBehaviour
{
    public string outfitName;

    SpriteRenderer spriteRend;
    public Sprite newSprite;

    public PlayerMasterController player;

    private void Start()
    {
        spriteRend = GetComponent<SpriteRenderer>();
    }

    public void ChangeOutfit()
    {
        player.anim.costume = outfitName;
        spriteRend.sprite = newSprite;
    }
}
