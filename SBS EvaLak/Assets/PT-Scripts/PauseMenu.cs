using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject pausePanel;
    public AudioSource audioSource;
    public Slider audioSlider;

    public float slideInSpeed;

    public void Start()
    {
        pausePanel.SetActive(false);

        audioSource = GetComponent<AudioSource>();

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
