using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneTrigger : MonoBehaviour
{
    public string cutsceneFileName = "file.csv";
    public bool destroy = false;
    public bool facePlayer = false;
    public List<string> conditions;
}
