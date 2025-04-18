using UnityEngine;
using System;
[System.Serializable]
public class DialogueOption
{
    public string Text;
    public string Translation;
    public bool IsCorrect;
    public AudioClip voiceClip;
}
