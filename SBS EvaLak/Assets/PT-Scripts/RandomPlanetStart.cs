using UnityEngine;

public class RandomPlanetStart : MonoBehaviour
{
    public GameObject[] randomPlanetStart;

    public void Start()
    {
        foreach (GameObject planets in randomPlanetStart)
        {
            planets.SetActive(false);
        }
        
        int randomIndex = Random.Range(0, randomPlanetStart.Length);

        randomPlanetStart[randomIndex].SetActive(true);
    }
}
