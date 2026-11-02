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

    public int energyCapacity = 5;

    public Image energyBar;
    public TextMeshProUGUI energyText;

    private Inventory inventory;
    private GameObject DDOL;
    [SerializeField] private string HeadSlot;
    [SerializeField] private string ArmsSlot;
    [SerializeField] private string LegsSlot;
    [SerializeField] private int HeadValue;
    [SerializeField] private int ArmsValue;
    [SerializeField] private int LegsValue;

    void Awake()
    {
        Instance = this;
        if (DDOL == null) { DDOL = GameObject.Find("DDOL"); inventory = DDOL.GetComponent<Inventory>(); }
        if (inventory != null)
        {
            HeadSlot = inventory.HeadCardSlot;
            ArmsSlot = inventory.ArmsCardSlot;
            LegsSlot = inventory.LegsCardSlot;

            HeadValue = inventory.HeadCardSlotValue;
            ArmsValue = inventory.ArmsCardSlotValue;
            LegsValue = inventory.LegsCardSlotValue;
        }

        CheckMech();
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
        if (Keyboard.current.jKey.wasPressedThisFrame) //debug to KILL THE ENEMY IM TIRED OF FIGHTING THIS IN PLAYTEST
        {
            GameObject enemyHealth = GameObject.Find("EnemyHealthbar");
            Healthbar healthbar = enemyHealth.GetComponent<Healthbar>();
            healthbar.health = 0;
        }

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

    void CheckMech()
    {
        if (HeadSlot == "energy")
        {
            energyCapacity = energyCapacity + HeadValue;
        }

        if (ArmsSlot == "energy")
        {
            energyCapacity = energyCapacity + ArmsValue;
        }

        if (LegsSlot == "energy")
        {
            energyCapacity = energyCapacity + LegsValue;
        }
    }
}