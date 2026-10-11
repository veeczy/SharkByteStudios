using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class LevelSelectManager : MonoBehaviour
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

    public void ReturnToHangar()
    {
        SceneManager.LoadScene("PT_Hangar");
    }
}
