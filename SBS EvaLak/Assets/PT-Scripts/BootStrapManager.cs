using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BootStrapManager : MonoBehaviour
{
    public Slider loadingProgressBar;
    public float minimumLoadTime = 2f;

    private void Start()
    {
        StartCoroutine(LoadMainMenu());
    }
    
    private IEnumerator LoadMainMenu()
    {
        AsyncOperation loading = SceneManager.LoadSceneAsync("PT_MainMenu");

        loading.allowSceneActivation = false;

        float elapsedTime = 0f;

        while (elapsedTime < minimumLoadTime)
        {
            elapsedTime += Time.deltaTime;

            float progress = Mathf.Clamp01(elapsedTime / minimumLoadTime);

            loadingProgressBar.value = Mathf.Min(progress, loading.progress / 0.9f);

            yield return null;
        }

        loadingProgressBar.value = 1f;

        loading.allowSceneActivation = true;
    }
}
