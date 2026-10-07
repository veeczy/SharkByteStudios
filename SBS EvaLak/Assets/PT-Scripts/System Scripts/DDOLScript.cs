using UnityEngine;

public class DDOLScript : MonoBehaviour
{
    public static DDOLScript Instance;


    void Awake()
    {
        if (Instance == null) 
        {
            Instance = this;
            DontDestroyOnLoad(transform.root.gameObject);
        }
        else 
        {
            Destroy(gameObject);
            return;
        }
    }
}
