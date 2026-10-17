using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneFade : MonoBehaviour
{
    [Header("Fade UI")]
    public Canvas fadeCanvas;
    public GameObject fadePanel;
    public Image fadeImage;

    [Header("Fade Data")]
    public float fadeSpeed = 1f;
    public bool isFading = false;
    public bool isBlack;
    

    void Start()
    {

        if(fadeImage == null) //if inspector is null then grab the goods
        {
            //GameObject temp = GameObject.Find("Canvas");
            //fadeCanvas = temp.GetComponent<Canvas>();
            fadePanel = GameObject.Find("FadePanel");
            fadeImage = fadePanel.GetComponent<Image>();
        }

        SetAlpha(1f); //set it to black if it is transparent on start

        if(isBlack) { StartCoroutine(FadeIn()); } //if screen starts black, fade in
    }

    private void FadeInOnLoad() //in case you need a method to call in the fade in
    {
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn() //ienumerator the method uses to fade in on scene load
    {
        yield return StartCoroutine(Fade(1f, 0f));
        isFading = false;
        isBlack = false;
    }

    public void FadeAndLoadScene(string sceneName) //callable method to fade to black and load new scene
    {
        if (!isFading)
            StartCoroutine(FadeOutAndLoad(sceneName));
    }

    private IEnumerator FadeOutAndLoad(string sceneName) //ienumerator the method uses to fade out and then load scene
    {
        isFading = true;
        yield return StartCoroutine(Fade(0f, 1f));
        isBlack = true;
        SceneManager.LoadScene(sceneName);
    }

    private IEnumerator Fade(float startAlpha, float endAlpha) //tha math behind fade
    {
        Color c = fadeImage.color;
        float alpha = startAlpha;
        c.a = alpha;
        fadeImage.color = c;

        while (!Mathf.Approximately(alpha, endAlpha))
        {
            alpha = Mathf.MoveTowards(alpha, endAlpha, fadeSpeed * Time.deltaTime);
            c.a = alpha;
            fadeImage.color = c;
            yield return null;
        }

        c.a = endAlpha;
        fadeImage.color = c;
    }

    private void SetAlpha(float alpha) //callable method to set the alpha of a canvas to specific value
    {
        Color c = fadeImage.color;
        c.a = alpha;
        fadeImage.color = c;
    }
}
