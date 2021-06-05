using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{

    public static GameController instance;

    public Text ScoreText;
	public Text ScoreMultiplierText;
    public GameObject PauseMenuParent;
    public GameObject DeathMenuParent;
    public SButton PauseGameButton;
    public GameObject NewHighScore;
	public GameObject HighScoreFire;
    public Text UnpauseCountdownText;
    public int BossInterval;
	public float ScreenMultiplierRadiusFactor0;
	public float ScreenMultiplierRadiusFactor1;
	public Color ScoreMultiplierTextCenterColor;
	public Color ScoreMultiplierTextMiddleColor;
	public Color ScoreMultiplierTextOuterColor;
	float ScreenMultiplierRadiusSqr0;
	float ScreenMultiplierRadiusSqr1;

	public ScoreTutorial ScoreTutorial;

    public float CanvasGroupFadeInTime;
    public float CanvasGroupFadeOutTime;

    bool BossSmallFryCountdownActive;
    public bool IsGameOver;
    public bool IsGamePaused;
    int CurrentSmallFryCount = 0;
    float CurrentScore;
	float HighScore;

    CameraBlur CameraBlur;
	Camera Camera;
	Vector2 ScreenCenterPoint;
	float ScreenScoreMultiplier = 1f;
	bool ScoreTutorialActive;

	float GameStartTime;
    
    void Awake()
    {
        instance = this;

        CurrentScore = 0;
        IsGameOver = false;
        BossSmallFryCountdownActive = true;
		Camera = Camera.main;
        CameraBlur = Camera.GetComponent<CameraBlur>();
		ScreenCenterPoint = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
		SetScreenScoreMultipliers();
    }

	void SetScreenScoreMultipliers()
	{
		float radius = Screen.height * ScreenMultiplierRadiusFactor0;
		ScreenMultiplierRadiusSqr0 = radius * radius;
		radius = Screen.height * ScreenMultiplierRadiusFactor1;
		ScreenMultiplierRadiusSqr1 = radius * radius;
	}

    void Start()
    {
        PauseGameButton.OnClickEvent.AddListener(OnPauseGameClicked);
        LilB.instance.IsEndless = true;
		LilB.instance.IsChallenge = false;
		LilB.instance.IsTutorial = false;
		
        HighScore = PlayerPrefs.GetFloat(Utility.PrefsHighScoreKey, 0f);
		bool seenScoreTutorial = PlayerPrefs.GetInt(Utility.PrefsSeenScoreTutorialKey, 0) == 1;
		if (seenScoreTutorial)
		{
			GameStartTime = Time.time;
			EnemyManager.instance.TriggerSmallFryLoop();
		}
		else
		{
			ScoreTutorialActive = true;
			ScoreTutorial.Initiate(ScreenCenterPoint, ScreenMultiplierRadiusSqr0, ScreenMultiplierRadiusSqr1, Camera);
			PauseGameButton.gameObject.SetActive(false);
		}
    }

	void Update()
	{
#if UNITY_ANDROID
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (IsGameOver)
			{
				OnBackToMenu(false);
			}
			else if (IsGamePaused)
			{
				OnResumeGameClicked();
			}
			else
			{
				OnPauseGameClicked();
			}
		}
#endif

		if (!IsGameOver)
		{
			SetScoreMultiplier();
			UpdateScore();
		}
	}

	void UpdateScore()
	{
		CurrentScore += Time.deltaTime * ScreenScoreMultiplier;
		ScoreText.text = "Score: " + CurrentScore.ToString("0.0");

		if (CurrentScore >= HighScore && !ScoreTutorialActive)
		{
			HighScoreFire.SetActive(true);
		}
	}

	void SetScoreMultiplier()
	{
		Vector3 speckWorldPos = LilB.instance.transform.position;
		Vector2 speckScreenPos = Camera.WorldToScreenPoint(speckWorldPos);
		Vector2 speckVector = speckScreenPos - ScreenCenterPoint;
		float distanceSqr = speckVector.x * speckVector.x + speckVector.y * speckVector.y;
		if (distanceSqr < ScreenMultiplierRadiusSqr0)
		{
			ScreenScoreMultiplier = 2.5f;
			ScoreMultiplierText.color = ScoreMultiplierTextCenterColor;
		}
		else if (distanceSqr < ScreenMultiplierRadiusSqr1)
		{
			ScreenScoreMultiplier = 1.5f;
			ScoreMultiplierText.color = ScoreMultiplierTextMiddleColor;
		}
		else
		{
			ScreenScoreMultiplier = 1f;
			ScoreMultiplierText.color = ScoreMultiplierTextOuterColor;
		}
		ScoreMultiplierText.text = "x" + ScreenScoreMultiplier.ToString();
	}

	public void OnBossDefeated()
	{
		CurrentScore += 40f;
		BossSmallFryCountdownActive = true;
	}

    public void OnPauseGameClicked()
    {
        Time.timeScale = 0f;
        Time.fixedDeltaTime = 0f;

        StartCoroutine(LerpCanvasGroupAlpha(PauseMenuParent, true));
        IsGamePaused = true;

        CameraBlur.enabled = true;
        PauseGameButton.IsButtonActive = false;
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

    public void EndGame()
    {
        if (IsGameOver)
        {
            return;
        }

        IsGameOver = true;
		ScoreMultiplierText.gameObject.SetActive(false);

        LilB.instance.HandleDeath();

        bool newHighScore = false;
        if (CurrentScore > HighScore)
        {
            PlayerPrefs.SetFloat(Utility.PrefsHighScoreKey, CurrentScore);
            newHighScore = true;
        }

		AnalyticsManager.SendEndlessStat(Time.time - GameStartTime, CurrentScore);

        GameOverUI(newHighScore);
    }

    void GameOverUI(bool newHighScore)
    {
        PauseGameButton.IsButtonActive = false;

        StartCoroutine(LerpCanvasGroupAlpha(DeathMenuParent, true));
        if (newHighScore)
        {
            NewHighScore.SetActive(true);
            NewHighScore.GetComponent<Text>().text = "New High Score: " + CurrentScore.ToString("0.0");
        }
    }

    public void CheckBoss()
    {
        if (!BossSmallFryCountdownActive)
        {
            return;
        }

        CurrentSmallFryCount++;
        if (CurrentSmallFryCount == BossInterval)
        {
            EnemyManager.instance.TriggerBoss();
            CurrentSmallFryCount = 0;
            BossSmallFryCountdownActive = false;
        }
    }

    public void OnRestart()
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;

        SceneManager.LoadScene("game");
    }

    public void OnBackToMenu(bool fromPause)
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;

		if (fromPause)
		{
			AnalyticsManager.SendEndlessBackToMenuStat(Time.time - GameStartTime, CurrentScore);
		}
        SceneManager.LoadScene("menu");
    }
}
