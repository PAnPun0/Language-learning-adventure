using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public DialogueManager dialogueManager;

    void OnTriggerEnter(Collider other)
    {
         Debug.Log("OnTriggerEnter: " + other.name);
        if (other.CompareTag("Player"))
        {
            dialogueManager.StartDialogue();
            gameObject.SetActive(false);
        }
    }
}
