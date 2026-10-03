using UnityEngine;

public class Credits : MonoBehaviour
{
    public GameObject creditPanel;

    public void Start()
    {       
        creditPanel.SetActive(false);
    }
    public void OpenCredits()
    {
        creditPanel.SetActive(true);
    }
    public void CloseCredits()
    {
        creditPanel.SetActive(false);
    }

}
