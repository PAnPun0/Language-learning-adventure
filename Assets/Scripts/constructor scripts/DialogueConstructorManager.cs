using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.IO;

public class DialogueConstructorManager : MonoBehaviour
{
    public Transform levelContainer; // Контейнер для всех вопросов
    public GameObject questionBlockPrefab; // Префаб одного вопроса
    public Button addQuestionButton; // Кнопка "Добавить вопрос"
    public Button saveButton; // Кнопка "Сохранить"
    public Button playTestButton;
    public Button loadLevelButton;
     // Кнопка "Играть"

    private List<QuestionBlock> questionBlocks = new List<QuestionBlock>();

    private void Start()
    {
        addQuestionButton.onClick.AddListener(AddQuestion);
        saveButton.onClick.AddListener(SaveLevel);
        playTestButton.onClick.AddListener(PlayTest);
        loadLevelButton.onClick.AddListener(LoadLevel);
        
    }

    void AddQuestion()
    {
        GameObject questionGO = Instantiate(questionBlockPrefab, levelContainer);
        QuestionBlock qb = questionGO.GetComponent<QuestionBlock>();
        if (qb != null)
        {
            questionBlocks.Add(qb);
        }
        else
        {
            Debug.LogError("Prefab QuestionBlock не содержит скрипт QuestionBlock!");
        }
         LayoutRebuilder.ForceRebuildLayoutImmediate(levelContainer.GetComponent<RectTransform>());
    }

    void SaveLevel()
    {
        List<Dialogue> dialogues = new List<Dialogue>();

        foreach (QuestionBlock qb in questionBlocks)
        {
            Dialogue dialogue = qb.BuildDialogue();
            dialogues.Add(dialogue);
        }

        DialogueList dialogueList = new DialogueList { dialogues = dialogues };
        string json = JsonUtility.ToJson(dialogueList, true);

    // Вместо Debug.Log — сохраняем в файл
        string path = Application.persistentDataPath + "/saved_level.json";
        System.IO.File.WriteAllText(path, json);

        Debug.Log("Уровень сохранён! Путь: " + path);
    }
    void LoadLevel()
    {
        string path = Application.persistentDataPath + "/saved_level.json";
    
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            DialogueList dialogueList = JsonUtility.FromJson<DialogueList>(json);

            ClearLevel(); // Очистить старые вопросы перед загрузкой

            foreach (Dialogue dialogue in dialogueList.dialogues)
            {
                GameObject questionGO = Instantiate(questionBlockPrefab, levelContainer);
                QuestionBlock qb = questionGO.GetComponent<QuestionBlock>();
                if (qb != null)
                {
                    qb.questionInput.text = dialogue.Question;

                    foreach (DialogueOption option in dialogue.Options)
                    {
                        GameObject optionGO = Instantiate(qb.optionPrefab, qb.optionsContainer);
                        DraggableOptionItem item = optionGO.GetComponent<DraggableOptionItem>();
                        if (item != null)
                        {
                            item.textInput.text = option.Text;
                            item.translationInput.text = option.Translation;
                            item.isCorrectToggle.isOn = option.IsCorrect;
                        }
                    }

                 questionBlocks.Add(qb);
                }
            }

            Debug.Log("Уровень успешно загружен!");
        }
        else
        {
            Debug.LogWarning("Файл уровня не найден по пути: " + path);
        }
    }
    void ClearLevel()
    {
        foreach (Transform child in levelContainer)
        {
            Destroy(child.gameObject);
        }
        questionBlocks.Clear();
    }

    void PlayTest()
    {
        if (DialogueDataHolder.Instance != null)
        {
            DialogueDataHolder.Instance.dialogues = new List<Dialogue>();

            foreach (QuestionBlock qb in questionBlocks)
            {
                Dialogue dialogue = qb.BuildDialogue();
                DialogueDataHolder.Instance.dialogues.Add(dialogue);
            }

            SceneManager.LoadScene("NPCscene"); // <-- Сцена игры с NPC
        }
        else
        {
            Debug.LogError("DialogueDataHolder.Instance == null! Нельзя запустить тест.");
        }
    }
}
