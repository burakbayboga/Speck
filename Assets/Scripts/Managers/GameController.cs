using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{

    public static GameController instance;

    public Text ScoreText;
    //public GameObject BackToMenuButton;
    //public GameObject RestartButton;
    public GameObject PauseMenuParent;
    public GameObject DeathMenuParent;
    public SButton PauseGameButton;
    public GameObject NewHighScore;
    public GameObject BossEncounterIntroParent;
    public Text UnpauseCountdownText;
    public int BossInterval;
    public bool BossSmallFryCountdownActive;

    public SButton[] AllSButtons;

    public float CanvasGroupFadeInTime;
    public float CanvasGroupFadeOutTime;

    private LilB Speck;

    public int CurrentScore;
    public bool IsGameOver;
    public bool IsGamePaused;
    Coroutine TimerCoroutine;
    int CurrentSmallFryCount = 0;

    Animator BossEncounterIntroParentAnimator;

    CameraBlur CameraBlur;
    
    void Awake()
    {
        instance = this;

        CurrentScore = 0;
        IsGameOver = false;
        UpdateScoreText();
        TimerCoroutine = StartCoroutine(Timer());
        BossSmallFryCountdownActive = true;
        BossEncounterIntroParentAnimator = BossEncounterIntroParent.GetComponent<Animator>();
        Speck = FindObjectOfType<LilB>();
        CameraBlur = Camera.main.GetComponent<CameraBlur>();
    }

    void Start()
    {
        PauseGameButton.OnClickEvent.AddListener(OnPauseGameClicked);
        Speck.IsEndless = true;
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

    public void EndGame()
    {
        if (IsGameOver)
        {
            return;
        }

        IsGameOver = true;

        Speck.HandleDeath();

        StopCoroutine(TimerCoroutine);
        bool newHighScore = false;
        int previousHighScore = PlayerPrefs.GetInt(Utility.PrefsHighScoreKey, 0);
        if (CurrentScore > previousHighScore)
        {
            PlayerPrefs.SetInt(Utility.PrefsHighScoreKey, CurrentScore);
            newHighScore = true;
        }

        GameOverUI(newHighScore);
    }

    void GameOverUI(bool newHighScore)
    {
        PauseGameButton.IsButtonActive = false;

        StartCoroutine(LerpCanvasGroupAlpha(DeathMenuParent, true));
        if (newHighScore)
        {
            NewHighScore.SetActive(true);
            NewHighScore.GetComponent<Text>().text = "New High Score: " + CurrentScore.ToString();
        }
    }

    void UpdateScoreText()
    {
        ScoreText.text = "Score: " + CurrentScore.ToString();
    }

    IEnumerator Timer()
    {
        WaitForSeconds secondsToWait = new WaitForSeconds(1f);
        
        while (true)
        {
            yield return secondsToWait;
            CurrentScore += 1;
            UpdateScoreText();
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

    public void TriggerBossEncounterIntro()
    {
        BossEncounterIntroParent.SetActive(true);
        BossEncounterIntroParentAnimator.Play("boss_encounter_intro");
    }

    public void DeactivateBossEncounterIntro()
    {
        BossEncounterIntroParent.SetActive(false);
    }

    public void OnRestart()
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;

        SceneManager.LoadScene("game");
    }

    public void OnBackToMenu()
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;

        SceneManager.LoadScene("menu");
    }

    public void DisableAllSButtons()
    {
        for (int i = 0; i < AllSButtons.Length; i++)
        {
            AllSButtons[i].IsButtonActive = false;
        }
    }


}
