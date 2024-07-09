using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogPrinter : MonoBehaviour
{
    [SerializeField] TextMeshPro dialogLine;
    [SerializeField] SpriteRenderer speakerSprite;
    [SerializeField] float defaultDelay = 1f;
    public TextMeshPro choice1;
    public TextMeshPro choice2;
    internal bool textDone = false;
    internal bool advanceText = false;

    AudioSource audioSource;
    public AudioClip beep;

    float minPitch = 0.5f;
    float maxPitch = 3;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    internal Sprite FindHead(string name)
    {
        Sprite[] spriteCol = Resources.LoadAll<Sprite>("Heads/Heads");
        List <Sprite> sList = spriteCol.ToList();
        return sList.Find(x => x.name == name);
    }

    internal IEnumerator PrintText(CutsceneAction d)
    {
        textDone = false;
        if(d.name != null && d.name != "") {
            speakerSprite.enabled = true;
            speakerSprite.sprite = FindHead(d.name);
        }
        else
        {
            speakerSprite.enabled = false;
        }
        switch (d.type)
        {
            case "dialog":

                if (d.isTyped)
                {
                    switch (d.name)
                    {
                        case "Dad":
                        case "Gym Teacher":
                        case "Jumper":
                        case "Bubba":
                        case "Nimbus":
                        case "Micycle":
                        case "Moe":
                        case "Maven":
                        case "Sticky":
                        case "Sandman":
                        case "George":
                        case "Ram":
                        case "Stilts":
                        case "Cubert":
                            minPitch = 0.25f;
                            maxPitch = 0.75f;
                            break;
                        case "Mom":
                        case "Slicky":
                        case "Ms Nice":
                        case "Ms Sparks":
                        case "Laser Boy":
                        case "Mirror":
                        case "Principal Fi":
                        case "Old Fi":
                        case "Kitchenette":
                        case "Freshy":
                        case "Marcy":
                        case "Seer":
                        case "Seagull":
                        case "Handstand":
                        case "Paperboy":
                        case "Wally":
                        case "Regina":
                        case "Friend":
                            minPitch = 0.75f;
                            maxPitch = 1.25f;
                            break;
                        case "Bloom":
                        case "Kid Fi":
                        case "Songbird":
                        case "Dreamy":
                        case "Puddle":
                        case "Young Sparks":
                        case "Ivy":
                            minPitch = 1.25f;
                            maxPitch = 1.75f;
                            break;
                    }
                    StartCoroutine(TypeText(dialogLine, d.text, defaultDelay, d.name != null && d.name != ""));
                    while (!textDone)
                    {
                        yield return new WaitForEndOfFrame();
                    }
                }
                else
                {
                    DisplayText(dialogLine, d.text);
                    textDone = true;
                }
                break;
            case "choice":
                DisplayText(choice1, d.choices[0]);
                DisplayText(choice2, d.choices[1]);
                textDone = true;
                break;
            case "dialogAndChoice":
                DisplayText(dialogLine, d.text);
                DisplayText(choice1, d.choices[0]);
                DisplayText(choice2, d.choices[1]);
                textDone = true;
                break;
        }
    }

    public IEnumerator TypeText(TextMeshPro textLine, string text, float delay, bool sound)
    {
        for (int i = 1; i < text.Length+1; i++)
        {
            if (advanceText)
            {
                textLine.text = text;
                break;
            }
            textLine.text = text.Substring(0, i);
            if (i % 2 == 0 && text[i-1] != ' ' && sound)
            {
                audioSource.Stop();
                audioSource.pitch = Random.Range(minPitch, maxPitch);
                audioSource.PlayOneShot(beep);
            }
            yield return new WaitForSeconds(delay);
        }
        advanceText = false;
        textDone = true;
    }
    
    public void DisplayText(TextMeshPro textLine, string text)
    {
        textLine.text = text;
    }

    public void ClearDialog()
    {
        dialogLine.text = "";
        choice1.text = "";
        choice2.text = "";
        choice1.fontStyle = TMPro.FontStyles.Underline;
        choice2.fontStyle = TMPro.FontStyles.Normal;
    }
}
