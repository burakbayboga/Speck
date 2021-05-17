using UnityEngine;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;


public class LoadingManager : MonoBehaviour
{

    private void Start()
    {
		LoadChallengeInfo();
        LoadInitialScene();
    }

	private void LoadChallengeInfo()
	{
		string challengeInfoJson = PlayerPrefs.GetString(Utility.PrefsChallengeInfoKey, string.Empty);
		PlayerChallengeInfo challengeInfo;

		if (challengeInfoJson == string.Empty)
		{
			challengeInfo = new PlayerChallengeInfo();
		}
		else
		{
			challengeInfo = JsonConvert.DeserializeObject<PlayerChallengeInfo>(challengeInfoJson);
		}

		Utility.ChallengeInfo = challengeInfo;
		
	}

    private void LoadInitialScene()
    {
        bool playedTutorial = PlayerPrefs.GetInt(Utility.PrefsPlayedTutorialKey, 0) != 0;

        if (playedTutorial)
        {
            SceneManager.LoadScene("menu");
        }
        else
        {
            SceneManager.LoadScene("tutorial");
        }
    }

}
