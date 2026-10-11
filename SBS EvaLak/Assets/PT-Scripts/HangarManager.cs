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

    [Header("Deck Build Cutscene")]
    
    
    public float speed;
    public float fallSpeed = 5f;
    public float fallDuration = 2f;

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
        SceneManager.LoadScene("PT_Deck_Build");
    }
    public virtual void ReturnToMainMenu()
    {
        SceneManager.LoadScene("PT_MainMenu");
    }
}
