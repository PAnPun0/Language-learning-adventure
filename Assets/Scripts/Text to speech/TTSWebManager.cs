using UnityEngine;

public class TTSWebManager : MonoBehaviour
{
    public static TTSWebManager Instance;
    public AudioSource audioSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayLocalClip(string clipName)
    {
        AudioClip clip = Resources.Load<AudioClip>("TTS/" + clipName);
        if (clip != null)
        {
            audioSource.clip = clip;
            audioSource.Play();
        }
        else
        {
            Debug.LogError("Аудиофайл не найден: " + clipName);
        }
    }
}
