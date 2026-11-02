using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Healthbar : MonoBehaviour
{
    public int health;
    public int maxHealth = 500;

    public bool showDamageNumbers = true;
    public TextMeshPro damageNumberPrefab;
    public float damageNumberSpeed = 0.5f;
    public float damageNumberDuration = 1.5f;

    public Image healthbarImage;
    public TextMeshProUGUI healthText;

    private int lastHealth;
    public bool isPlayer = false; //check if player

    private Inventory inventory;
    private GameObject DDOL;
    [SerializeField] private string HeadSlot;
    [SerializeField] private string ArmsSlot;
    [SerializeField] private string LegsSlot;
    [SerializeField] private int HeadValue;
    [SerializeField] private int ArmsValue;
    [SerializeField] private int LegsValue;

    void Start()
    {
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

        Debug.Log(maxHealth);
        if (isPlayer) { CheckMech(); }

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

    void CheckMech()
    {
        float addHealth;
        if(HeadSlot == "health") 
        {
            addHealth = IntPercentage(HeadValue); //convert to percentage 
            addHealth = maxHealth * addHealth; //calc new health
            maxHealth = Mathf.RoundToInt(addHealth); //new max health is rounded to int
        }

        if(ArmsSlot == "health")
        {
            addHealth = IntPercentage(ArmsValue); //convert to percentage 
            addHealth = maxHealth * addHealth; //calc new health
            maxHealth = Mathf.RoundToInt(addHealth); //new max health is rounded to int
        }

        if(LegsSlot == "health")
        {
            addHealth = IntPercentage(LegsValue); //convert to percentage 
            addHealth = maxHealth * addHealth; //calc new health
            maxHealth = Mathf.RoundToInt(addHealth); //new max health is rounded to int
        }
    }

    float IntPercentage(int value)
    {
        float floatvalue = ((float)value) / 100; //make percentage
        Debug.Log(floatvalue);
        floatvalue = floatvalue + 1; //plus 1 so its instead of 5% -> 105% 
        Debug.Log(floatvalue);
        return floatvalue;
    }
}