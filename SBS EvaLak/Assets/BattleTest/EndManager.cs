using UnityEngine;
using UnityEngine.SceneManagement;

public class EndManager : MonoBehaviour
{
    [SerializeField] private Deck deck;
    [SerializeField] private Healthbar playerHP;
    [SerializeField] private Healthbar enemyHP;
    [SerializeField] private string winScene;
    [SerializeField] private string loseScene;

    [SerializeField] private SceneFade sceneFade;

    private void Start()
    {
        if(sceneFade == null) { GameObject temp = GameObject.Find("FadeManager"); sceneFade = temp.GetComponent<SceneFade>(); }
    }

    // Update is called once per frame
    void Update()
    {
        if (!deck.isPlaying && enemyHP.health <= 0)
        {
            if(sceneFade != null) { sceneFade.FadeAndLoadScene(winScene); }
            else { SceneManager.LoadScene(winScene); }
        }
        else if (!deck.isPlaying && playerHP.health <= 0)
        {
            if(sceneFade != null) { sceneFade.FadeAndLoadScene(loseScene); }
            else { SceneManager.LoadScene(loseScene); }
        }
    }
}
