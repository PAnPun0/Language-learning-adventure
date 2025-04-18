using System.Collections.Generic;
using UnityEngine;

public class DialogueDataHolder : MonoBehaviour
{
    public static DialogueDataHolder Instance;

    public List<Dialogue> dialogues = new List<Dialogue>();

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
