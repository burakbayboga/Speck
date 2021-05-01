using UnityEngine;
using UnityEngine.UI;

public class ChallengeButton : SButton
{
	public Image ButtonImage;
	public Image UnmodifiedImg;
	public Image DoubleImg;
	public Image FastImg;
	public Image HardcoreImg;
	public Text LevelText;
	public Sprite[] ButtonSprites;
	public Image[] LevelImages;
	public Sprite[] NumberSprites;

	private int Level;
	private ChallengeMode Modes;
	private bool Passed;

	public void InitChallengeButton(ChallengeMode modes, int level, bool isActive, bool passed)
	{
		ButtonImage.sprite = ButtonSprites[Random.Range(0, ButtonSprites.Length)];
		Modes = modes;
		IsButtonActive = isActive;
		Level = level;
		SetLevelImages(level);
		Passed = passed;

		if (IsButtonActive)
		{
			InitPassedImage(DoubleImg, ChallengeMode.Double);
			InitPassedImage(FastImg, ChallengeMode.Fast);
			InitPassedImage(HardcoreImg, ChallengeMode.Hardcore);
			if (Passed)
			{
				InitPassedImage(UnmodifiedImg, ChallengeMode.None, true);
			}
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

	private void InitPassedImage(Image image, ChallengeMode comparedMode, bool forceInit = false)
	{
		Color color;
		float alpha;

		color = image.color;
		alpha = (((Modes & comparedMode) == comparedMode) || forceInit) ? 1f : 0f;
		color.a = alpha;
		image.color = color;
	}

	public void OnChallengeButtonClicked()
	{
		Utility.CurrentChallengeIndex = Level;
		MenuController.instance.OnChallengeLevelClicked(Modes, Passed);
	}

}
