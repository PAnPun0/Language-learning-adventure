using UnityEngine;

public class ExitTrigger : MonoBehaviour
{
    public DialogueManager dialogueManager;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            dialogueManager.ShowResult();
            gameObject.SetActive(false);
        }
    }
}
