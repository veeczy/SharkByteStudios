using UnityEngine;

public class PlayerPrefsManager : MonoBehaviour
{
    void Awake()
    {
        if (!PlayerPrefs.HasKey("Game_Volume"))
        {
            PlayerPrefs.SetFloat("Game_Volume", 1.0f);
            PlayerPrefs.Save();
        }
    }

}
