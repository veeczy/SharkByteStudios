using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pausePanel;
    public Slider audioSlider;

    public void Start()
    {
        pausePanel.SetActive(false);

        float savedVolume = PlayerPrefs.GetFloat("Game_Volume", 1.0f);

        audioSlider.SetValueWithoutNotify(savedVolume);
        AudioManager.Instance.SetMasterVolume(savedVolume);

        audioSlider.onValueChanged.AddListener(SetVolume);
    }

    public void SetVolume(float sliderValue)
    {
        AudioManager.Instance.SetMasterVolume(sliderValue);

        PlayerPrefs.SetFloat("Game_Volume", sliderValue);
        PlayerPrefs.Save();
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
