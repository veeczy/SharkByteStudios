using UnityEngine;
using UnityEngine.SceneManagement;

public class EndManager : MonoBehaviour
{
    [SerializeField] private Deck deck;
    [SerializeField] private Healthbar playerHP;
    [SerializeField] private Healthbar enemyHP;
    [SerializeField] private string winScene;
    [SerializeField] private string loseScene;

    // Update is called once per frame
    void Update()
    {
        if (!deck.isPlaying && enemyHP.health <= 0)
        {
            SceneManager.LoadScene(winScene);
        }
        else if (!deck.isPlaying && playerHP.health <= 0)
        {
            SceneManager.LoadScene(loseScene);
        }
    }
}
