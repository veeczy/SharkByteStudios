using UnityEngine;

public class EndManager : MonoBehaviour
{
    [SerializeField] private Deck deck;
    [SerializeField] private Healthbar playerHP;
    [SerializeField] private Healthbar enemyHP;

    // Update is called once per frame
    void Update()
    {
        if (!deck.isPlaying && enemyHP.health <= 0)
        {
            Debug.Log("WIN!!!!!!");
        }
        else if (!deck.isPlaying && playerHP.health <= 0)
        {
            Debug.Log("LOSE!!!!!!");
        }
    }
}
