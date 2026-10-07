using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pausePanel;
    public Slider audioSlider;

    private GameObject DDOL;
    private AudioSource audioSource;

    public void Start()
    {
        if(DDOL == null)
        {
            DDOL = GameObject.Find("DDOL");

            audioSource = DDOL.GetComponent<AudioSource>();
        }

        pausePanel.SetActive(false);

        float savedVolume = PlayerPrefs.GetFloat("Game_Volume");

        //audioSlider.value = playerPres;
        //audioSource.volume = savedVolume;

        audioSlider.onValueChanged.AddListener(SetVolume);
    }

    public void SetVolume(float sliderValue)
    {
        audioSource.volume = (sliderValue);

        PlayerPrefs.SetFloat("Game_Volume", sliderValue);
    }

    public void PauseOpen()
    {
        pausePanel.SetActive(true);
    }
    public void PauseClose()
    {
        pausePanel.SetActive(false);
    }
}
