using UnityEngine;
using UnityEngine.SceneManagement;

public class HangarManager : MonoBehaviour
{
    public AudioSource audio;

    public void Awake()
    {
        audio = GetComponent<AudioSource>();
    }

    public virtual void OpenLevelSelect()
    {
        SceneManager.LoadScene("PT_Level_Select");
    }
    public virtual void OpenDeckBuild()
    {
        SceneManager.LoadScene("PT_Deck_Build");
    }
    public virtual void ReturnToMainMenu()
    {
        SceneManager.LoadScene("PT_MainMenu");
    }
}
