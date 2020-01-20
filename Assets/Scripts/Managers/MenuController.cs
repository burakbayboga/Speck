using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{

    public static MenuController instance;

    public InputField SetChallengeLevelInputField;

    public float CanvasGroupFadeInTime;
    public float CanvasGroupFadeOutTime;

    public Text HighScoreText;
    public Animator EndlessWarningAnimator;
    public GameObject[] Padlocks;
    public Animator[] PadlockAnimators;

    public GameObject MainMenuParent;
    public GameObject ChallengeMenuParent;

    public SButton EndlessButton;

    public SButton[] ChallengeLevelButtons;
    public Sprite[] ChallengeLevelBoxSprites;

    public SButton[] AllSButtons;

    private bool WarningActive;
    private int PlayerChallengeLevel;
    private bool CanPlayEndless;

    void Awake()
    {
        instance = this;

        int highScore = PlayerPrefs.GetInt(Utility.PrefsHighScoreKey, 0);
        HighScoreText.text = "HighScore: " + highScore.ToString();

        PlayerChallengeLevel = PlayerPrefs.GetInt(Utility.PrefsChallengeLevelKey, 0);
        CanPlayEndless = PlayerChallengeLevel >= 5;
        if (!CanPlayEndless)
        {
            Padlocks[0].SetActive(true);
            Padlocks[1].SetActive(true);
            EndlessButton.IsButtonActive = false;
        }

        InitializeChallengeMenu();
    }

    public void DisableAllSButtons()
    {
        for (int i = 0; i < AllSButtons.Length; i++)
        {
            AllSButtons[i].IsButtonActive = false;
        }
    }

    public void OnSetChallengeLevelButtonClicked()
    {
        int level = int.Parse(SetChallengeLevelInputField.text);
        PlayerPrefs.SetInt(Utility.PrefsChallengeLevelKey, level);
    }

    public void OnEndlessButtonClicked()
    {
        SceneManager.LoadScene("game");
    }

    public void OnEndlessButtonClickedInactive()
    {
        if (!WarningActive)
        {
            WarningActive = true;
            EndlessWarningAnimator.SetTrigger("show_warning");
            StartCoroutine(EndlessWarningTimer());
        }

        PadlockAnimators[0].SetTrigger("jiggle");
        PadlockAnimators[1].SetTrigger("jiggle");
    }

    public void OnChallengeLevelClicked(int levelIndex)
    {
        PlayerPrefs.SetInt(Utility.PrefsCurrentChallengeLevelKey, levelIndex);
        SceneManager.LoadScene("challenge");
    }

    public void OnChallengeMenuBackButtonClicked()
    {
        StartCoroutine(LerpCanvasGroupAlpha(ChallengeMenuParent, false));
        StartCoroutine(LerpCanvasGroupAlpha(MainMenuParent, true));
    }

    private IEnumerator EndlessWarningTimer()
    {
        yield return new WaitForSeconds(10.0f);
        EndlessWarningAnimator.SetTrigger("close_warning");
        yield return new WaitForSeconds(1.0f);
        WarningActive = false;
    }

    IEnumerator LerpCanvasGroupAlpha(GameObject cggo, bool activating)
    {
        float rawLerpParameter = 0f;
        float startTime = Time.time;

        CanvasGroup cg = cggo.GetComponent<CanvasGroup>();
        cg.blocksRaycasts = activating;

        while (rawLerpParameter < 1f)
        {
            rawLerpParameter = Mathf.Clamp01((Time.time - startTime) / (activating ? CanvasGroupFadeInTime : CanvasGroupFadeOutTime));
            float lerpParameter = activating ? rawLerpParameter : 1f - rawLerpParameter;
            cg.alpha = lerpParameter;

            yield return null;
        }
    }

    private void InitializeChallengeMenu()
    {
        for (int i=0; i < ChallengeLevelButtons.Length; i++)
        {
            if (i > PlayerChallengeLevel)
            {
                ChallengeLevelButtons[i].IsButtonActive = false;
            }

            // :(
            ChallengeLevelButtons[i].transform.GetChild(0).GetComponent<Image>().sprite = ChallengeLevelBoxSprites[Random.Range(0, ChallengeLevelBoxSprites.Length)];
        }
    }
    
    public void OnChallengeButtonClicked()
    {
        StartCoroutine(LerpCanvasGroupAlpha(MainMenuParent, false));
        StartCoroutine(LerpCanvasGroupAlpha(ChallengeMenuParent, true));
    }

    public void OnTutorialButtonClicked()
    {
        SceneManager.LoadScene("tutorial");
    }

}
