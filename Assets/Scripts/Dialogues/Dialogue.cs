using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Dialogue
{
    public string Question;
    
    public List<DialogueOption> Options;
    public AudioClip voiceClip; // <--- Добавляем сюда
}