// --- DialogueManager.cs ---
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Collections;

public class DialogueManager : MonoBehaviour
{
    public List<Dialogue> dialogues;
    public TextMeshProUGUI questionText;
    public Button[] optionButtons;
    public GameObject resultPanel;
    public TextMeshProUGUI resultText;
    public int hearts = 3;
    public Image[] heartImages;
    public GameObject gameOverPanel;
    public TextMeshProUGUI gameOverText;
    public GameObject dialogueUI;
    public Transform dialogueCameraPoint;
    public Transform playerCamera;
    public PlayerMovement playerMovement;
    public CameraController cameraController;
    

    private int currentDialogueIndex = 0;
    private List<int> mistakes = new List<int>();
    private Vector3 originalCameraPosition;
    private Quaternion originalCameraRotation;
    

    void Start()
    {
        
        TryLoadDialogues();

        if (dialogues == null || dialogues.Count == 0)
        {
            Debug.LogWarning("Нет доступных диалогов для показа.");
            enabled = false;
            return;
        }

        dialogueUI.SetActive(false);
    }

    void TryLoadDialogues()
    {
        if (DialogueDataHolder.Instance != null && DialogueDataHolder.Instance.dialogues.Count > 0)
        {
            dialogues = DialogueDataHolder.Instance.dialogues;
            Debug.Log($"Загружено {dialogues.Count} диалог(ов) из конструктора.");
        }
        else
        {
            Debug.LogWarning("DialogueDataHolder не найден или пуст. Будет использоваться стандартный список диалогов.");
        }
    }

    public void StartDialogue()
    {
        Debug.Log("Starting Dialogue!");
        

        

        
        if (dialogues == null || dialogues.Count == 0)
        {
            Debug.LogWarning("Нельзя начать диалог: нет загруженных диалогов.");
            return;
        }


        originalCameraPosition = playerCamera.position;
        originalCameraRotation = playerCamera.rotation;

        playerMovement.enabled = false;
        cameraController.enabled = false; // ОТКЛЮЧАЕМ КАМЕРУ
        dialogueUI.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        StartCoroutine(MoveCameraToDialoguePoint());
    }
    



    IEnumerator MoveCameraToDialoguePoint()
    {
        float elapsedTime = 0f;
        float duration = 1f;

        Vector3 startPos = playerCamera.position;
        Quaternion startRot = playerCamera.rotation;

        while (elapsedTime < duration)
        {
            playerCamera.position = Vector3.Lerp(startPos, dialogueCameraPoint.position, elapsedTime / duration);
            playerCamera.rotation = Quaternion.Lerp(startRot, dialogueCameraPoint.rotation, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        playerCamera.position = dialogueCameraPoint.position;
        playerCamera.rotation = dialogueCameraPoint.rotation;

        ShowDialogue();
    }

    void ShowDialogue()
    {
        Dialogue dialogue = dialogues[currentDialogueIndex];
        questionText.text = dialogue.Question;


        for (int i = 0; i < optionButtons.Length; i++)
        {
            int index = i;
            if (i < dialogue.Options.Count)
            {
                optionButtons[i].gameObject.SetActive(true);
                optionButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = dialogue.Options[i].Text;
                optionButtons[i].onClick.RemoveAllListeners();
                optionButtons[i].onClick.AddListener(() => OnOptionSelected(index));
            }
            else
            {
                optionButtons[i].gameObject.SetActive(false);
            }
        }
    }

    void OnOptionSelected(int index)
    {
        Dialogue dialogue = dialogues[currentDialogueIndex];
        DialogueOption selected = dialogue.Options[index];

        if (selected.IsCorrect)
        {
            optionButtons[index].image.color = Color.green;
        }
        else
        {
            optionButtons[index].image.color = Color.red;
            ShowTranslation(selected);
            hearts--;
            UpdateHearts();
            mistakes.Add(currentDialogueIndex);

            if (hearts <= 0)
            {
                GameOver();
                return;
            }
        }

        Invoke("NextDialogue", 2f);
    }

    void ShowTranslation(DialogueOption option)
    {
        resultPanel.SetActive(true);
        resultText.text = $"Перевод: {option.Translation}";
    }

    void NextDialogue()
    {
        resultPanel.SetActive(false);
        

        if (++currentDialogueIndex < dialogues.Count)
        {
            ResetOptionColors();
            ShowDialogue();
        }
        else
        {
            EndDialogue();
        }
    }

    void ResetOptionColors()
    {
        foreach (Button btn in optionButtons)
        {
            btn.image.color = Color.white;
        }
    }

    void UpdateHearts()
    {
        for (int i = 0; i < heartImages.Length; i++)
        {
            heartImages[i].enabled = i < hearts;
        }
    }

    void GameOver()
    {
        gameOverPanel.SetActive(true);
        gameOverText.text = "Game Over! Попробуйте снова.";
    }

    void EndDialogue()
    {
        
        StartCoroutine(RestoreCameraAndEnableMovement());
    }

    IEnumerator RestoreCameraAndEnableMovement()
    {
        float elapsedTime = 0f;
        float duration = 1f;

        Vector3 startPos = playerCamera.position;
        Quaternion startRot = playerCamera.rotation;

        while (elapsedTime < duration)
        {
            playerCamera.position = Vector3.Lerp(startPos, originalCameraPosition, elapsedTime / duration);
            playerCamera.rotation = Quaternion.Lerp(startRot, originalCameraRotation, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        playerCamera.position = originalCameraPosition;
        playerCamera.rotation = originalCameraRotation;

        dialogueUI.SetActive(false);

        playerMovement.enabled = true;
        cameraController.enabled = true; // ВКЛЮЧАЕМ КАМЕРУ ОБРАТНО

        // Блокируем курсор обратно
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void ShowResult()
    {
        gameOverPanel.SetActive(true);
        string result = "Вы завершили уровень! Ошибки в ситуациях: \n";
        foreach (int mistake in mistakes)
        {
            result += $"- Ситуация {mistake + 1}\n";
        }
        gameOverText.text = result;
    }
}