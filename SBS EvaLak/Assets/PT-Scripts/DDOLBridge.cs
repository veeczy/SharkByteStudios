using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DDOLBridge : MonoBehaviour
{
    private GameObject DDOL;
    private Inventory inventory;

    private GameObject addRemoveRef;
    private AddRemoveCardScript addRemoveScript;

    public GameObject myButtonGameObject;
    public Button myButton;
    [SerializeField] private string cardType;

    void Start()
    {
        Init();
    }

    public void AddRemoveCard()
    {
        Debug.Log("BUTTON CLICKED");

        myButtonGameObject = EventSystem.current.currentSelectedGameObject;
        if (myButtonGameObject == null) { Debug.Log("Unable to find GameObject."); }
        else { myButton = myButtonGameObject.GetComponent<Button>(); cardType = myButtonGameObject.name; Debug.Log("Selected " + cardType); }

        if (cardType == "") { Debug.Log("No card type found!"); }

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
