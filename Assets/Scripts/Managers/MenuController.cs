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
	public GameObject EndlessWarning;
	public GameObject EndlessChain;

    public GameObject MainMenuParent;
    public GameObject ChallengeMenuParent;
	public GameObject ChallengeModePopupParent;
	public GameObject PassedNormalImage;
	public GameObject PassedHardcoreImage;

	public Text TotalStarCountText;
	private int TotalStarCount;

	public GameObject FullscreenBackButton;

	public Color ActiveModeColor;
	public Color InactiveModeColor;

    public SButton EndlessButton;
	public Image EndlessChainRenderer;

    public ChallengeButton[] ChallengeLevelButtons;
	public ChallengeModeButton SelectNormalButton;
	public ChallengeModeButton SelectHardcoreButton;

    private bool WarningActive;
    private bool CanPlayEndless;

    void Awake()
    {
        instance = this;
    }

	void Start()
	{
		float highScore = PlayerPrefs.GetFloat(Utility.PrefsHighScoreKey, 0f);
        HighScoreText.text = "HighScore: " + highScore.ToString("0.0");
		
		SetTotalStarCount();

		bool endlessUnlocked = PlayerPrefs.GetInt(Utility.PrefsEndlessUnlocked, 0) == 1;
        CanPlayEndless = TotalStarCount >= 8;
        if (!CanPlayEndless)
        {
			EndlessChain.SetActive(true);
			EndlessButton.SetActivity(false);
			HighScoreText.gameObject.SetActive(false);
        }
		else if (!endlessUnlocked)
		{
			EndlessChain.SetActive(true);
			StartCoroutine(UnlockEndless());
			PlayerPrefs.SetInt(Utility.PrefsEndlessUnlocked, 1);
		}

		Utility.CurrentChallengeMode = ChallengeMode.None;
        InitializeChallengeMenu();

		LilB.instance.IsEndless = false;
		LilB.instance.IsChallenge = false;
		LilB.instance.IsTutorial = false;
	}

	IEnumerator UnlockEndless()
	{
		float startTime = Time.time;
		float t = 0f;
		Vector4 st = Vector4.one;
		while (t < 1f)
		{
			t = (Time.time - startTime) / 2f;
			st.z = -Mathf.Lerp(0f, 1f, t);
			st.w = st.z / 2f;
			EndlessChainRenderer.material.SetVector("_TextureTiling", st);

			yield return null;
		}
		EndlessChain.SetActive(false);
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
			EndlessWarning.SetActive(true);
            StartCoroutine(EndlessWarningTimer());
        }
    }

	public void OnChallengeLevelClicked(ChallengeMode passedModes)
	{
		PassedNormalImage.SetActive(Utility.IsNormal(passedModes));
		PassedHardcoreImage.SetActive(Utility.IsHardcore(passedModes));

		if (!Utility.IsNormal(passedModes))
		{
			SelectHardcoreButton.LockMode();
		}

		SelectNormalButton.Select();

		StartCoroutine(LerpCanvasGroupAlpha(ChallengeModePopupParent, true));
		FullscreenBackButton.SetActive(true);
	}

	public void OnNormalModeClicked()
	{
		SelectHardcoreButton.Unselect();
		SelectNormalButton.Select();
	}

	public void OnHardcoreModeClicked()
	{
		SelectNormalButton.Unselect();
		SelectHardcoreButton.Select();
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
		EndlessWarning.SetActive(false);
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

	private void SetTotalStarCount()
	{
		List<PlayerChallengeLevelInfo> infoList = Utility.ChallengeInfo.ChallengeLevelInfoList;
		
		for (int i = 0; i < infoList.Count; i++)
		{
			TotalStarCount++;
			
			if (Utility.IsHardcore(infoList[i].Modes))
			{
				TotalStarCount++;
			}
		}

		TotalStarCountText.text = TotalStarCount.ToString() + " x";
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
		SelectNormalButton.Reset();
		SelectHardcoreButton.Reset();
	}
}
