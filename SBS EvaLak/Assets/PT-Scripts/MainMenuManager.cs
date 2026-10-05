using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public AudioSource audio;
    public Camera playerCamera;

    public GameObject loadingPanel;

    public Transform target;
    public float zoomSpeed;

    public bool isStarting;
    public void Awake()
    {
        if (playerCamera == null)
        {
            playerCamera = Camera.main;
        }

        loadingPanel.SetActive(false);
        
        audio = GetComponent<AudioSource>();
    }
    public void PlayGame()
    {
        if (!isStarting && target != null && playerCamera != null )
        {
            StartCoroutine(ZoomAndSwitchScene());
        }
    }

    public IEnumerator ZoomAndSwitchScene()
    {
        isStarting = true;

        Transform camTransform = playerCamera.transform;

        Vector3 startPosition = camTransform.position;
        Quaternion startRotation = camTransform.rotation;

        Vector3 targetPosition = target.position - (target.forward  * 2f);
        Quaternion targetRotation = Quaternion.LookRotation(target.position - targetPosition);

        float elapsedTime = 0f;

        while (elapsedTime < zoomSpeed)
        {
            float t = elapsedTime / zoomSpeed;

            float smoothT = Mathf.SmoothStep(0f, 1f, t);

            camTransform.position = Vector3.Lerp(startPosition, targetPosition, t);
            camTransform.rotation = Quaternion.Slerp(startRotation, targetRotation, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        loadingPanel.SetActive(true);

        camTransform.position = targetPosition;
        camTransform.rotation = targetRotation;
        

        SceneManager.LoadScene("PT_Hangar");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quitting Game...");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
