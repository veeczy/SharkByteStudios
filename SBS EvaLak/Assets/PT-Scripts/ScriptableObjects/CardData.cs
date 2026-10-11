using UnityEngine;

public enum DamageType {Kinetic, Thermal, Cryo, Warp, Psychic, Biotic, Radiation}
public enum CardStyle {Melee, Ranged, Buff, Nerf, Heal}

//ONLY TO BE READ NOT TO BE CHANGED
public enum StanceStyle {Offensive, Defensive,Neutral}


[CreateAssetMenu(fileName = "NewCardData", menuName = "ScriptableObjects")]
public class CardData : ScriptableObject
{
    [Header("Enums")]
    public DamageType damageType;
    public CardStyle cardStyle;
    public StanceStyle stanceStyle;

    [Header("Floats")] 
    public float cardNumValue; //how much damage the card does, how much you can heal, percent buffs etc.

    [Header("Strings")] 
    public string cardName;
    public string cardDescription;

    [Header("Object References")] 
    public GameObject objectMesh;
    public Material objectMaterial;

}
