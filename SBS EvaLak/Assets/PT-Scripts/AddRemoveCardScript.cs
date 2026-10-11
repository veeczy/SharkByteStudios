using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AddRemoveCardScript : MonoBehaviour
{
    [SerializeField] Button myButton;
    [SerializeField] private TMP_Text addText;
    [SerializeField] private TMP_Text removeText;

    public bool isAdd = true;

    void Update()
    {
        if (isAdd)
        {
            addText.gameObject.SetActive(true);
            removeText.gameObject.SetActive(false);
        }

        else
        {
            addText.gameObject.SetActive(false);
            removeText.gameObject.SetActive(true);
        }
    }

    public void Swap()
    {
        isAdd = !isAdd;
    }

}
