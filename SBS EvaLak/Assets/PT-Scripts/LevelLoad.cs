using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class LevelLoad : MonoBehaviour
{
    [SerializeField] private string sceneName;
    public HangarManager hangarManager;

    [SerializeField] private string planetName;
    Inventory inventory;
    [SerializeField] private GameObject DDOL;
    private List<string> deckList;

    void Start()
    {
        DDOL = GameObject.Find("DDOL");
        inventory = DDOL.GetComponent<Inventory>();
        deckList = inventory.deckList;
    }

    public void OpenLevel()
    {
        if(deckList.Count == 0) { Debug.Log("No Deck Found.");} //if you dont have a deck
        else //if you have a deck selected
        {
            hangarManager.StartBattle(sceneName);

            inventory.planetInfo = planetName; 
        }
    }
}