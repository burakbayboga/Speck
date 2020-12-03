using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{

    public static MenuController instance;

    public float CanvasGroupFadeInTime;
    public float CanvasGroupFadeOutTime;

    public Text HighScoreText;
    public Animator EndlessWarningAnimator;
    public GameObject[] Padlocks;
    public Animator[] PadlockAnimators;

    public GameObject MainMenuParent;
    public GameObject ChallengeMenuParent;
	public GameObject ChallengeModePopupParent;
	public Image PassedUnmodifiedImage;
	public Image PassedDoubleImage;
	public Image PassedFastImage;
	public Image PassedHardcoreImage;

	public GameObject FullscreenBackButton;

	public Color ActiveModeColor;
	public Color InactiveModeColor;

    public SButton EndlessButton;

    public ChallengeButton[] ChallengeLevelButtons;
	public ChallengeModeButton SelectDoubleButton;
	public ChallengeModeButton SelectFastButton;
	public ChallengeModeButton SelectHardcoreButton;

    public SButton[] AllSButtons;

    private bool WarningActive;
    private int PlayerChallengeLevel;
    private bool CanPlayEndless;

    void Awake()
    {
        instance = this;

        int highScore = PlayerPrefs.GetInt(Utility.PrefsHighScoreKey, 0);
        HighScoreText.text = "HighScore: " + highScore.ToString();
		
        PlayerChallengeLevel = Utility.ChallengeInfo.ChallengeLevelInfoList.Count + 1;
        CanPlayEndless = PlayerChallengeLevel >= 5;
        if (!CanPlayEndless)
        {
            Padlocks[0].SetActive(true);
            Padlocks[1].SetActive(true);
            EndlessButton.IsButtonActive = false;
        }

		Utility.CurrentChallengeMode = ChallengeMode.None;
        InitializeChallengeMenu();
    }

    public void DisableAllSButtons()
    {
        for (int i = 0; i < AllSButtons.Length; i++)
        {
            AllSButtons[i].IsButtonActive = false;
        }
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

	public void OnChallengeLevelClicked(ChallengeMode modes, bool passed)
	{
		InitPassedImage(PassedUnmodifiedImage, passed);
		InitPassedImage(PassedDoubleImage, Utility.IsDouble(modes));
		InitPassedImage(PassedFastImage, Utility.IsFast(modes));
		InitPassedImage(PassedHardcoreImage, Utility.IsHardcore(modes));

		StartCoroutine(LerpCanvasGroupAlpha(ChallengeModePopupParent, true));
		FullscreenBackButton.SetActive(true);
	}

	private void InitPassedImage(Image image, bool passed)
	{
		Color color;
		float alpha;

		color = image.color;
		alpha = passed ? 1f : 0f;
		color.a = alpha;
		image.color = color;
	}

	public void OnChallengeModeHardcoreClicked(bool isSelected)
	{
		if (isSelected)
		{
			SelectDoubleButton.SelectMode();
			SelectDoubleButton.SetActivity(false);
			SelectFastButton.SelectMode();
			SelectFastButton.SetActivity(false);
		}
		else
		{
			SelectDoubleButton.UnselectMode();
			SelectDoubleButton.SetActivity(true);
			SelectFastButton.UnselectMode();
			SelectFastButton.SetActivity(true);
		}
	}

	public void OnPlayChallengeButtonClicked()
	{
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
		List<PlayerChallengeLevelInfo> infoList = Utility.ChallengeInfo.ChallengeLevelInfoList;

		for (int i = 0; i < ChallengeLevelButtons.Length; i++)
		{
			bool isActive;
			ChallengeMode modes;
			bool passed;
			if (i < infoList.Count)
			{
				isActive = true;
				modes = infoList[i].Modes;
				passed = true;
			}
			else if (i == infoList.Count)
			{
				isActive = true;
				modes = ChallengeMode.None;
				passed = false;
			}
			else
			{
				isActive = false;
				modes = ChallengeMode.None;
				passed = false;		
			}

			ChallengeLevelButtons[i].InitChallengeButton(modes, i, isActive, passed);
		}
    }
    
    public void OnChallengeButtonClicked()
    {
        StartCoroutine(LerpCanvasGroupAlpha(MainMenuParent, false));
        StartCoroutine(LerpCanvasGroupAlpha(ChallengeMenuParent, true));
    }

	public void OnFullscreenBackButtonClicked()
	{
		StartCoroutine(LerpCanvasGroupAlpha(ChallengeModePopupParent, false));
		ResetModeSelectPopup();
		FullscreenBackButton.SetActive(false);
	}

	private void ResetModeSelectPopup()
	{
		SelectDoubleButton.UnselectMode();
		SelectDoubleButton.SetActivity(true);
		SelectFastButton.UnselectMode();
		SelectFastButton.SetActivity(true);
		SelectHardcoreButton.UnselectMode();
	}

    public void OnTutorialButtonClicked()
    {
        SceneManager.LoadScene("tutorial");
    }

}
