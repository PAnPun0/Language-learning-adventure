using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void PlayDeveloperLevels()
    {
        // Здесь переход на сцену с готовыми диалогами разработчика
        SceneManager.LoadScene("NPCscene"); // <-- ты можешь назвать свою сцену как хочешь
    }

    public void OpenConstructor()
    {
        // Здесь переход на конструктор
        SceneManager.LoadScene("Constructor");
    }

    public void QuitGame()
    {
        Debug.Log("Выход из игры...");
        Application.Quit();
    }
}
