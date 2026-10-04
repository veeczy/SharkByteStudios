using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Healthbar : MonoBehaviour
{
    public int health;
    public int maxHealth = 250;

    public bool showDamageNumbers = true;
    public TextMeshPro damageNumberPrefab;
    public float damageNumberSpeed = 0.5f;
    public float damageNumberDuration = 1.5f;

    public Image healthbarImage;
    public TextMeshProUGUI healthText;

    private int lastHealth;

    void Start()
    {
        health = maxHealth;
        lastHealth = health;
    }

    void Update()
    {
        // Detect health change by comparing current health to health last frame
        if (health != lastHealth)
        {
            if (showDamageNumbers) SpawnDamageNumber(health - lastHealth);
            lastHealth = health;
        }

        healthText.text = health.ToString() + "/" + maxHealth.ToString();

        healthbarImage.fillAmount = (float)health / (float)maxHealth;
    }

    private void SpawnDamageNumber(int change)
    {
        // Instantiate damage number at healthbar position
        TextMeshPro text = Instantiate(damageNumberPrefab, healthbarImage.transform.position, healthbarImage.transform.rotation);
        text.text = change.ToString("+0;-0"); // + for health gained, - for health lost

        // Destroy after animation, even if this healthbar is destroyed first
        Destroy(text.gameObject, damageNumberDuration);
        StartCoroutine(AnimateDamageNumber(text));
    }

    // Animate number floating down while fading out
    private IEnumerator AnimateDamageNumber(TextMeshPro text)
    {
        Color color = text.color;

        for (float elapsed = 0f; elapsed < damageNumberDuration; elapsed += Time.deltaTime)
        {
            if (text == null) yield break; // already destroyed

            text.transform.position += Vector3.down * damageNumberSpeed * Time.deltaTime;

            color.a = 1f - elapsed / damageNumberDuration;
            text.color = color;

            yield return null;
        }
    }
}