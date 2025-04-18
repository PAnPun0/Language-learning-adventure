using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;

public class QuestionBlock : MonoBehaviour
{
    public TMP_InputField questionInput;       // поле для текста вопроса
    public Transform optionsContainer;         // контейнер для вариантов ответа
    public GameObject optionPrefab;             // префаб одного варианта ответа
    public Button addOptionButton;              // кнопка "Добавить вариант"

    private void Start()
    {
        if (addOptionButton != null)
        {
            addOptionButton.onClick.AddListener(AddOption);
        }
        else
        {
            Debug.LogWarning("Кнопка AddOptionButton не привязана к QuestionBlock!");
        }
    }

    public void AddOption()
    {
        if (optionPrefab != null && optionsContainer != null)
        {
            Instantiate(optionPrefab, optionsContainer);
        }
        else
        {
            Debug.LogWarning("OptionPrefab или OptionsContainer не назначены!");
        }
    }

    public Dialogue BuildDialogue()
    {
        Dialogue dialogue = new Dialogue();
        dialogue.Question = questionInput.text;
        dialogue.Options = new List<DialogueOption>();

        foreach (Transform child in optionsContainer)
        {
            DraggableOptionItem item = child.GetComponent<DraggableOptionItem>();
            if (item != null)
            {
                DialogueOption option = new DialogueOption
                {
                    Text = item.textInput.text,
                    Translation = item.translationInput.text,
                    IsCorrect = item.isCorrectToggle.isOn
                };
                dialogue.Options.Add(option);
            }
            else
            {
                Debug.LogWarning("Вариант без скрипта DraggableOptionItem найден!");
            }
        }

        return dialogue;
    }
}
