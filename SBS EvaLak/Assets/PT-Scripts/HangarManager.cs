using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HangarManager : MonoBehaviour
{
    public AudioSource audio;
    public Camera playerCam;

    public GameObject loadingPanel;

    [Header("Level Select Cutscene")]
    public GameObject hangarBottomPlane;
    public GameObject mech;

    public Transform playerTarget;
    public Transform mechTarget;

    public float speed;
    public float fallSpeed = 5f;
    public float fallDuration = 2f;

    [Header("Deck Build Cutscene")]
    public Transform deckBuildDoor;
    public Transform doorOpenTarget;
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
        Quaternion startRotation = camTransform.rotation;

        Vector3 targetPosition = playerTarget.position - (playerTarget.forward * 2f);
        Quaternion targetRotation = Quaternion.LookRotation(mechTarget.transform.position - startPosition); 

        float elapsedTime = 0f;

        while (elapsedTime < speed)
        {
            float t = elapsedTime / speed;

            float smoothT = Mathf.SmoothStep(0f, 1f, t);

            camTransform.rotation = Quaternion.Slerp(startRotation, targetRotation, smoothT);

            elapsedTime += Time.deltaTime;
            yield return null;

        }

        camTransform.rotation = targetRotation;

        hangarBottomPlane.SetActive(false);

        yield return new WaitForSeconds(0.5f);

        float fallTime = 0f;

        while (fallTime < fallDuration)
        {
            Vector3 fallMovement = Vector3.down * fallSpeed * Time.deltaTime;

            camTransform.position += fallMovement;
            mech.transform.position += fallMovement;

            fallTime += Time.deltaTime;
            yield return null;
        }

        loadingPanel.SetActive(true);

        SceneManager.LoadScene("PT_Level_Select");
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

        Vector3 startPosition = deckBuildDoor.transform.position;
        Vector3 targetPosition = doorOpenTarget.position;

        float elapsedTime = 0f;

        while (elapsedTime < doorOpenDuration)
        {
            float t = elapsedTime / doorOpenDuration;

            float smoothT = Mathf.SmoothStep(0f, 1f, t);

            deckBuildDoor.position = Vector3.Lerp(startPosition, targetPosition, smoothT);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        deckBuildDoor.position = targetPosition;

        Vector3 cameraTargetPosition = cameraMoveTarget.position;

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

        loadingPanel.SetActive(true);

        SceneManager.LoadScene("PT_Deck_Build");
    }
    public virtual void ReturnToMainMenu()
    {
        SceneManager.LoadScene("PT_MainMenu");
    }
}
