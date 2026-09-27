using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pausePanel;
    public AudioSource audioSource;
    public Slider audioSlider;

    public void Start()
    {
        pausePanel.SetActive(false);

        float savedVolume = PlayerPrefs.GetFloat("Game_Volume", 1.0f);

        audioSlider.value = savedVolume;   
        audioSource.volume = savedVolume;

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
