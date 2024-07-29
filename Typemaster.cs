using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Typemaster : MonoBehaviour
{

    bool typingEnabled = true;
    public int totalChars;
    int charsRemaining;

    public SpriteRenderer progressHider;

    TextMeshPro typeText;

    public GameObject alertBox;

    public TextMeshPro alertHeader;
    public TextMeshPro alertText;
    TextMeshPro alertCounter;

    bool copy = false;
    bool paste = false;
    bool backspace = false;

    // Start is called before the first frame update
    void Start()
    {
        charsRemaining = totalChars;
    }

    // Update is called once per frame
    void Update()
    {
        copy = false;
        paste = false;
        if (Input.anyKeyDown && typingEnabled)
        {
            charsRemaining--;
            CropProgress();
        }
        if(charsRemaining == 800)
        {
            StartCoroutine(CopyPaste("Copy Paste", "Alternate pressing Ctl + C and Ctl + V", 10));
            charsRemaining--;
        }
        if(Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.C))
        {
            copy = true;
        }
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.V))
        {
            paste = true;
        }
        if (Input.GetKey(KeyCode.Backspace))
        {
            backspace = true;
        }
    }

    IEnumerator CopyPaste(string message, string description, int times)
    {
        typingEnabled = false;
        alertHeader.text = message;
        alertText.text = description;
        alertBox.SetActive(true);
        int counter = 0;

        while(counter < times)
        {
            while (!copy)
            {
                yield return new WaitForEndOfFrame();

            }
            while (!paste)
            {
                yield return new WaitForEndOfFrame();

            }
            counter++;
            Debug.Log(counter);
        }
        alertBox.SetActive(false);
        typingEnabled = true;
    }

    IEnumerator BackSpace(string message, string description, int times)
    {
        typingEnabled = false;
        alertHeader.text = message;
        alertText.text = description;
        alertBox.SetActive(true);
        int counter = 0;

        while (counter < times)
        {
            while (!backspace)
            {
                yield return new WaitForEndOfFrame();

            }
            counter++;
            Debug.Log(counter);
        }
        alertBox.SetActive(false);
        typingEnabled = true;
    }

    void CropProgress()
    {
        float percentage = (charsRemaining * 1.0f) / (totalChars * 1.0f);
        progressHider.transform.localScale = new Vector3(percentage, 1, 1);
    }
}
