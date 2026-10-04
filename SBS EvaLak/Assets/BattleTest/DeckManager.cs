using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;

public class DeckManager : MonoBehaviour
{
    // Cards are prefabs and can't reference scene objects, so they reach this through Instance
    public static DeckManager Instance { get; private set; }

    [SerializeField] private Deck deck;
    [SerializeField] private CardZone hand;

    public int handSize = 7;

    public int energyCapacity = 8;

    public Image energyBar;
    public TextMeshProUGUI energyText;

    void Awake()
    {
        Instance = this;
    }

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

        // Recalculated every frame from the selected cards, so it also updates when cards are deselected
        int totalEnergy = SelectedEnergy();
        energyBar.fillAmount = (float)totalEnergy / (float)energyCapacity;
        energyText.text = totalEnergy + "/" + energyCapacity;
    }

    // Whether selecting this card keeps the total energy of the selected cards within the energy capacity
    public bool CanSelect(Card card)
    {
        return SelectedEnergy() + card.energyCost <= energyCapacity;
    }

    // Total energy cost of the cards currently selected in the hand
    private int SelectedEnergy()
    {
        int total = 0;

        foreach (Card card in hand.GetSelected())
            total += card.energyCost;

        return total;
    }

    private void InitialDraw()
    {
        deck.DrawCards(handSize);
    }
}