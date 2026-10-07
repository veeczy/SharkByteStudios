using UnityEngine;
using UnityEngine.SceneManagement;

public class Bootstrapper : MonoBehaviour
{
    [SerializeField] string SceneName;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (!PlayerPrefs.HasKey("Game_Volume"))
        {
            PlayerPrefs.SetFloat("Game_Volume", 1.0f);
            PlayerPrefs.Save();
        }





        // once everything is initialized
        SceneManager.LoadScene(SceneName);
    }
}
