using UnityEngine;

public class PlanetSpawner : MonoBehaviour
{
    private GameObject DDOL;
    private Inventory inventory;
    private string planetInfo;

    [SerializeField] private GameObject naturePlanet;
    [SerializeField] private GameObject roundPlanet;
    [SerializeField] private GameObject robotPlanet;
    [SerializeField] private GameObject undeadPlanet;
    void Awake()
    {
        InitPlanets();
        DDOL = GameObject.Find("DDOL");
        inventory = DDOL.GetComponent<Inventory>();
        planetInfo = inventory.planetInfo;

        switch (planetInfo)
        {
            case "Nature":
                naturePlanet.SetActive(true);
                break;
            case "Robot":
                robotPlanet.SetActive(true);
                break;
            case "Round":
                roundPlanet.SetActive(true);
                break;
            case "Undead":
                undeadPlanet.SetActive(true);
                break;
            default:
                break;
        }
    }

    void InitPlanets()
    {
        naturePlanet.SetActive(false);
        roundPlanet.SetActive(false);
        robotPlanet.SetActive(false);
        undeadPlanet.SetActive(false);
    }
}
