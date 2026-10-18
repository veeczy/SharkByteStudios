using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class LevelLoad : MonoBehaviour
{
    [SerializeField] private string sceneName;
    public HangarManager hangarManager;

    public void OpenLevel()
    {
        hangarManager.StartBattle(sceneName);
    }
}