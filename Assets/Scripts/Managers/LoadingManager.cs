using UnityEngine;
using UnityEngine.SceneManagement;


public class LoadingManager : MonoBehaviour
{

    private void Start()
    {
        LoadInitialScene();
    }

    private void LoadInitialScene()
    {
        bool playedTutorial = PlayerPrefs.GetInt(Utility.PrefsPlayedTutorialKey, 0) == 0 ? false : true;

        if (playedTutorial)
        {
            SceneManager.LoadScene("menu");
        }
        else
        {
            SceneManager.LoadScene("basic_tutorial");
        }
    }

}
