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
    public GameObject [] planetNameButton;

    public Transform defaultPosition;
    public Transform levelSelect;

    public float moveDuration = 2f;
    public float fadeDuration = 1.5f;

    [Header("Battle Deploy Cutscene")]
    public Transform platform;
    public Transform platformRaisedTarget;

    public Transform mechHeadCamTarget;
    public Transform deploymentCamTarget;
    public Transform mechLoweredTarget;
    public Transform catwalkEdgeTarget;

    public float platformMoveDuration = 2f;
    public float mechLowerDuration = 3f;

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
    private Coroutine planetFadeCoroutine;

    [Header("Buttons")]
    private GameObject deckbuildButton;
    private GameObject shopButton;
    private GameObject planetPanel;

    public void Awake()
    {
        if (playerCam == null)
        {
            playerCam = Camera.main;
        }

        //button for movement
        if (deckbuildButton == null) { deckbuildButton = GameObject.Find("DECKBUILDER"); }
        if (shopButton == null) { shopButton = GameObject.Find("SHOP"); }
        if(planetPanel == null) {planetPanel = GameObject.Find("PLANET BUTTONS");}
        //set inactive at start unless looking at it
        disableButton(deckbuildButton);
        disableButton(shopButton);

        //audio
        audio = GetComponent<AudioSource>();

        if (audio != null && musicMixerGroup != null)
        {
            audio.outputAudioMixerGroup = musicMixerGroup;
        }

        //init setting level select button false
        foreach (GameObject button in planetNameButton)
        {
            button.SetActive(false);
        }
        planetPanel.SetActive(false);

        //init plant fade
        foreach (GameObject planet in planets)      
        {
            planet.SetActive(false);
        }

        //init door pos for shop and deckbuild
        deckLeftClosed = deckBuildDoorLeft.position;
        deckRightClosed = deckBuildDoorRight.position;

        shopLeftClosed = shopDoorLeft.position;
        shopRightClosed = shopDoorRight.position;

    }

    #region LevelSelectCutsceneLogic
    public virtual void OpenLevelSelect()
    {
        if (!isStarting && playerCam != null)       //level select button push start 
        {
            HidePlanets();
            CloseAllDoors();
            StartCoroutine(LevelSelectCutscene());
        }
    }
    public IEnumerator LevelSelectCutscene()    //turn player towards level select, fade in planets 
    {
        isStarting = true;

        Transform camTransform = playerCam.transform;

        Vector3 startPosition = camTransform.position;
        Quaternion startrotation = camTransform.rotation;

        Vector3 targetPosition = levelSelect.position;
        Quaternion targetRotation = levelSelect.rotation;

        float elapsedTime = 0f;

        planetFadeCoroutine = StartCoroutine(FadeInPlanets());    //planets fade in as player moves towards panel 

        while (elapsedTime < moveDuration)
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
    private IEnumerator FadeInPlanets()      //planets fade in as they load 
    {
        yield return null;
        planetPanel.SetActive(true);
        for (int i = 0; i <planets.Length; i++)
        {
            planets[i].SetActive(true);
            planetNameButton[i].SetActive(true);

            Renderer render = planets[i].GetComponent<Renderer>();
            Material mat = render.material;

            CanvasGroup buttonCanvas = planetNameButton[i].GetComponent<CanvasGroup>();

            Color color = mat.color;
            color.a = 0f;
            mat.color = color;

            buttonCanvas.alpha = 0f;

            float elapsedTime = 0f;

            while (elapsedTime < fadeDuration)
            {
                float t = elapsedTime / fadeDuration;

                color.a = Mathf.Lerp(0f, 0.5f, t);
                mat.color = color;

                buttonCanvas.alpha = Mathf.Lerp(0f, 1f, t);

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            color.a = 0.5f;
            mat.color = color;

            buttonCanvas.alpha = 1f;
        }
    }
    private void HidePlanets()    //hide planets for when the player goes to a different seftion
    {
        if (planetFadeCoroutine != null)
        {
            StopCoroutine(planetFadeCoroutine);
            planetFadeCoroutine = null;
        }

        foreach (GameObject planet in planets)
        {
            planet.SetActive(false);
        }

        foreach (GameObject button in planetNameButton)
        {
            button.SetActive(false);
        }

        planetPanel.SetActive(false);
    }
    #endregion

    #region DeckBuildCutsceneLogic
    public virtual void OpenDeckBuild()     //deck build button push start 
    {
        if (!isStarting)
        {
            HidePlanets();

            StartCoroutine(CloseDoors(shopDoorLeft, shopDoorRight, shopLeftClosed, shopRightClosed));

            StartCoroutine(DeckBuildCutscene());
        }
    }
    public IEnumerator DeckBuildCutscene()       //opens and closes doors when interacted with respective buttons
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

        Transform camTransform = playerCam.transform;                   //player moving 

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

        enableButton(deckbuildButton);
    }
    #endregion

    #region ShopCutsceneLogic
    public virtual void OpenShop()           //shop open button
    {
        if (!isStarting)
        {
            HidePlanets();

            StartCoroutine(CloseDoors(deckBuildDoorLeft, deckBuildDoorRight, deckLeftClosed, deckRightClosed));

            StartCoroutine(ShopCutscene());
        }
    }
    public IEnumerator ShopCutscene()       //shop open doors 
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

        Transform camTransform = playerCam.transform;            //player move 

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

        enableButton(shopButton);
    }
    #endregion

    #region MechUpgradesLogic
    public virtual void MechUpgrades()      //go to mech button
    {
        if (!isStarting)
        {
            HidePlanets();
            CloseAllDoors();
            StartCoroutine(MechUpgradesCutscene());
        }
    }
    public IEnumerator MechUpgradesCutscene()       //moves player to transform in front of mech 
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
    public virtual void DefaultPosition()   //go back to default middle position
    {
        if (!isStarting)
        {
            HidePlanets();

            StartCoroutine(ReturnToDefault());
        }
    }
    public IEnumerator ReturnToDefault()        //moves player from current poisition back to middle position
    {
        Transform camTransform = playerCam.transform;

        Vector3 currentPosition = camTransform.position;
        Quaternion currentRotation = camTransform.rotation;

        Vector3 defaultPosition = defaultTransform.position;

        float elapsedTime = 0f;

        while (elapsedTime < moveDuration)
        {
            float t = elapsedTime / moveDuration;

            float smoothT = Mathf.SmoothStep(0f, 1f, t);

            camTransform.position = Vector3.Lerp(currentPosition, defaultPosition, smoothT);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        camTransform.position = defaultPosition;
    }
    #endregion

    #region CloseAllDoor
    private void CloseAllDoors()   // co routine to close all doors when the player moves to a different location
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

        disableButton(deckbuildButton);
        disableButton(shopButton);

    }
    #endregion

    #region BattleDeploymentLogic
    public void StartBattle(string sceneName)       //button to start the cutscene called by the planet select 
    {
        if (!isStarting)
        {
            HidePlanets();

            StartCoroutine(BattleDeploymentCutscene(sceneName));
        }
    }
    public IEnumerator BattleDeploymentCutscene(string sceneName)       //goes through each co routine in order 
    {
        isStarting = true;

        yield return StartCoroutine(MoveObject(playerCam.transform, catwalkEdgeTarget, cameraMoveDuration));   //player moves to the edge of the catwalk 

        yield return StartCoroutine(MoveObject(platform, platformRaisedTarget, platformMoveDuration));   //platform rises to show the path to the mech 

        yield return StartCoroutine(MoveObject(playerCam.transform, mechHeadCamTarget, cameraMoveDuration));  //player walks towards designated mech target (chest or head)

        playerCam.transform.SetPositionAndRotation(deploymentCamTarget.position, deploymentCamTarget.rotation);   //camera switches to lower target that shows the mech going down

        //yield return new WaitForSeconds(1f);  //1 second delay

        StartCoroutine(MoveObject(mech.transform, mechLoweredTarget, mechLowerDuration));  //mech lowers

        yield return new WaitForSeconds(Mathf.Max(0f, mechLowerDuration - 0.3f));  //timer for scene to load before the mech reaches the transform 

        loadingPanel.SetActive(true);

        SceneManager.LoadScene(sceneName);
    }
    public IEnumerator MoveObject(Transform obj, Transform target, float duration)       //movement logic
    {
        Vector3 startPosition = obj.position;
        Quaternion startRotation = obj.rotation;

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;

            float smoothT = Mathf.SmoothStep(0f, 1f, t);

            obj.position = Vector3.Lerp(startPosition, target.position, smoothT);
            obj.rotation = Quaternion.Slerp(startRotation, target.rotation, smoothT);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        obj.position = target.position;
        obj.rotation = target.rotation;
    }
    #endregion
    public virtual void ReturnToMainMenu()
    {
        SceneManager.LoadScene("PT_MainMenu");
    }

    public void GoToScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void enableButton(GameObject button) { button.SetActive(true); }
    public void disableButton(GameObject button) { button.SetActive(false); }
}
