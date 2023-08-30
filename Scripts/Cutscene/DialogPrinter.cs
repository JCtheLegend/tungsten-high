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

    internal IEnumerator PrintText(CutsceneAction d)
    {
        textDone = false;
        if(d.sprite != null && d.sprite != "") {
            speakerSprite.enabled = true;
            if (speakerSprite != null) {
                Sprite[] spriteCol = Resources.LoadAll<Sprite>("Sprites/" + d.sprite);
                speakerSprite.sprite = spriteCol[1];
            }
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
                    StartCoroutine(TypeText(dialogLine, d.text, defaultDelay));
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

    public IEnumerator TypeText(TextMeshPro textLine, string text, float delay)
    {
        for (int i = 0; i < text.Length+1; i++)
        {
            if (advanceText)
            {
                textLine.text = text;
                break;
            }
            textLine.text = text.Substring(0, i);
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
