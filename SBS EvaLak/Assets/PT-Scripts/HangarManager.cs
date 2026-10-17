using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class HangarManager : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource audio;
    public AudioMixerGroup musicMixerGroup;

    [Header("Camera Properties")]
    public Camera playerCam;

    public GameObject loadingPanel;

    [Header("Level Select Cutscene")]
    public GameObject mech;
    public GameObject[] planets;

    public Transform defaultPosition;
    public Transform levelSelect;

    public float moveDuration = 2f;
    public float fadeDuration = 1.5f;

    [Header("Default Spot")]
    public Transform defaultTransform;

    [Header("Deck Build Cutscene")]
    public Transform deckBuildDoorLeft;
    public Transform deckBuildDoorRight;
    public Transform doorOpenLeftTarget;
    public Transform doorOpenRightTarget;
    public Transform deckBuildCamTarget;

    [Header("Shop Cutscene")]
    public Transform shopDoorLeft;
    public Transform shopDoorRight;
    public Transform shopDoorOpenLeftTarget;
    public Transform shopDoorOpenRightTarget;
    public Transform shopCamTarget;

    [Header("Mech Upgrades Cutscene")]
    public Transform mechUpgradesTransform;

    [Header("Cutscene Variables")]
    public float doorOpenDuration = 1.5f;
    public float cameraMoveDuration = 2f;

    private bool isStarting;
    private Vector3 deckLeftClosed;
    private Vector3 deckRightClosed;
    private Vector3 shopLeftClosed;
    private Vector3 shopRightClosed;

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

        deckLeftClosed = deckBuildDoorLeft.position;
        deckRightClosed = deckBuildDoorRight.position;

        shopLeftClosed = shopDoorLeft.position;
        shopRightClosed = shopDoorRight.position;

    }

    #region LevelSelectCutsceneLogic
    public virtual void OpenLevelSelect()
    {
        if (!isStarting && playerCam != null)
        {
            CloseAllDoors();
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
    #endregion

    #region DeckBuildCutsceneLogic
    public virtual void OpenDeckBuild()
    {
        if (!isStarting)
        {
            StartCoroutine(CloseDoors(shopDoorLeft, shopDoorRight, shopLeftClosed, shopRightClosed));

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

        Transform camTransform = playerCam.transform;

        Vector3 cameraStartPosition = camTransform.position;
        Quaternion cameraStartRotation = camTransform.rotation;

        Vector3 cameraTargetPosition = deckBuildCamTarget.position;
        Quaternion cameraTargetRotation = deckBuildCamTarget.rotation;

        elapsedTime = 0f;

        while (elapsedTime < cameraMoveDuration)
        {
            float t = elapsedTime / cameraMoveDuration;

            float smoothT = Mathf.SmoothStep(0f, 1f, t);

            camTransform.position = Vector3.Lerp(cameraStartPosition, cameraTargetPosition, smoothT);
            camTransform.rotation = Quaternion.Slerp(cameraStartRotation, cameraTargetRotation, smoothT);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        camTransform.position = cameraTargetPosition;
        camTransform.rotation = cameraTargetRotation;

    }
    #endregion

    #region ShopCutsceneLogic
    public virtual void OpenShop()
    {
        if (!isStarting)
        {
            StartCoroutine(CloseDoors(deckBuildDoorLeft, deckBuildDoorRight, deckLeftClosed, deckRightClosed));

            StartCoroutine(ShopCutscene());
        }
    }
    public IEnumerator ShopCutscene()
    {
        Vector3 startPositionLeft = shopDoorLeft.transform.position;
        Vector3 startPositionRight = shopDoorRight.transform.position;
        Vector3 targetPositionLeft = shopDoorOpenLeftTarget.position;
        Vector3 targetPositionRight = shopDoorOpenRightTarget.position;

        float elapsedTime = 0f;

        while (elapsedTime < doorOpenDuration)
        {
            float t = elapsedTime / doorOpenDuration;

            float smoothT = Mathf.SmoothStep(0f, 1f, t);

            shopDoorLeft.position = Vector3.Lerp(startPositionLeft, targetPositionLeft, smoothT);
            shopDoorRight.position = Vector3.Lerp(startPositionRight, targetPositionRight, smoothT);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Transform camTransform = playerCam.transform;

        Vector3 cameraStartPosition = camTransform.position;
        Quaternion cameraStartRotation = camTransform.rotation;

        Vector3 cameraTargetPosition = shopCamTarget.position;
        Quaternion cameraTargetRotation = shopCamTarget.rotation;

        elapsedTime = 0f;

        while (elapsedTime < cameraMoveDuration)
        {
            float t = elapsedTime / cameraMoveDuration;

            float smoothT = Mathf.SmoothStep(0f, 1f, t);

            camTransform.position = Vector3.Lerp(cameraStartPosition, cameraTargetPosition, smoothT);
            camTransform.rotation = Quaternion.Slerp(cameraStartRotation, cameraTargetRotation, smoothT);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        camTransform.position = cameraTargetPosition;
        camTransform.rotation = cameraTargetRotation;
    }
    #endregion

    #region MechUpgradesLogic
    public virtual void MechUpgrades()
    {
        if (!isStarting)
        {
            CloseAllDoors();
            StartCoroutine(MechUpgradesCutscene());
        }
    }
    public IEnumerator MechUpgradesCutscene()
    {
        Transform camTransform = playerCam.transform;

        Vector3 camStartPosition = camTransform.position;
        Quaternion camStartRotation = camTransform.rotation;

        Vector3 camTargetPosition = mechUpgradesTransform.position;
        Quaternion camTargetRotation = mechUpgradesTransform.rotation;

        float elapsedTime = 0f;

        while (elapsedTime < moveDuration)
        {
            float t = elapsedTime / moveDuration;

            float smoothT = Mathf.SmoothStep(0f, 1f, t);

            camTransform.position = Vector3.Lerp(camStartPosition, camTargetPosition, smoothT);
            camTransform.rotation = Quaternion.Slerp(camStartRotation, camTargetRotation, smoothT);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        camTransform.position = camTargetPosition;
        camTransform.rotation = camTargetRotation;
    }
    #endregion

    #region DefaultPosition
    public virtual void DefaultPosition()
    {
        if (!isStarting)
        {
            StartCoroutine(ReturnToDefault());
        }
    }
    public IEnumerator ReturnToDefault()
    {
        Transform camTransform = playerCam.transform;
        
        Vector3 currentPosition = camTransform.position;
        Quaternion currentRotation = camTransform.rotation;

        Vector3 defaultPosition = defaultTransform.position;
        Quaternion defaultRotation = defaultTransform.rotation;

        float elapsedTime = 0f;

        while (elapsedTime < moveDuration)
        {
            float t = elapsedTime / moveDuration;

            float smoothT = Mathf.SmoothStep(0f, 1f, t);

            camTransform.position = Vector3.Lerp(currentPosition, defaultPosition, smoothT);
            camTransform.rotation = Quaternion.Slerp(currentRotation, defaultRotation, smoothT);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        camTransform.position = defaultPosition;
        camTransform.rotation = defaultRotation;
    }
    #endregion

    #region CloseAllDoor
    private void CloseAllDoors()
    {
        StartCoroutine(CloseDoors(deckBuildDoorLeft, deckBuildDoorRight, deckLeftClosed, deckRightClosed));

        StartCoroutine(CloseDoors(shopDoorLeft, shopDoorRight, shopLeftClosed, shopRightClosed));
    }
    private IEnumerator CloseDoors(Transform leftDoor, Transform rightDoor, Vector3 leftPosition, Vector3 rightPosition)
    {
        Vector3 startLeft = leftDoor.position;
        Vector3 startRight = rightDoor.position;

        float elapsedTIme = 0f;

        while (elapsedTIme < doorOpenDuration)
        {
            float t = elapsedTIme / doorOpenDuration;
            float smoothT = Mathf.SmoothStep(0f, 1f, t);

            leftDoor.position = Vector3.Lerp(startLeft, leftPosition, smoothT);
            rightDoor.position = Vector3.Lerp(startRight, rightPosition, smoothT);

            elapsedTIme += Time.deltaTime;
            yield return null;
        }

        leftDoor.position = leftPosition;
        rightDoor.position = rightPosition;

    }
    #endregion
    public virtual void ReturnToMainMenu()
    {
        SceneManager.LoadScene("PT_MainMenu");
    }
}
