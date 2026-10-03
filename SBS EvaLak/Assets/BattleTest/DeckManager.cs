using UnityEngine;
using UnityEngine.InputSystem;

public class DeckManager : MonoBehaviour
{
    [SerializeField] private Deck deck;

    public int handSize = 7;

    void Start()
    {
        InitialDraw();
    }

    void Update()
    {
        if (Keyboard.current.fKey.wasPressedThisFrame)
        {
            deck.DrawCards(1);
        }
    }

    private void InitialDraw()
    {
        deck.DrawCards(handSize);
    }
}
