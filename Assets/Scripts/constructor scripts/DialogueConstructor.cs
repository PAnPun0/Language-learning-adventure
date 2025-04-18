using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class DialogueConstructor : MonoBehaviour
{
    [Header("UI References")]
    public TMP_InputField questionInput;
    public Transform optionsContainer;
    public GameObject optionPrefab;
    public Button addOptionButton;
    public Button saveDialogueButton;
    public Button startGameButton;

    [Header("Runtime")]
    public DialogueManager dialogueManager;
    
    private List<DialogueOption> currentOptions = new List<DialogueOption>();
    private List<Dialogue> customDialogues = new List<Dialogue>();

    void Start()
    {
        addOptionButton.onClick.AddListener(AddOptionField);
        saveDialogueButton.onClick.AddListener(SaveDialogue);
        startGameButton.onClick.AddListener(StartCustomGame);

        AddOptionField(); // начнем с одного варианта
    }

    void AddOptionField()
    {
        GameObject optionGO = Instantiate(optionPrefab, optionsContainer);
        DialogueOptionField field = optionGO.GetComponent<DialogueOptionField>();
        field.Init();
        currentOptions.Add(field.GetOption());
    }

    void SaveDialogue()
    {
        Dialogue newDialogue = new Dialogue();
        newDialogue.Question = questionInput.text;
        newDialogue.Options = new List<DialogueOption>();

        foreach (Transform child in optionsContainer)
        {
            DialogueOptionField field = child.GetComponent<DialogueOptionField>();
            newDialogue.Options.Add(field.GetOption());
        }

        customDialogues.Add(newDialogue);
        ClearForm();
    }

    void StartCustomGame()
    {
        dialogueManager.dialogues = new List<Dialogue>(customDialogues);
        dialogueManager.StartDialogue();
    }

    void ClearForm()
    {
        questionInput.text = "";
        foreach (Transform child in optionsContainer)
        {
            Destroy(child.gameObject);
        }
        currentOptions.Clear();
        AddOptionField();
    }
}
