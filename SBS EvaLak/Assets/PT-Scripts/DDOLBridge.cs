using UnityEngine;

public class DDOLBridge : MonoBehaviour
{
    private GameObject DDOL;
    private Inventory inventory;

    private GameObject addRemoveRef;
    private AddRemoveCardScript addRemoveScript;
    [SerializeField] private string cardType;
    void Start()
    {
        Init();
    }
    public void AddRemoveCard()
    {
        if (inventory == null)
        {
            Debug.Log("Inventory not found");
            return;
        }
        if (addRemoveScript.isAdd)
        {
            inventory.AddCardByType(cardType);
            return;
        }
        else
        {
            inventory.RemoveCardByType(cardType);
            return;
        }
    }

    void Init()
    {
        DDOL = GameObject.Find("DDOL");
        if (DDOL == null)
        {
            Debug.Log("DDOL not found");
            return;
        }

        addRemoveRef = GameObject.Find("Add/Remove Cards");
        if (addRemoveRef == null)
        {
            Debug.Log("Add/Remove not found");
            return;
        }

        inventory = DDOL.GetComponent<Inventory>();
        addRemoveScript = addRemoveRef.GetComponent<AddRemoveCardScript>();
    }
}
