using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class DeckBuilderManager : MonoBehaviour
{
    public AudioSource audio;
    public AudioMixerGroup musicMixerGroup;

    public void Awake()
    {
        audio = GetComponent<AudioSource>();

        if (audio != null && musicMixerGroup != null)
        {
            audio.outputAudioMixerGroup = musicMixerGroup;
        }
    }

    public void HangarReturn()
    {
        SceneManager.LoadScene("HangarLevel");
    }

    public void MainMenuReturn()
    {
        SceneManager.LoadScene("PT_MainMenu");
    }
}

