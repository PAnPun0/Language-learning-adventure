using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DraggableOptionItem : MonoBehaviour
{
    public TMP_InputField textInput;
    public TMP_InputField translationInput;
    public Toggle isCorrectToggle;

    private ConstructedOption optionData;

    public void Setup(ConstructedOption option)
    {
        optionData = option;

        textInput.text = optionData.text;
        translationInput.text = optionData.translation;
        isCorrectToggle.isOn = optionData.isCorrect;

        // Подпишемся на изменения пользователя
        textInput.onValueChanged.AddListener(OnTextChanged);
        translationInput.onValueChanged.AddListener(OnTranslationChanged);
        isCorrectToggle.onValueChanged.AddListener(OnToggleChanged);
    }

    private void OnTextChanged(string newText)
    {
        optionData.text = newText;
    }

    private void OnTranslationChanged(string newTranslation)
    {
        optionData.translation = newTranslation;
    }

    private void OnToggleChanged(bool newValue)
    {
        optionData.isCorrect = newValue;
    }
}
