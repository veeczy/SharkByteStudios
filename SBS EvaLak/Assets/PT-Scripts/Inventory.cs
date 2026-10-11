using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Inventory : MonoBehaviour
{
    public List<string> fullDeckList; //this is all the possible cards
    public List<string> deckList; //this is what the player owns
    public List<GameObject> cardGameObjectList; //to put the prefabs from the scene into when the cards are matching

    private GameObject tempGameObject;

    public bool cardScene = false;
    private List<string> sceneNameList = new List<string>() { "PT_Deck_Build","BattleSceneTest"};
    public string sceneName;

    private void Start()
    {
        //temporarily, until ui for selecting which cards in deck, this will just fill our chosen deck with 25 of the possible cards
        int deckLength = 25;

        while(deckList.Count < deckLength)
        {
            int randomIndex = Random.Range(0,fullDeckList.Count);
            string tempElement = fullDeckList[randomIndex];

            if(!deckList.Contains(tempElement)) { deckList.Add(tempElement);}
        }
    }

    public void OnLevelWasLoaded() //at start of new scene loading
    {
        sceneName = SceneManager.GetActiveScene().name;
        if (sceneNameList.Contains(sceneName)) { cardScene = true; }
        else { cardScene = false; }


        if (cardScene) //if scene with cards in it
        {
            //check to see if deck game objects are in the scene and if they are append them to the list
            foreach (string name in deckList)
            {
                tempGameObject = GameObject.Find(name); //see if card with name is in scene
                if (tempGameObject != null) { cardGameObjectList.Add(tempGameObject); } // if so add to game object list
                else { Debug.Log("Could not find " + name); } // if not then break to next one
            }
        }
    }
}
