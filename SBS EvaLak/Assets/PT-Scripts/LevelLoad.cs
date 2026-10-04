using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class LevelLoad : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private string sceneToLoad;

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Loading scene");
        SceneManager.LoadScene(sceneToLoad);
    }
}