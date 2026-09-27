using UnityEngine;
using UnityEngine.SceneManagement;

public class DeckBuilderManager : HangarManager
{

    public override void OpenLevelSelect()
    {
        SceneManager.LoadScene("PT_Hangar");
    }
}

