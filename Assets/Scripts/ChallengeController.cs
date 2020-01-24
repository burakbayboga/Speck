using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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
    public GameObject RetryUIParent;
    public GameObject PauseMenuParent;
    public SButton PauseGameButton;
    public Text UnpauseCountdownText;
    public Text CurrentLevelText;
    public Text LevelOverText;
    public TextAsset ChallengeTextAsset;

    private ChallengeData ChallengeData;
    private ChallengeLevel CurrentLevel;
    private int CurrentWaveIndex;
    private ChallengeWave CurrentWave;
    private int CurrentLevelIndex;
    private int PlayerHighestLevel;

    private LilB LilB;

    private CameraBlur CameraBlur;

    private Coroutine WatchForLevelEndCoroutine;
    private bool WatchForLevelEndActive;

    private bool IsBossActive;
    private Coroutine BossTimerCoroutine;

    private Coroutine SpawnCoroutine;
    private bool IsSpawnCoroutineActive;

    public SButton[] AllSButtons;

    public bool IsGamePaused;
    public bool IsGameOver;

    public void OnBackToMenuButtonCLicked()
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;

        SceneManager.LoadScene("menu");
    }

    public void Awake()
    {
        instance = this;

        LilB = FindObjectOfType<LilB>();
        LilB.IsChallenge = true;

        CameraBlur = Camera.main.GetComponent<CameraBlur>();

        InitChallenge();
    }

    private void Start()
    {
        PauseGameButton.OnClickEvent.AddListener(OnPauseGameClicked);
    }

    private void InitChallenge()
    {

        //StreamReader reader = new StreamReader("Assets/Resources/ChallengeData.txt");
        //string challengeText = reader.ReadToEnd();

        string challengeText = ChallengeTextAsset.text;

        ChallengeData = JsonConvert.DeserializeObject<ChallengeData>(challengeText);

        PlayerHighestLevel = PlayerPrefs.GetInt(Utility.PrefsChallengeLevelKey, 0);

        CurrentLevelIndex = PlayerPrefs.GetInt(Utility.PrefsCurrentChallengeLevelKey, 0);

        CurrentLevel = ChallengeData.Levels[CurrentLevelIndex];
        CurrentWaveIndex = 0;

        CurrentLevelText.text = "Level : " + (CurrentLevelIndex + 1);

        SpawnCoroutine = StartCoroutine(SpawnLoop());
    }

    private IEnumerator SpawnLoop()
    {
        IsSpawnCoroutineActive = true;
        float delay;

        for (int i=0; i < CurrentLevel.Waves.Count; i++)
        {
            CurrentWaveIndex = i;
            CurrentWave = CurrentLevel.Waves[CurrentWaveIndex];

            if (i == 0)
            {
                delay = CurrentWave.Time;
            }
            else
            {
                delay = CurrentWave.Time - CurrentLevel.Waves[i - 1].Time;
            }

            yield return new WaitForSeconds(delay);
            for (int j=0; j < CurrentWave.Enemies.Count; j++)
            {
                SpawnEnemy(CurrentWave.Enemies[j]);
            }

            if (i == CurrentLevel.Waves.Count - 1)
            {
                WatchForLevelEndCoroutine = StartCoroutine(WatchForLevelEnd());    
            }
        }

        IsSpawnCoroutineActive = false;
    }

    private IEnumerator BossTimer(float bossTime)
    {
        yield return new WaitForSeconds(bossTime);
        IsBossActive = false;
    }

    private IEnumerator WatchForLevelEnd()
    {
        WatchForLevelEndActive = true;

        while (true)
        {
            if (SmallFryManager.instance.SmallFryExists() || IsBossActive)
            {
                yield return null;
            }
            else
            {
                StartCoroutine(LerpCanvasGroupAlpha(LevelOverUIParent, true));
                PauseGameButton.IsButtonActive = false;
                LevelOverText.text = "Level " + (CurrentLevelIndex + 1) + " Completed !!";
                CurrentLevelIndex++;
                PlayerPrefs.SetInt(Utility.PrefsCurrentChallengeLevelKey, CurrentLevelIndex);
                if (CurrentLevelIndex > PlayerHighestLevel)
                {
                    PlayerHighestLevel = CurrentLevelIndex;
                    PlayerPrefs.SetInt(Utility.PrefsChallengeLevelKey, PlayerHighestLevel);
                }
                yield break;
            }
        }
    }

    public void OnNextLevelClicked()
    {
        StartCoroutine(LerpCanvasGroupAlpha(LevelOverUIParent, false));

        CurrentWaveIndex = 0;

        CurrentLevel = ChallengeData.Levels[CurrentLevelIndex];
        CurrentLevelText.text = "Level : " + (CurrentLevelIndex + 1);

        SpawnCoroutine = StartCoroutine(SpawnLoop());
        PauseGameButton.IsButtonActive = true;
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
            IsBossActive = true;
            BossTimerCoroutine = StartCoroutine(BossTimer(spawnedBoss.EncounterTime));
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

        if (IsBossActive)
        {
            StopCoroutine(BossTimerCoroutine);
        }

        LilB.HandleDeath();
        StartCoroutine(LerpCanvasGroupAlpha(RetryUIParent, true));
    }

    public void OnRetryClicked()
    {
        SceneManager.LoadScene("challenge");
    }

    public void DisableAllSButtons()
    {
        for (int i = 0; i < AllSButtons.Length; i++)
        {
            AllSButtons[i].IsButtonActive = false;
        }
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
        CameraBlur.enabled = false;
    }

    IEnumerator ResumeAfterCountdown()
    {
        StartCoroutine(LerpCanvasGroupAlpha(PauseMenuParent, false));

        WaitForSecondsRealtime secondsToWait = new WaitForSecondsRealtime(0.5f);
        UnpauseCountdownText.text = "3";
        yield return secondsToWait;
        UnpauseCountdownText.text = "2";
        yield return secondsToWait;
        UnpauseCountdownText.text = "1";
        yield return secondsToWait;
        UnpauseCountdownText.text = string.Empty;

        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;

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
