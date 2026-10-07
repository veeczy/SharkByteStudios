using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class DeckScriptableObjectManager : MonoBehaviour
{
    public CardData[] allCardsArray;
    public List<CardData> DeckList = new List<CardData>();
    public int deckSize;
}
