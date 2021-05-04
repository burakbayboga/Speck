using UnityEngine;
using UnityEngine.UI;

public class ChallengeButton : SButton
{
	public Image ButtonImage;
	public GameObject UnmodifiedImg;
	public GameObject DoubleImg;
	public GameObject FastImg;
	public GameObject HardcoreImg;
	public Sprite[] ButtonSprites;
	public Image[] LevelImages;
	public Sprite[] NumberSprites;
	public GameObject Chain;

	private int Level;
	private ChallengeMode Modes;
	private bool Passed;

	public void InitChallengeButton(ChallengeMode modes, int level, bool isActive, bool passed)
	{
		ButtonImage.sprite = ButtonSprites[Random.Range(0, ButtonSprites.Length)];
		Modes = modes;
		IsButtonActive = isActive;
		SetActivity(isActive);
		Level = level;
		SetLevelImages(level);
		Passed = passed;

		if (IsButtonActive)
		{
			UnmodifiedImg.SetActive(Passed);
			DoubleImg.SetActive(Utility.IsDouble(Modes));
			FastImg.SetActive(Utility.IsFast(Modes));
			HardcoreImg.SetActive(Utility.IsHardcore(Modes));
			
			Chain.SetActive(false);
		}
	}

	private void SetLevelImages(int level)
	{
		level++;

		LevelImages[1].sprite = NumberSprites[level % 10];

		if (level < 10)
		{
			LevelImages[0].gameObject.SetActive(false);
		}
		else
		{
			LevelImages[0].sprite = NumberSprites[level / 10];
		}
	}

	public void OnChallengeButtonClicked()
	{
		Utility.CurrentChallengeIndex = Level;
		MenuController.instance.OnChallengeLevelClicked(Modes);
	}
}
