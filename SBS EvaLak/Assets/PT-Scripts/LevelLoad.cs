using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class LevelLoad : MonoBehaviour
{
    [SerializeField] private string sceneName;
    public HangarManager hangarManager;

    [SerializeField] private string planetName;
    Inventory inventory;
    [SerializeField] private GameObject DDOL;

    void Start()
    {
        DDOL = GameObject.Find("DDOL");
        inventory = DDOL.GetComponent<Inventory>();
    }

    public void OpenLevel()
    {
        hangarManager.StartBattle(sceneName);

        inventory.planetInfo = planetName;
    }
}