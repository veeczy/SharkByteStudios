using UnityEngine;
using UnityEngine.SceneManagement;

public class Bootstrapper : MonoBehaviour
{
    [SerializeField] string SceneName;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {

        if (!PlayerPrefs.HasKey("Game_Volume"))
        {
            //Debug.Log("doesnt have key");

            PlayerPrefs.SetFloat("Game_Volume", 1.0f);
        }

        else
        {
            //Debug.Log("has key");

            float dumb = PlayerPrefs.GetFloat("Game_Volume");

            PlayerPrefs.SetFloat("Game_Volume", dumb);
        }


        // once everything is initialized
        SceneManager.LoadScene(SceneName);
    }
}
