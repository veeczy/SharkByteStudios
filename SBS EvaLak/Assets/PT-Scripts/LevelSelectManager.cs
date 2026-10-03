using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectManager : HangarManager
{
    public override void OpenLevelSelect()
    {
        SceneManager.LoadScene("PT_Hangar");
    }
}
