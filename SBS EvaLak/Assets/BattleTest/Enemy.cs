using UnityEngine;
using TMPro;

public class Enemy : MonoBehaviour
{
    public int attackMin;
    public int attackMax;
    public int doubleDamageEvery = 3;

    private Healthbar playerHP;
    public TextMeshProUGUI warningText;

    private int tracker = 0;

    void Start()
    {
        playerHP = GameObject.FindWithTag("Player").GetComponent<Healthbar>();
    }

    public void Attack()
    {
        tracker++;

        if (tracker == doubleDamageEvery)
        {
            playerHP.health -= Random.Range(attackMin, attackMax) * 2;
            tracker = 0;
        }
        else
        {
            playerHP.health -= Random.Range(attackMin, attackMax);
        }
            
        RefreshText();
    }

    private void RefreshText()
    {
        if (tracker == doubleDamageEvery - 1)
            warningText.text = "WARNING: X2 DAMAGE NEXT ATTACK";
        else
            warningText.text = "";
    }
}
