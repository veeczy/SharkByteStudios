using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Healthbar : MonoBehaviour
{
    public int health;
    public int maxHealth = 250;

    public Image healthbarImage;
    public TextMeshProUGUI healthText;

    public 

    void Start()
    {
        health = maxHealth;
    }

    void Update()
    {
        healthText.text = health.ToString() + "/" + maxHealth.ToString();

        healthbarImage.fillAmount = (float)health / (float)maxHealth;
    }
}
