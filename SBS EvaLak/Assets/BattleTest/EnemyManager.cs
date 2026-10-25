using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR;

public class EnemyManager : MonoBehaviour
{
    private CardZone enemyDeck;
    [SerializeField] private CardZone enemyHand;
    [SerializeField] private CardZone enemyPlayArea;
    [SerializeField] private CardZone enemyGraveyard;

    [SerializeField] private CardZone enemyHealspot;
    [SerializeField] private int handSize = 3;
    [SerializeField] private float playDelay = 1f;
    [SerializeField] private bool recycleCardsToDeck = false; // send played cards back to deck, so enemy can't run out of cards

    [SerializeField] private List<Card> cards;
    [SerializeField] private List<string> cardsType = new List<string>();
    private string cardName;
    private string enemyStrategy;

    [SerializeField] private int playerHealth;
    [SerializeField] private int playerHealthMax;
    private Healthbar PlayerHP;
    private Healthbar enemyHP;
    [SerializeField] private int enemyHealth;
    [SerializeField] private int enemyHealthMax;
    private Card playCard;
    private int index;
    private int comboCount; //how many combo cards
    private int repairCount; //how many heal cards
    private int damageCount; //how many attack cards
    private int buffCount; //how many buff mult cards
    private bool canBuff;
    private bool playedCards = false;

    void Start()
    {
        enemyDeck = GameObject.FindWithTag("EnemyPrefab").GetComponent<CardZone>();
        RefillHand();
        Init();
    }
    private void Init()
    {
        PlayerHP = GameObject.FindWithTag("Player").GetComponent<Healthbar>();
        playerHealthMax = PlayerHP.maxHealth;
        enemyHP = GameObject.FindWithTag("Enemy").GetComponent<Healthbar>();
        enemyHealthMax = enemyHP.maxHealth;
    }

    public IEnumerator Attack()
    {
        if (enemyHealth > 0) { AttackRoutine(); }
        yield return new WaitForSeconds(0.2f);
    }

    public void AttackRoutine()
    {
        UpdateCards();

        if (cards.Count > 0) //if enemy deck has more than zero cards
        {
            //here im leaving this so we can change this to a switch statement with different ai that is chosen on conditions, maybe the ai will get smarter if health is lower?
            BasicEnemyAI();
        }

        RefillHand(); //after playing cards, refill enemy deck
    }

    private void RefillHand()
    {
        while (enemyHand.Count < handSize && enemyDeck.Count > 0)
            enemyHand.Add(enemyDeck.RandomCard());

        if (enemyDeck.Count < handSize) { RefillDeck(); }
    }

    private void BasicEnemyAI()
    {
        playerHealth = PlayerHP.health;
        enemyHealth = enemyHP.health;

        CountCards(); //see how many cards are duplicates and if so which ones are (can use this later for playing cards in succession
        playedCards = false;

        if (cardsType.Contains("buff") && (cardsType.Contains("damage") || comboCount > 1)) //if you have a buff card and you have a card that deals damage
        {
            canBuff = true;
        }


        if (enemyHealth < enemyHealthMax && cardsType.Contains("repair")) //if enemy health is not full + check if any of the playable enemy cards can heal the enemy (BASIC ENEMY PRIORITIZES HEALING)
        {
            index = cardsType.IndexOf("repair");
            playCard = cards[index];

            StartCoroutine(PlayCards(playCard));
            UpdateCards();
            CountCards();
            playedCards = true;
        }
        else if (comboCount > 1) //if the enemy is holding more than one combo card, play them together
        {
            if (canBuff)
            {
                index = cardsType.IndexOf("buff");
                playCard = cards[index];

                StartCoroutine(PlayCards(playCard));
                UpdateCards();
                CountCards();
                canBuff = false;
            }
            while (cardsType.Contains("combo") && comboCount > 0) //while deck contains combo cards
            {
                index = cardsType.IndexOf("combo");
                playCard = cards[index];

                StartCoroutine(PlayCards(playCard));
                UpdateCards();
                CountCards();
                //comboCount--;
            }
            playedCards = true;
        }
        else if (cardsType.Contains("damage"))
        {
            if (canBuff)
            {
                index = cardsType.IndexOf("buff");
                playCard = cards[index];

                StartCoroutine(PlayCards(playCard));
                UpdateCards();
                CountCards();
                canBuff = false;
            }

            index = cardsType.IndexOf("damage");
            playCard = cards[index];

            StartCoroutine(PlayCards(playCard));
            UpdateCards();
            CountCards();
            playedCards = true;
        }
        else { playCard = cards[0]; StartCoroutine(PlayCards(playCard)); } //else just play the card in first slot
    }

    public void UpdateCards()
    {
        // Index 0 is the leftmost card
        cards = enemyHand.GetAllCards(); //reset card list
        cardsType = new List<string>(); //reset cardtype list

        foreach (Card card in cards) //for each card in the enemy card list, record what type of card they are
        {
            if (card.TryGetComponent<Damage>(out Damage damage)) { cardName = "damage"; }
            if (card.TryGetComponent<Heal>(out Heal repair)) { cardName = "repair"; }
            if (card.TryGetComponent<MultBuff>(out MultBuff buff)) { cardName = "buff"; }
            if (card.TryGetComponent<ComboDamage>(out ComboDamage combo)) { cardName = "combo"; }

            cardsType.Add(cardName); //record what card type is at that index
        }
    }

    public void CountCards()
    {
        var duplicateCount = cardsType.GroupBy(x => x)
            .Where(g => g.Count() > 1) //where group item with count over 1
            .Select(g => new { Item = g.Key, Count = g.Count() }); //each group is an item with the key as the name, and the count measured from the above

        foreach (var duplicate in duplicateCount)
        {
            if (duplicate.Item == "combo") { comboCount = duplicate.Count; }
            if (duplicate.Item == "repair") { repairCount = duplicate.Count; }
            if (duplicate.Item == "damage") { damageCount = duplicate.Count; }
            if (duplicate.Item == "buff") { buffCount = duplicate.Count; }
        }

    }

    public IEnumerator PlayCards(Card card)
    {
        enemyPlayArea.Add(card);

        // Wait for the card to arrive in the play area
        yield return new WaitForSeconds(playDelay);

        card.OnPlay();

        SendCard(card);
    }

    public void SendCard(Card card)
    {
        if (card.TryGetComponent(out Heal heal))
        {
            if (recycleCardsToDeck)
                enemyDeck.Add(card);
            else
            {
                enemyHealspot.Add(card);
            }

        }

        else
        {
            if (recycleCardsToDeck)
                enemyDeck.Add(card);
            else
                enemyGraveyard.Add(card);
        }


    }

    public void RefillDeck()
    {
        foreach (Card card in enemyGraveyard.GetAllCards())
        {
            enemyDeck.Add(card);
        }
    }

}