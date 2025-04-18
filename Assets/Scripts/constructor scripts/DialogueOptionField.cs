using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueOptionField : MonoBehaviour
{
    public TMP_InputField optionInput;
    public TMP_InputField translationInput;
    public Toggle isCorrectToggle;

    private DialogueOption option = new DialogueOption();

    public void Init()
    {
        optionInput.onValueChanged.AddListener(val => option.Text = val);
        translationInput.onValueChanged.AddListener(val => option.Translation = val);
        isCorrectToggle.onValueChanged.AddListener(val => option.IsCorrect = val);
    }

    public DialogueOption GetOption()
    {
        option.Text = optionInput.text;
        option.Translation = translationInput.text;
        option.IsCorrect = isCorrectToggle.isOn;
        return option;
    }
}
