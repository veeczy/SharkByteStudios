using UnityEngine;

public class Deck : MonoBehaviour
{
    [SerializeField] private DeckManager deckManager;

    void Start()
    {
        deckManager.DrawCards();
    }
}
