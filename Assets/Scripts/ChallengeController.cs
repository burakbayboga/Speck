using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChallengeController : MonoBehaviour
{
    public static ChallengeController instance;

    public float CanvasGroupFadeInTime;
    public float CanvasGroupFadeOutTime;
    public GameObject[] SpawnableEnemies;
    public GameObject LevelOverUIParent;
	public GameObject NextLevelButton;
    public GameObject RetryUIParent;
    public GameObject PauseMenuParent;
    public SButton PauseGameButton;
    public Text UnpauseCountdownText;
    public Text CurrentLevelText;
    public Text LevelOverText;
    public TextAsset ChallengeTextAsset;

    private ChallengeData ChallengeData;
    private ChallengeLevel CurrentLevel;
    private ChallengeWave CurrentWave;
    private int CurrentLevelIndex;

    private CameraBlur CameraBlur;

    private Coroutine WatchForLevelEndCoroutine;
    private bool WatchForLevelEndActive;

    private int ActiveBossCount;

    private Coroutine SpawnCoroutine;
    private bool IsSpawnCoroutineActive;

    public bool IsGamePaused;
    public bool IsGameOver;

	private ChallengeMode CurrentChallengeMode;
	private ElectricFence[] EdgeFences;

	private Camera MainCamera;

    public void OnBackToMenuButtonCLicked()
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;

        SceneManager.LoadScene("menu");
    }

    void Awake()
    {
        instance = this;

		MainCamera = Camera.main;
        CameraBlur = MainCamera.GetComponent<CameraBlur>();
    }

    void Start()
    {
		LilB.instance.IsChallenge = true;
        LilB.instance.IsTutorial = false;
        LilB.instance.IsEndless = false;
        PauseGameButton.OnClickEvent.AddListener(OnPauseGameClicked);
        InitChallenge();
    }

	void OnApplicationPause(bool pause)
	{
		if (pause && !IsGameOver && !IsGamePaused)
		{
			OnPauseGameClicked();
		}
	}

    private void InitChallenge()
    {

        //StreamReader reader = new StreamReader("Assets/Resources/ChallengeData.txt");
        //string challengeText = reader.ReadToEnd();

        string challengeText = ChallengeTextAsset.text;

        ChallengeData = JsonConvert.DeserializeObject<ChallengeData>(challengeText);


        CurrentLevelIndex = Utility.CurrentChallengeIndex;

		CurrentChallengeMode = Utility.CurrentChallengeMode;

        CurrentLevel = ChallengeData.Levels[CurrentLevelIndex];

        CurrentLevelText.text = "Level : " + (CurrentLevelIndex + 1);

		if (Utility.CurrentChallengeMode == ChallengeMode.Hardcore)
		{
			CreateHardcoreEdges();
		}

        SpawnCoroutine = StartCoroutine(SpawnLoop());
    }

	private void CreateHardcoreEdges()
	{
		EdgeFences = new ElectricFence[4];

		Vector3 topRight = MainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0f));
		float edgeX = topRight.x - 0.2f;
		float edgeY = topRight.y - 0.2f;

		// right
		Vector3 masterPosition = new Vector3(edgeX, edgeY, 0f);
		Vector3 helperPosition = new Vector3(edgeX, -edgeY, 0f);
		EdgeFences[0] = CreateEdgeElectric(masterPosition, helperPosition);

		// down
		masterPosition = new Vector3(edgeX, -edgeY, 0f);
		helperPosition = new Vector3(-edgeX, -edgeY, 0f);
		EdgeFences[1] = CreateEdgeElectric(masterPosition, helperPosition);

		// left
		masterPosition = new Vector3(-edgeX, -edgeY, 0f);
		helperPosition = new Vector3(-edgeX, edgeY, 0f);
		EdgeFences[2] = CreateEdgeElectric(masterPosition, helperPosition);

		// up
		masterPosition = new Vector3(-edgeX, edgeY, 0f);
		helperPosition = new Vector3(edgeX, edgeY, 0f);
		EdgeFences[3] = CreateEdgeElectric(masterPosition, helperPosition);
	}

	private ElectricFence CreateEdgeElectric(Vector3 masterPos, Vector3 helperPos)
	{
		ElectricFence fence = Instantiate(SpawnableEnemies[(int)EnemyType.ElectricFence], masterPos, Quaternion.identity).GetComponent<ElectricFence>();
		fence.Predetermined = true;
		fence.PredeterminedHelperPosition = helperPos;
		fence.LifeTime = float.MaxValue;
		fence.Init();
		return fence;
	}

    private IEnumerator SpawnLoop()
    {
        IsSpawnCoroutineActive = true;
        float delay;

        for (int i = 0; i < CurrentLevel.Waves.Count; i++)
        {
            CurrentWave = CurrentLevel.Waves[i];

			delay = GetDelayForWave(i);

            yield return new WaitForSeconds(delay);
            for (int j = 0; j < CurrentWave.Enemies.Count; j++)
            {
                SpawnEnemy(CurrentWave.Enemies[j]);
				if (Utility.CurrentChallengeMode == ChallengeMode.Hardcore)
				{
					if (CurrentWave.Enemies[j].EnemyType != EnemyType.BlackHole
						&& CurrentWave.Enemies[j].EnemyType != EnemyType.SwissCheeseBoss
						&& CurrentWave.Enemies[j].EnemyType != EnemyType.LazerBoss)
					{
						SpawnEnemy(CurrentWave.Enemies[j]);
					}
				}
            }
        }

        WatchForLevelEndCoroutine = StartCoroutine(WatchForLevelEnd());    

        IsSpawnCoroutineActive = false;
    }

	private float GetDelayForWave(int waveIndex)
	{
		float delay;
		if (waveIndex == 0)
		{
			delay = CurrentWave.Time;
		}
		else
		{
			delay = CurrentWave.Time - CurrentLevel.Waves[waveIndex - 1].Time;
		}

		return Utility.CurrentChallengeMode == ChallengeMode.Hardcore ? (delay / 1.5f) : delay;
	}

	public void OnBossDefeated()
	{
		ActiveBossCount--;
	}

    private IEnumerator WatchForLevelEnd()
    {
        WatchForLevelEndActive = true;

        while (true)
        {
			if (IsGameOver)
			{
				yield break;
			}

            if (SmallFryManager.instance.SmallFryExists() || ActiveBossCount > 0)
            {
                yield return null;
            }
            else
            {
				if (Utility.IsTest)
				{
					SceneManager.LoadScene("challenge_editor");
					yield break;
				}
				SaveChallengeInfo();
				if (Utility.CurrentChallengeMode == ChallengeMode.Hardcore && !IsNextLevelHardcoreAvailable())
				{
					NextLevelButton.SetActive(false);
				}
				else
				{
					NextLevelButton.SetActive(true);
				}

                StartCoroutine(LerpCanvasGroupAlpha(LevelOverUIParent, true));
                PauseGameButton.IsButtonActive = false;
				if (Utility.CurrentChallengeMode == ChallengeMode.Hardcore)
				{
					LevelOverText.text = "<color=#c05E5c>Hardcore</color> Level " + (CurrentLevelIndex + 1) + " Completed !!";
				}
				else
				{
					LevelOverText.text = "Level " + (CurrentLevelIndex + 1) + " Completed !!";
				}
                CurrentLevelIndex++;
				Utility.CurrentChallengeIndex = CurrentLevelIndex;
                
				if (Utility.CurrentChallengeMode == ChallengeMode.Hardcore)
				{
					for(int i = 0; i < EdgeFences.Length; i++)
					{
						EdgeFences[i].HandleDeath();
					}
				}
                yield break;
            }
        }
    }

	private bool IsNextLevelHardcoreAvailable()
	{
		List<PlayerChallengeLevelInfo> infoList = Utility.ChallengeInfo.ChallengeLevelInfoList;
		int levelToCheck = CurrentLevelIndex + 1;
		for (int i = 0; i < infoList.Count; i++)
		{
			if (infoList[i].Level == levelToCheck)
			{
				return true;
			}
		}

		return false;
	}

	private void SaveChallengeInfo()
	{
		bool levelInfoExists = false;
		List<PlayerChallengeLevelInfo> infoList = Utility.ChallengeInfo.ChallengeLevelInfoList;
		for (int i = 0; i < infoList.Count; i++)
		{
			if (infoList[i].Level == CurrentLevelIndex)
			{
				infoList[i].Modes |= CurrentChallengeMode;
				levelInfoExists = true;
				break;
			}
		}

		if (!levelInfoExists)
		{
			PlayerChallengeLevelInfo newInfo = new PlayerChallengeLevelInfo()
			{
				Level = CurrentLevelIndex,
				Modes = CurrentChallengeMode
			};

			infoList.Add(newInfo);
		}
		
		Utility.ChallengeInfo.ChallengeLevelInfoList = infoList;
		string infoJson = JsonConvert.SerializeObject(Utility.ChallengeInfo);
		PlayerPrefs.SetString(Utility.PrefsChallengeInfoKey, infoJson);
	}

    public void OnNextLevelClicked()
    {
        StartCoroutine(LerpCanvasGroupAlpha(LevelOverUIParent, false));

        CurrentLevel = ChallengeData.Levels[CurrentLevelIndex];
		CurrentLevelText.text = "Level : " + (CurrentLevelIndex + 1);

		if (Utility.CurrentChallengeMode == ChallengeMode.Hardcore)
		{
			CreateHardcoreEdges();
		}

        SpawnCoroutine = StartCoroutine(SpawnLoop());
        PauseGameButton.IsButtonActive = true;
		ActiveBossCount = 0;
		WatchForLevelEndActive = false;
    }

    private void SpawnEnemy(ChallengeWaveEnemy enemy)
    {
    
        GameObject spawned = Instantiate(SpawnableEnemies[(int)enemy.EnemyType], SpawnPositionHelper.GetSpawnPositionForEnemy(enemy.EnemyType), Quaternion.identity);
        
        if (enemy.FryType == FryType.SmallFry)
        {
            SmallFry spawnedSmallFry = spawned.GetComponent<SmallFry>();
            spawnedSmallFry.Init();
            SmallFryManager.instance.AddToSmallFryList(spawnedSmallFry);
        }
        else
        {
            Boss spawnedBoss = spawned.GetComponent<Boss>();
            spawnedBoss.Initiate();
            ActiveBossCount++;
        }
    }

    public void ChallengeDeath()
    {
        if (IsGameOver)
        {
            return;
        }

        IsGameOver = true;
        PauseGameButton.IsButtonActive = false;

        if (IsSpawnCoroutineActive)
        {
            StopCoroutine(SpawnCoroutine);
        }

        if (WatchForLevelEndActive)
        {
            StopCoroutine(WatchForLevelEndCoroutine);
        }

        LilB.instance.HandleDeath();
        StartCoroutine(LerpCanvasGroupAlpha(RetryUIParent, true));
    }

    public void OnRetryClicked()
    {
        SceneManager.LoadScene("challenge");
    }

    public void OnPauseGameClicked()
    {
        Time.timeScale = 0f;
        Time.fixedDeltaTime = 0f;

        StartCoroutine(LerpCanvasGroupAlpha(PauseMenuParent, true));
        IsGamePaused = true;
        PauseGameButton.IsButtonActive = false;
        CameraBlur.enabled = true;
    }

    public void OnResumeGameClicked()
    {
        StartCoroutine(ResumeAfterCountdown());
    }

    IEnumerator ResumeAfterCountdown()
    {
        StartCoroutine(LerpCanvasGroupAlpha(PauseMenuParent, false));

        WaitForSecondsRealtime secondsToWait = new WaitForSecondsRealtime(0.5f);
        UnpauseCountdownText.text = "2";
        yield return secondsToWait;
        UnpauseCountdownText.text = "1";
        yield return secondsToWait;
        UnpauseCountdownText.text = string.Empty;

        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;

		CameraBlur.enabled = false;

        PauseGameButton.IsButtonActive = true;
        IsGamePaused = false;
    }

    IEnumerator LerpCanvasGroupAlpha(GameObject cggo, bool activating)
    {
        float rawLerpParameter = 0f;
        float startTime = Time.unscaledTime;

        CanvasGroup cg = cggo.GetComponent<CanvasGroup>();
        cg.blocksRaycasts = activating;

        while (rawLerpParameter < 1f)
        {
            rawLerpParameter = Mathf.Clamp01((Time.unscaledTime - startTime) / (activating ? CanvasGroupFadeInTime : CanvasGroupFadeOutTime));
            float lerpParameter = activating ? rawLerpParameter : 1f - rawLerpParameter;
            cg.alpha = lerpParameter;

            yield return null;
        }
    }

}

public enum ChallengeMode
{
	None = 0,
	Normal = 1 << 0,
	Double = 1 << 1,
	Fast = 1 << 2,
	Hardcore = 1 << 3
}
