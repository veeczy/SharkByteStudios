using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public AudioSource audio;
    public void Awake()
    {
        audio = GetComponent<AudioSource>();
    }
    public void PlayGame()
    {
        SceneManager.LoadScene("PT_Hangar");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quitting Game...");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
