using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class HangarManager : MonoBehaviour
{
    public AudioSource audio;
    public AudioMixerGroup musicMixerGroup;
    public Camera playerCam;

    public GameObject loadingPanel;

    [Header("Level Select Cutscene")]
    public GameObject mech;
    public GameObject backButton;
    public GameObject[] planets;

    public Transform defaultPosition;
    public Transform levelSelect;

    public float moveDuration = 2f;
    public float fadeDuration = 1.5f;


    [Header("Deck Build Cutscene")]
    public Transform deckBuildDoorLeft;
    public Transform deckBuildDoorRight;
    public Transform doorOpenLeftTarget;
    public Transform doorOpenRightTarget;
    public Transform cameraMoveTarget;

    public float doorOpenDuration = 1.5f;
    public float cameraMoveDuration = 2f;
    
    


    private bool isStarting;

    

    public void Awake()
    {
        if (playerCam == null)
        {
            playerCam = Camera.main;
        }
        
        audio = GetComponent<AudioSource>();

        if (audio != null && musicMixerGroup != null)
        {
            audio.outputAudioMixerGroup = musicMixerGroup;
        }

        foreach (GameObject planet in planets)
        {
            planet.SetActive(false);
        }

        backButton.SetActive(false);
    }

    public virtual void OpenLevelSelect()
    {
        if (!isStarting && playerCam != null)
        {
            StartCoroutine(LevelSelectCutscene());
        }
    }
    public IEnumerator LevelSelectCutscene()
    {
        isStarting = true;

        Transform camTransform = playerCam.transform;
        
        Vector3 startPosition = camTransform.position;
        Quaternion startrotation = camTransform.rotation;

        Vector3 targetPosition = levelSelect.position;
        Quaternion targetRotation = levelSelect.rotation;

        float elapsedTime = 0f;

        StartCoroutine(FadeInPlanets());

        while ( elapsedTime < moveDuration)
        {
            float t = elapsedTime / moveDuration;

            float smoothT = Mathf.SmoothStep(0f, 1f, t);

            camTransform.position = Vector3.Lerp(startPosition, targetPosition, smoothT);
            camTransform.rotation = Quaternion.Slerp(startrotation, targetRotation, smoothT);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        camTransform.position = targetPosition;
        camTransform.rotation = targetRotation;

        backButton.SetActive(true);

        isStarting = false;
    }
    private IEnumerator FadeInPlanets()
    {
        yield return null;

        foreach (GameObject planet in planets)
        {
            planet.SetActive(true);

            Renderer render = planet.GetComponent<Renderer>();
            Material mat = render.material;

            Color color = mat.color;
            color.a = 0f;
            mat.color = color;

            float elapsedTime = 0f;

            while (elapsedTime < fadeDuration)
            {
                float t = elapsedTime / fadeDuration;

                color.a = Mathf.Lerp(0f, 1f, t);
                mat.color = color;

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            color.a = 0.5f;
            mat.color = color;
        }
    }
    public void BackToDefault()
    {
        if (!isStarting)
        {
            backButton.SetActive(false);

            foreach (GameObject planet in planets)
            {
                planet.SetActive(false);
            }

            StartCoroutine(ReturnToDefault());
        }
    }
    private IEnumerator ReturnToDefault()
    {
        isStarting = true;

        Transform camTransform = playerCam.transform;

        Vector3 startPosition = camTransform.position;
        Quaternion startRotation = camTransform.rotation;

        float elapsedTime = 0f;

        while ( elapsedTime < moveDuration)
        {
            float t = elapsedTime / moveDuration;
            float smoothT = Mathf.SmoothStep(0f, 1f, t);

            camTransform.position = Vector3.Lerp(startPosition, defaultPosition.position, smoothT);
            camTransform.rotation = Quaternion.Slerp(startRotation, defaultPosition.rotation, smoothT);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        camTransform.position = defaultPosition.position;
        camTransform.rotation = defaultPosition.rotation;
        
        isStarting = false;
    }
    public virtual void OpenDeckBuild()
    {
        if (!isStarting)
        {
            StartCoroutine(DeckBuildCutscene());
        }
    }
    public IEnumerator DeckBuildCutscene()
    {

        Vector3 startPositionLeft = deckBuildDoorLeft.transform.position;
        Vector3 startPositionRight = deckBuildDoorRight.transform.position;
        Vector3 targetPositionLeft = doorOpenLeftTarget.position;
        Vector3 targetPositionRight = doorOpenRightTarget.position;

        float elapsedTime = 0f;

        while (elapsedTime < doorOpenDuration)
        {
            float t = elapsedTime / doorOpenDuration;

            float smoothT = Mathf.SmoothStep(0f, 1f, t);

            deckBuildDoorLeft.position = Vector3.Lerp(startPositionLeft, targetPositionLeft, smoothT);
            deckBuildDoorRight.position = Vector3.Lerp(startPositionRight, targetPositionRight, smoothT);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

        /*Vector3 cameraTargetPosition = cameraMoveTarget.position;

        Transform camTransform = playerCam.transform;

        Vector3 cameraStartPosition = camTransform.position;

        elapsedTime = 0f;

        while (elapsedTime < cameraMoveDuration)
        {
            float t = elapsedTime / cameraMoveDuration;

            float smoothT = Mathf.SmoothStep(0f, 1f, t);

            camTransform.position = Vector3.Lerp(cameraStartPosition, cameraTargetPosition, smoothT);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        camTransform.position = cameraTargetPosition;

    }*/
    public virtual void ReturnToMainMenu()
    {
        SceneManager.LoadScene("PT_MainMenu");
    }
}
